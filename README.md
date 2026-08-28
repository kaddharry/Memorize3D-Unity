# Memorize

A 3D memory card-matching game built with Unity 6. Flip cards, find matching pairs, and complete the board with the best possible time and number of attempts.

## Gameplay

- Flip cards to reveal their faces.
- Find and match all pairs.
- Unmatched cards shake and flip back face-down.
- Track remaining pairs, elapsed time, and failed attempts.
- Personal best time and attempts are saved between sessions.
- Complete all pairs to trigger the win screen.
- Restart the game directly from the win screen.
- Return to the main menu using the Home button.

## Tech Stack

- **Engine:** Unity 6 (6000.3.13f1)
- **Language:** C#
- **UI:** Unity UI + TextMeshPro
- **Target Platforms:** Android (portrait), Windows, macOS
- **Persistence:** Unity PlayerPrefs

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
    ├── DisparCardController.cs
    ├── FrameRateLimiter.cs
    ├── GameManager.cs
    ├── LanguageSelector.cs
    ├── LocalizationManager.cs
    ├── LocalizedText.cs
    ├── MovilController.cs
    ├── Music.cs
    ├── ObjectNameTuple.cs
    ├── PlayerController.cs
    └── Transitions.cs
```

## Scene Flow

```text
Preload → Start (Main Menu) → Game
                          ↘ Options
                          ↘ Credits

Game → Home → Start
Game → Restart → Game
```

## Scenes

### Preload

Initializes the required managers and persistent systems before entering the main menu.

### Start

The main menu and entry point to the game.

Provides access to:

- Start / Play
- Options
- Credits

### Game

The main card-matching gameplay scene.

Includes:

- Card board
- Remaining pairs counter
- Timer
- Failed attempts counter
- Best score information
- Win screen
- Home button
- Restart button

### Options

Provides the game's options interface, including language selection and other available settings.

### Credits

Displays the game's credits.

## How to Run

1. Open the project in **Unity 6** using Unity Hub.
2. Allow Unity to import and process the project assets.
3. Open the `Start` scene from `Assets/Scenes`.
4. Click the **Play** button in the Unity Editor.
5. Select **Play / Start** from the main menu to enter the game.

## How It Works

1. The `Preload` scene initializes the required managers and persistent systems.
2. The `Start` scene loads as the main menu.
3. Starting the game loads the `Game` scene.
4. Cards are placed on the game board and can be selected by the player.
5. Selecting a card reveals its face.
6. The player selects a second card to attempt a match.
7. Matching cards are removed from the active game.
8. If the cards do not match, they shake and flip back face-down.
9. The HUD tracks remaining pairs, elapsed time, and failed attempts.
10. Completing all pairs triggers the win screen.
11. The win screen displays the final game result and allows the player to restart or return home.
12. Best performance information is persisted using `PlayerPrefs`.

## Implemented Features

### Gameplay

- Card matching gameplay with 10 pairs.
- Card flip animations.
- Randomized card arrangement.
- Shake animation for incorrect matches.
- Remaining pairs counter.
- Elapsed game timer.
- Failed attempts counter.
- Win screen after completing all pairs.
- Restart functionality from the win screen.
- Home button to return to the main menu.

### Scoring and Persistence

- Personal best time tracking.
- Personal best attempts tracking.
- Best results saved between sessions using `PlayerPrefs`.
- Best score displayed in the game HUD and win screen.

### User Experience

- Main menu with navigation to gameplay, options, and credits.
- Home and restart navigation from gameplay.
- Options screen with language selection.
- English and Spanish localization.
- Persistent language selection using `PlayerPrefs`.
- Fade transitions between scenes.

### Technical

- Persistent managers initialized through the `Preload` scene.
- Scene-based game flow.
- Modular C# scripts for gameplay, UI, localization, audio, and transitions.
- Randomized card setup.
- Persistent player preferences for saved settings and scores.

## Controls

### Desktop

- Use the mouse to interact with cards and UI buttons.

### Mobile

- Tap cards and UI buttons using the touchscreen.

## Localization

The game includes support for:

- English
- Spanish

The selected language is stored using `PlayerPrefs` and applied through the custom localization system.

## Project Notes

This project was originally developed as a Unity-based memory card game and was further adapted and improved with additional gameplay, navigation, UI, localization, and persistence features.

## Future Improvements

- Additional difficulty levels.
- Countdown mode.
- Match visual effects.
- Separate audio cues for matches and mismatches.
- More language support.
- Improved mobile input handling.
- Additional gameplay and accessibility settings.

## License

This project is available under the MIT License.
