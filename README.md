# GoodMoodCombatTest

Welcome to **GoodMoodCombatTest**

## What's This?
This is a little playground for testing out:
- Third-person movement (with camera lock-on and free look)
- Combo-based melee combat (think: mash attack, chain combos)
- Dummies that take damage, play hit/death/respawn, and show floating damage numbers
- Simple UI for health bar and damage popups
- Audio feedback for hits, swings, and deaths

## Features
- **ThirdPersonController**: WASD to move, Tab to toggle lock-on. Smooth movement, strafe when locked on, and camera blending with Cinemachine.
- **PlayerCombat**: Combo system (up to 3 hits), buffered input, sword hitbox, and satisfying swoosh SFX. Animations are triggered for each attack stage.
- **Dummies (DummyPuppet)**: Take damage, play hit/death/respawn, show floating numbers, and auto-respawn after a few seconds.
- **UI**: Health bar, floating damage numbers, and a simple UI manager for registering UI elements.
- **Audio**: SFX for hits, deaths, and sword swings.

## How To Play
1. Open the `Showcase` scene (in the `Scenes` folder).
2. Move around with WASD, attack with MLB (check InputBridge for bindings), and try locking on to a dummy with Tab.

---
