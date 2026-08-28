# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Memorize** is a Unity 2020.3.32f1 (LTS) memory card-matching game by TheDrGame. Players flip cards to find matching pairs. Primary target is Android (portrait) with desktop support.

## Build & Run

There is no Makefile, npm, or CI pipeline. All building is done through the Unity Editor:

- **Open project**: File → Open Project → `D:\Git\unity-samples\Memorize`
- **Play in editor**: `Ctrl+P` or press the Play button
- **Standalone build**: File → Build Settings → select scenes → Build
- **Android APK**: File → Build Settings → switch platform to Android → Build

The Unity Test Framework package (`com.unity.test-framework`) is installed but **no tests have been written** — there are no `EditMode` or `PlayMode` test scripts in the project.

There are no linting or static analysis tools configured.

## Scene Flow

The game uses a linear scene graph with `GameManager` (singleton, `DontDestroyOnLoad`) handling all transitions:

```
Preload → Start (main menu) → Game
                           ↘ Options
                           ↘ Credits
```

Scenes must be registered in `ProjectSettings/EditorBuildSettings.asset`. Scene transitions go through `GameManager.cs` using `SceneManager.LoadScene()`.

## Architecture

**Core Scripts** (`Assets/Script/`):

- **`GameManager.cs`** — Singleton scene controller, persists across scenes via `DontDestroyOnLoad`. Entry point for all scene navigation.
- **`PlayerController.cs`** — Central game logic: tracks `CardsRemaining` (starts at 20, decrements by 2 on match), holds `FirstSelected`/`LastSelected` GameObjects, manages the `ListOfTuples` of card pairs. Win condition triggers when `CardsRemaining == 0`.
- **`CardController.cs`** — Per-card interaction via `OnMouseDown()`. Handles flip animation (Z-axis rotation coroutine), calls back into `PlayerController` for match validation, and plays SFX.
- **`ButtonsManager.cs`** — Wires UI buttons to `GameManager` scene transitions at runtime via `Button.onClick` listeners.
- **`Music.cs`** — Background music singleton with duplicate-prevention logic to survive scene loads.
- **`Transitions.cs`** — Animator-driven fade transitions; triggers the `"ExitTrigger"` parameter before scene unload.

**Match Flow**:
1. First card click → `FirstSelected` set, card flips 180° on Z-axis
2. Second card click → `LastSelected` set, match checked against `ListOfTuples`
3. Match: both cards deactivated, `CardsRemaining -= 2`
4. No match: both cards rotate back; `Waiting` flag prevents input during animation

**Card Randomization**: At game start, `PlayerController` uses LINQ `OrderBy` with a `Random` seed to reposition cards, resetting Y to `MinYPosition` (-0.22) and randomizing X positions while preserving pair adjacency.

## Communication & Language

- The user writes in **Spanish** — respond and explain in Spanish when communicating.
- All generated code, variable names, comments, and file names must be in **English**.

## Key Conventions

- Scripts use `OnMouseDown()` (not UI raycasts) for 3D card click detection.
- Card identity for matching uses GameObject `name` (tag-based lookup via `ObjectNameTuple`).
- All in-code comments are in **Spanish**.
- `CamaraController.cs` and `BetterJump.cs` are dead code (fully commented out); safe to ignore.
- Several scripts (`MovilController.cs`, `RotatoriaController.cs`, `DisparadorController.cs`, `ColorMaterial.cs`) are platform/level utilities not used in the card game scenes.

## Unity Version & Packages

Unity **6000.3.13f1** (Unity 6).

Key packages (`Packages/manifest.json`):
- `com.unity.textmeshpro` 3.2.0-pre.10
- `com.unity.probuilder` 6.0.3
- `com.unity.timeline` 1.8.7
- C# target: .NET Framework 4.7.1, language version C# 8.0
- `UnityEngine.UI.Text` sigue en uso (legacy) — preferir TMP para nuevos elementos UI
