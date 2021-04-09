using Oxide.Core;
using System.Linq;

namespace Oxide.Plugins
{
    [Info("Animated Drones", "WhiteThunder", "1.0.0")]
    [Description("Animates propellers of RC drones and plays audio effects while they are being controlled.")]
    internal class AnimatedDrones : CovalencePlugin
    {
        #region Fields

        private const string DeliveryDronePrefab = "assets/prefabs/misc/marketplace/drone.delivery.prefab";

        #endregion

        #region Hooks

        private void OnServerInitialized()
        {
            // Delay this in case Drone Hover needs a moment to set the drones to being controlled.
            NextTick(() =>
            {
                foreach (var entity in BaseNetworkable.serverEntities)
                {
                    var drone = entity as Drone;
                    if (drone == null || !IsDroneEligible(drone))
                        continue;

                    StartAnimatingIfNotAlready(drone);
                }
            });
        }

        private void Unload()
        {
            foreach (var deliveryDrone in BaseNetworkable.serverEntities.OfType<DeliveryDrone>().ToArray())
            {
                if (deliveryDrone != null && deliveryDrone.GetParentEntity() is Drone)
                    deliveryDrone.Kill();
            }
        }

        private void OnBookmarkControl(ComputerStation computerStation, BasePlayer player, string bookmarkName, IRemoteControllable entity)
        {
            var previousDrone = GetControlledDrone(computerStation);

            // Must delay since the drone hasn't stopped being controlled yet.
            NextTick(() =>
            {
                // Delay again in case Drone Hover is going to keep it in the controlled state.
                NextTick(() =>
                {
                    if (previousDrone != null && !previousDrone.IsBeingControlled)
                        MaybeStopAnimating(previousDrone);

                    var nextDrone = entity as Drone;
                    if (nextDrone != null)
                        StartAnimatingIfNotAlready(nextDrone);
                });
            });
        }

        private void OnBookmarkControlEnded(ComputerStation station, BasePlayer player, Drone drone)
        {
            // Delay in case Drone Hover is going to keep it in the controlled state.
            NextTick(() =>
            {
                if (drone == null || drone.IsBeingControlled)
                    return;

                MaybeStopAnimating(drone);
            });
        }

        #endregion

        #region API

        private void API_StopAnimating(Drone drone)
        {
            MaybeStopAnimating(drone);
        }

        #endregion

        #region Helper Methods

        private static bool AnimateWasBlocked(Drone drone)
        {
            object hookResult = Interface.CallHook("OnDroneAnimationStart", drone);
            return hookResult is bool && (bool)hookResult == false;
        }

        private static bool IsDroneEligible(Drone drone) =>
            !(drone is DeliveryDrone);

        private static Drone GetControlledDrone(ComputerStation computerStation) =>
            computerStation.currentlyControllingEnt.Get(serverside: true) as Drone;

        private static T GetChildOfType<T>(BaseEntity entity) where T : BaseEntity
        {
            foreach (var child in entity.children)
            {
                var childOfType = child as T;
                if (childOfType != null)
                    return childOfType;
            }
            return null;
        }

        private static DeliveryDrone GetChildDeliveryDrone(Drone drone) =>
            GetChildOfType<DeliveryDrone>(drone);

        private static DeliveryDrone StartAnimatingIfNotAlready(Drone drone)
        {
            if (!drone.IsBeingControlled)
                return null;

            var deliveryDrone = GetChildDeliveryDrone(drone);
            if (deliveryDrone != null)
            {
                SetupDeliveryDrone(deliveryDrone);
                return deliveryDrone;
            }

            return TryStartAnimating(drone);
        }

        private static void MaybeStopAnimating(Drone drone)
        {
            var deliveryDrone = GetChildDeliveryDrone(drone);
            if (deliveryDrone == null)
                return;

            deliveryDrone.Kill();
        }

        private static DeliveryDrone TryStartAnimating(Drone drone)
        {
            if (AnimateWasBlocked(drone))
                return null;

            var deliveryDrone = GameManager.server.CreateEntity(DeliveryDronePrefab) as DeliveryDrone;
            if (deliveryDrone == null)
                return null;

            deliveryDrone.SetParent(drone);
            deliveryDrone.CancelInvoke(deliveryDrone.Think);
            deliveryDrone.Spawn();
            SetupDeliveryDrone(deliveryDrone);

            return deliveryDrone;
        }

        private static void SetupDeliveryDrone(DeliveryDrone deliveryDrone)
        {
            deliveryDrone.EnableSaving(false);
            deliveryDrone.EnableGlobalBroadcast(false);

            // Disable delivery drone AI.
            deliveryDrone.CancelInvoke(deliveryDrone.Think);

            // Prevent the Update() method from running.
            deliveryDrone.IsBeingControlled = true;

            // Prevent the FixedUpdate() method from running.
            deliveryDrone.lifestate = BaseCombatEntity.LifeState.Dead;

            // Remove physics.
            UnityEngine.Object.Destroy(deliveryDrone.body);

            if (deliveryDrone._mapMarkerInstance != null)
                deliveryDrone._mapMarkerInstance.Kill();
        }

        #endregion
    }
}
