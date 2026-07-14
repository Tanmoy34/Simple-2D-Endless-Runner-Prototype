# 2D Endless Runner — Unity Prototype

## Overview
A 2D endless runner built in Unity (2D physics, `Rigidbody2D`). The player
stays at a fixed X position while ground and obstacles scroll toward it —
this keeps the collision/physics setup simple and avoids camera-follow
complexity within the deadline.

## Architecture

| Script | Responsibility |
|---|---|
| `GameManager` | Owns game state (`Playing` / `GameOver`), current scroll speed (difficulty scaling), restart. |
| `GameEvents` | Static event hub — the Observer pattern backbone. Decouples gameplay from UI/audio. |
| `GameConfig` | ScriptableObject holding all tunable values (speed, spawn rate, jump force). |
| `PlayerController` | Jump / double jump input, ground check, obstacle collision → triggers game over. |
| `EndlessGroundSpawner` / `GroundSegment` | Recycles ground tiles instead of instantiating new ones. |
| `ObstacleSpawner` / `Obstacle` | Spawns obstacles from an `ObjectPool<Obstacle>` on a randomized timer. |
| `ObjectPool<T>` | Generic reusable pool — used by `ObstacleSpawner`. |
| `ScoreManager` | Pure gameplay logic, no UI references. Raises `OnScoreChanged`. |
| `UIManager` | Only script that touches UI. Listens to `GameEvents`. |
| `SoundManager` | Bonus — plays SFX on jump / game over via events. |

## Design Patterns Used

- **Object Pool Pattern** — `ObjectPool<T>` reused by `ObstacleSpawner` so
  obstacles are never `Instantiate`/`Destroy`d at runtime, only
  activated/deactivated.
- **Observer Pattern** — `GameEvents` static C# events. `ScoreManager`,
  `UIManager`, `SoundManager`, `PlayerController`, and the spawners all
  communicate exclusively through events, with zero direct references to
  each other. This is also what keeps gameplay and UI logic separated.
- **State Pattern** — `GameManager.CurrentState` (`GameState` enum) is the
  single source of truth for whether the game is playing or over; every
  other script branches off `GameManager.Instance.IsPlaying` instead of
  scattering booleans.

## SOLID Notes
- **Single Responsibility**: each script does exactly one job (spawning,
  scoring, UI, audio, pooling).
- **Open/Closed**: `ObjectPool<T>` is generic and works for any pooled
  component without modification; new obstacle types just need a new
  prefab.
- **Dependency Inversion**: gameplay scripts depend on the `GameEvents`
  abstraction, not on concrete UI/audio classes.

## Optimization
- Object pooling for obstacles (no runtime `Instantiate`/`Destroy`).
- Ground segments are recycled, not destroyed.
- All per-frame logic early-returns when the game isn't in the `Playing`
  state, avoiding unnecessary work while on the game-over screen.

## Controls
- **Keyboard**: Space / configured "Jump" input to jump (press again in
  air for double jump).
- **Mobile**: tap anywhere on screen to jump (bonus).

## Setup
See `SETUP_GUIDE.md` for exact editor wiring steps (GameObjects,
prefabs, tags, layers, Inspector references).

## Bonus Features Implemented
- Mobile tap-to-jump controls
- Sound effects (event-driven, decoupled)
- ScriptableObject config (`GameConfig`) for all tuning values
- Difficulty scaling (scroll speed increases over time, capped by
  `maxScrollSpeed`)
