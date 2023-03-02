## This plugin is obsolete

**As of the March 2023 Rust update, RC drones have animation, collision and on-death effects built into the game. This plugin no longer compiles and will not be updated. This plugin can still be downloaded to review the old implementation.** 

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
    - This option is ignored while [Better Drone Collision](https://umod.org/plugins/better-drone-collision) is loaded. Instead, collision effects will play when that plugin signals it.
  - `EffectPrefab` -- The effect prefab to run.
- `DeathEffect`
  - `Enabled` (`true` or `false`) -- While `true`, drones will run an effect on death.
  - `EffectPrefab` -- The effect prefab to run.

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
