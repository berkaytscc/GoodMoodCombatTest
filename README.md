# GoodMoodCombatTest

Hey there! 👋 Welcome to **GoodMoodCombatTest** – a Unity project where you can mess around with a third-person character, try out some combo-based sword combat, and whack some dummies for science (or fun, whatever works).

## What's This?
This is a little playground for testing out:
- Third-person movement (with camera lock-on and free look)
- Combo-based melee combat (think: mash attack, chain combos)
- Dummies that take damage, play hit/death/respawn, and show floating damage numbers
- Simple UI for health bars and damage popups
- Audio feedback for hits, swings, and deaths
- Extensible code (interfaces, singletons, etc.)

## Features
- **ThirdPersonController**: WASD to move, mouse to look, Tab to toggle lock-on. Smooth movement, strafe when locked on, and camera blending with Cinemachine.
- **PlayerCombat**: Combo system (up to 3 hits), buffered input, sword hitbox, and satisfying swoosh SFX. Animations are triggered for each attack stage.
- **Dummies (DummyPuppet)**: Take damage, play hit/death/respawn, show floating numbers, and auto-respawn after a few seconds. Health bar UI included.
- **UI**: Health bars, floating damage numbers, and a simple UI manager for registering UI elements.
- **Audio**: Plug in your own SFX for hits, deaths, and sword swings.
- **Extensible**: Use the `IDamageable` interface to make anything damageable. Singleton pattern for managers. Easy to add more stuff.

## How To Play
1. Open the `Showcase.unity` scene (in the `Scenes` folder).
2. Hit Play. Move around with WASD, attack with your input (check InputBridge for bindings), and try locking on to a dummy with Tab.
3. Hit the dummies! Watch the numbers fly. If you KO one, it'll respawn after a bit.

## Project Structure (Quick Tour)
- `0-Scripts/Player/ThirdPersonController.cs` – Movement, camera, lock-on
- `0-Scripts/Combat/PlayerCombat.cs` – Combo logic, attack triggers
- `0-Scripts/Combat/DummyPuppet.cs` – Dummy logic, health, respawn
- `0-Scripts/UI/` – Health bars, floating damage, UI base classes
- `0-Scripts/Managers/UIManager.cs` – Simple singleton UI manager
- `0-Scripts/Combat/IDamageable.cs` – Interface for anything that can take damage
- `2-Prefabs/UI/DamageNumber.prefab` – Floating damage number prefab
- `3-SFX/` – Drop your SFX here

## Extending Stuff
- Want more enemies? Just make them implement `IDamageable`.
- Want more UI? Derive from `UIElement` and register with `UIManager`.
- Want more attacks? Expand the combo logic in `PlayerCombat`.

## Notes
- This is a testbed, not a full game. Stuff is a bit rough around the edges, but it's easy to hack on.
- Animations, models, and SFX are mostly placeholders – swap them out as you like.
- If you break something, just respawn the dummies or reload the scene. No biggie.

---

Have fun! If you improve something cool, let me know. :)
