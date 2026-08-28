````markdown
# Memorize

A 3D memory card-matching game built with Unity 6. The player flips cards to find matching pairs while tracking time, failed attempts, and personal best performance.

## Gameplay

- Flip cards to reveal their faces.
- Find all matching pairs.
- Unmatched cards shake and flip back face-down.
- Track remaining pairs, elapsed time, and failed attempts.
- Personal best performance is saved between sessions.
- Complete all pairs to trigger the win screen.
- Restart the game directly from the win screen.
- Return to the main menu using the Home button.

## Tech Stack

- **Engine:** Unity 6 (6000.3.13f1)
- **Language:** C#
- **UI:** Unity UI + TextMeshPro
- **Target Platforms:** Android (portrait), Windows/macOS
- **Persistence:** Unity `PlayerPrefs`

## Project Structure

```text
Assets/
├── Animations/
├── Editor/
├── Fonts/
├── Materials/
├── Models/
├── Music/
│   ├── BGM/
│   └── SFX/
├── Plugins/
├── Prefabs/
├── ProBuilder Data/
├── Resources/
├── Scenes/
│   ├── Preload
│   ├── Start
│   ├── Game
│   ├── Options
│   └── Credits
└── Script/
    ├── BetterJump.cs
    ├── ButtonsManager.cs
    ├── CameraController.cs
    ├── CardController.cs
    ├── ColorMaterial.cs
    ├── DisparadorController.cs
    ├── FrameRaterLimiter.cs
    ├── GameManager.cs
    ├── LanguageSelector.cs
    ├── LocalizationManager.cs
    ├── LocalizedText.cs
    ├── MovilController.cs
    ├── Music.cs
    ├── ObjectNameTuple.cs
    ├── PlayerController.cs
    └── Transitions.cs
````

## Scene Flow

```text
Preload → Start (Main Menu) → Game
                         ↘ Options
                         ↘ Credits

Game → Home → Start
Game → Restart → Game
```

## Scenes

* **Preload** — Initializes the required managers and persistent systems.
* **Start** — Main menu and entry point to the game.
* **Game** — Main memory card gameplay.
* **Options** — Language selector and settings interface.
* **Credits** — Credits screen.

## How to Run

1. Open the project in **Unity 6** using Unity Hub.
2. Allow Unity to import and process the project assets.
3. Open the `Start` scene from `Assets/Scenes`.
4. Click the **Play** button in the Unity Editor.
5. Select **Play** from the main menu to enter the game.
6. Use the **Home** button during gameplay to return to the main menu.
7. After winning, use **Restart** to start another round.

## How It Works

1. The `Preload` scene initializes the required managers.
2. The `Start` scene provides the main menu and navigation.
3. The `Game` scene handles the card-matching gameplay.
4. Cards are revealed when the player selects them.
5. A second card is selected and compared with the first.
6. Matching cards are removed from play.
7. Non-matching cards shake and flip back face-down.
8. The HUD tracks remaining pairs, failed attempts, and elapsed time.
9. The player's best performance is stored using `PlayerPrefs`.
10. When all pairs are matched, the win screen displays the final result.
11. The player can restart the game or return to the home screen.

## Main Scripts

### `GameManager.cs`

Handles the main game-management functionality and persistent game systems.

### `PlayerController.cs`

Handles core gameplay state, card matching, remaining cards, win conditions, timer and attempt tracking, and best-score logic.

### `CardController.cs`

Controls individual cards, including selection, flip animation, matching behavior, and mismatch feedback.

### `ButtonsManager.cs`

Handles UI button navigation between scenes and menu states, including Start, Game, Options, Credits, Exit, Home, and Restart flows.

### `LocalizationManager.cs`

Provides the custom localization system and persists the selected language using `PlayerPrefs`.

### `LocalizedText.cs`

Updates UI text when the selected language changes.

### `LanguageSelector.cs`

Provides the runtime language-selection interface in the Options scene.

### `Music.cs`

Manages background music and persistent audio behavior.

### `Transitions.cs`

Controls animator-driven scene and UI fade transitions.

## Implemented Improvements

### Gameplay

* Visible timer while the game is active.
* Timer stops when the player wins.
* Failed attempts counter displayed in the HUD.
* Personal best time and attempts saved using `PlayerPrefs`.
* Best score displayed during gameplay and on the win screen.
* Cards drop onto the board with an ease-out animation at the beginning of the game.
* Cards shake when the selected pair does not match.
* Restart functionality available from the win screen.

### User Experience

* Added a **Home** button to return from the Game scene to the main menu.
* Added a **Restart** button to start another round after winning.
* Added English/Spanish localization support.
* Added an Options screen with language and settings interface.
* Added Credits screen.
* Added navigation between the main menu, game, options, and credits.

### Bug Fixes

* Fixed card matching so that matching is based on card/material identity instead of relying on array index order.
* Added working Home navigation from the Game scene back to the Start scene.

## Localization

The project includes a custom localization system supporting:

* English
* Spanish

Localized content is stored under:

```text
Assets/Resources/Localization/
├── en.txt
└── es.txt
```

The selected language is persisted using `PlayerPrefs` and restored between sessions.

## Current Game UI

The gameplay HUD includes:

* **Remaining** — Number of cards/pairs remaining.
* **Attempts** — Number of failed matching attempts.
* **Timer** — Current elapsed game time.
* **Best** — Personal best performance.
* **Home** — Returns to the main menu.
* **Restart** — Restarts the game after winning.

## Architecture Notes

The project uses Unity scenes and MonoBehaviour-based managers and controllers.

Persistent systems use Unity's `DontDestroyOnLoad` pattern where appropriate. UI buttons are connected through Unity Button `OnClick` events, with `ButtonsManager` handling navigation between the relevant scenes.

The project also contains legacy Unity UI and animation components inherited from the original project.

## Planned Improvements

### Gameplay

* Difficulty levels with different card counts and animation timings.
* Countdown mode with a limited time to complete the board.
* Penalties for incorrect matches.
* Additional card sets and textures.

### User Experience

* Visual effects when a matching pair is found.
* Highlight the first selected card while waiting for the second card.
* Animated win-screen entrance using fade and scale effects.
* More complete settings controls for SFX volume, BGM volume, and difficulty.

### Technical / Quality

* Separate audio cues for match and mismatch events.
* Continue migrating legacy `UnityEngine.UI.Text` components to TextMeshPro.
* Replace `OnMouseDown()`-based card interaction with `IPointerClickHandler` and appropriate raycasting for more reliable mobile touch input.
* Further cleanup and modernization of legacy Unity components.

## Credits

This project was originally developed as a Unity memory-card game and subsequently extended with additional gameplay, UI, navigation, localization, and usability improvements.

