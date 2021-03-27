## Features

- Animates propellers of RC drones and plays audio effects while they are being controlled

## FAQ

#### How do I get a drone?

As of this writing (March 2021), RC drones are a deployable item named `drone`, but they do not appear naturally in any loot table, nor are they craftable. However, since they are simply an item, you can use plugins to add them to loot tables, kits, GUI shops, etc. Admins can also get them with the command `inventory.give drone 1`, or spawn one in directly with `spawn drone.deployed`.

#### How do I remote-control a drone?

If a player has building privilege, they can pull out a hammer and set the ID of the drone. They can then enter that ID at a computer station and select it to start controlling the drone. Controls are `W`/`A`/`S`/`D` to move, `shift` (sprint) to go up, `ctrl` (duck) to go down, and mouse to steer.

#### Can I remove the sounds effects of animated drones?

No, not possible.

## Developer Hooks

#### OnDroneAnimationStart

- Called when a drone is about to start being animated
- Returning `false` will prevent the drone from being animated
- Returning `null` will result in the default behavior

```csharp
bool? OnDroneAnimationStart(Drone drone)
```
