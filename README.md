## Features

- Animates propellers of RC drones and plays audio effects while they are being controlled
- Adds RC drone collision effects
- Adds RC drone on-death effects

## Configuration

```json
{
  "Animation": {
    "Enabled": true
  },
  "CollisionEffect": {
    "Enabled": true,
    "RequiredMagnitude": 40,
    "EffectPrefab": "assets/content/vehicles/modularcar/carcollisioneffect.prefab"
  },
  "DeathEffect": {
    "Enabled": true,
    "EffectPrefab": "assets/prefabs/ammo/40mmgrenade/effects/40mm_he_explosion.prefab"
  }
}
```

- `Animation`
  - `Enabled` (`true` or `false`) -- While `true`, drones will display animated propellers and play audio effects while they are being controlled.
    - Also applies when a drone is hovering via the [Drone Hover](https://umod.org/plugins/drone-hover) plugin.
    - Does not apply to drones resized via Drone Scale Manager.
- `CollisionEffect`
  - `Enabled` (`true` or `false`) -- While `true`, drones will run an effect on collision.
  - `RequiredMagnitude` -- Required collision magnitude to run a collision effect.
  - `EffectPrefab` -- The effect prefab to run.
- `DeathEffect`
  - `Enabled` (`true` or `false`) -- While `true`, drones will run an effect on death.
  - `EffectPrefab` -- The effect prefab to run.

## FAQ

#### How do I get a drone?

As of this writing (March 2021), RC drones are a deployable item named `drone`, but they do not appear naturally in any loot table, nor are they craftable. However, since they are simply an item, you can use plugins to add them to loot tables, kits, GUI shops, etc. Admins can also get them with the command `inventory.give drone 1`, or spawn one in directly with `spawn drone.deployed`.

#### How do I remote-control a drone?

If a player has building privilege, they can pull out a hammer and set the ID of the drone. They can then enter that ID at a computer station and select it to start controlling the drone. Controls are `W`/`A`/`S`/`D` to move, `shift` (sprint) to go up, `ctrl` (duck) to go down, and mouse to steer.

Note: If you are unable to steer the drone, that is likely because you have a plugin drawing a UI that is grabbing the mouse cursor. The Movable CCTV was previously guilty of this and was patched in March 2021.

#### Can I remove the sounds effects of animated drones?

No, not possible.

## Recommended compatible plugins

- [Drone Hover](https://umod.org/plugins/drone-hover) -- Allows RC drones to hover in place while not being controlled.
- [Drone Lights](https://umod.org/plugins/drone-lights) -- Adds controllable search lights to RC drones.
- [Drone Storage](https://umod.org/plugins/drone-storage) -- Allows players to deploy a small stash to RC drones.
- [Drone Turrets](https://umod.org/plugins/drone-turrets) -- Allows players to deploy auto turrets to RC drones.
- [Auto Flip Drones](https://umod.org/plugins/auto-flip-drones) -- Auto flips upside-down RC drones when a player takes control.
- [RC Identifier Fix](https://umod.org/plugins/rc-identifier-fix) -- Auto updates RC identifiers saved in computer stations to refer to the correct entity.

## Developer API

#### API_StopAnimating

Plugins can call this API to stop animating a drone. Nothing happens if the drone was not already being animated.

```csharp
void API_StopAnimating(Drone drone)
```

This is useful for plugins that resize drones since the animated delivery drone may not resize when animation begins if the drone was already resized. In such an example, it's probably better to just disable animating the drone completely by using this API method plus the hook to prevent it from starting animation.

## Developer Hooks

#### OnDroneAnimationStart

- Called when a drone is about to start being animated
- Returning `false` will prevent the drone from being animated
- Returning `null` will result in the default behavior

```csharp
bool? OnDroneAnimationStart(Drone drone)
```

#### OnDroneCollisionEffect

- Called when a drone is about to run a collision effect
- Returning `false` will prevent the effect from being run
- Returning `null` will result in the default behavior

```csharp
bool? OnDroneCollisionEffect(Drone drone, Collision collision)
```
