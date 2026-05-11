# Report

## 3D Coin Collection Game

**Student:** Karina  
**Project name:** CoinCollection  
**Engine:** Unity 6  
**Game type:** 3D coin collection game  

---

## 1. Introduction

This project is a 3D coin collection game created in Unity. The player controls an animated 3D character in a game environment, collects coins to increase the score, and avoids traps that decrease the score. The game includes a main menu, a playable game scene, a live scoreboard, Win and Lose states, result panels, animations, sound effects, lighting changes, and particle effects.

The main goal of the game is to collect all coins in the level. If the player collects all coins, the game triggers the Win state. If the player's score becomes lower than zero after touching traps, the game triggers the Lose state.

**Screenshot 1: Main Menu scene**  
Insert screenshot here.

**Screenshot 2: Main Game scene**  
Insert screenshot here.

---

## 2. Project Setup

The Unity project is named **CoinCollection** and uses a 3D scene setup. The project contains two main scenes:

- **MenuScene**: contains the title screen, Start Game button, Quit button, and menu UI.
- **GameScene**: contains the main playable level with the player, coins, traps, camera, UI, lighting, and result panels.

The game scene includes an animated 3D character, a terrain/world environment, collectible coins, obstacles/traps, and UI elements. The world uses imported assets, materials, lighting, and environmental objects, so it is not made only from default grey objects.

---

## 3. Player and Controls

The player is represented by an animated 3D character. The character is controlled using keyboard input:

- **W / Up Arrow**: move forward
- **S / Down Arrow**: move backward
- **A / Left Arrow**: move left
- **D / Right Arrow**: move right

The player movement is implemented in `PlayerScript.cs`. The script reads horizontal and vertical input, creates a world-space movement direction, normalizes diagonal movement, and moves the player using either a `CharacterController` or direct transform movement if no controller is available. The player also rotates to face the movement direction.

The player animation is controlled by Animator parameters:

- `isRunning` controls the transition between Idle and Run.
- `Win` triggers the victory pose.
- `Lose` triggers the defeat pose.

This makes the animation flow logical: the character idles when not moving, runs while moving, and switches to the result animation when the game ends.

**Screenshot 3: Player character and Animator Controller**  
Insert screenshot here.

---

## 4. Camera System

The game uses a stable third-person camera. The camera follows the player using the `CameraFollow.cs` script. The script stores an offset from the player and updates the camera position in `LateUpdate()`, which makes the camera follow smoothly after the player has moved.

The camera can also look at the player, creating a clear third-person view during gameplay.

**Script used:** `CameraFollow.cs`

**Screenshot 4: Camera setup in Inspector**  
Insert screenshot here.

---

## 5. Gameplay Mechanics

The game includes two main interaction types:

### Collectible Coins

Coins are collectible objects. When the player touches a coin:

1. The coin detects the player using trigger collision.
2. The player's score increases by 1.
3. The scoreboard updates immediately.
4. The coin is destroyed so it cannot be collected twice.
5. The game checks if all coins have been collected.

This logic is implemented in `CoinScript.cs` and `GameManager.cs`.

### Traps / Obstacles

Traps are obstacle objects. When the player touches a trap:

1. The trap detects the player using trigger collision.
2. The player's score decreases by 1.
3. The scoreboard updates immediately.
4. The game checks if the score is below zero.
5. If the score is less than zero, the Lose state starts.

This logic is implemented in `TrapScript.cs` and `GameManager.cs`.

**Screenshot 5: Coins and traps in the scene**  
Insert screenshot here.

---

## 6. Scoring System

The scoring system is managed by `GameManager.cs`. The player starts with a score of 0. Coins increase the score by 1, while traps decrease it by 1.

The UI scoreboard is handled by `UIManager.cs`. Instead of updating the score text every frame, the UI listens to a score change event from the GameManager. This is cleaner and more efficient because the text changes only when the score actually changes.

The score is displayed during gameplay in this format:

`Score: 0`

**Screenshot 6: Scoreboard during gameplay**  
Insert screenshot here.

---

## 7. Win and Lose Logic

The game has two result states:

### Win State

The Win state is triggered when all coins in the level are collected. When this happens:

- Player movement is disabled.
- The character plays the Win animation trigger.
- The Win Panel becomes visible.
- Happy music plays.
- Win particle effects are played.
- The lighting returns to normal brightness.

### Lose State

The Lose state is triggered when the score becomes lower than zero. When this happens:

- Player movement is disabled.
- The character plays the Lose animation trigger.
- The Lose Panel becomes visible.
- Sad music plays.
- The scene lighting becomes darker.

This logic is implemented in `GameManager.cs`.

**Screenshot 7: Win Panel**  
Insert screenshot here.

**Screenshot 8: Lose Panel**  
Insert screenshot here.

---

## 8. Animation and VFX

The character uses an Animator Controller with movement and result animation parameters. The basic movement animations are Idle and Run. The result animations are triggered by the GameManager:

- `PlayWinAnimation()` is called when the player wins.
- `PlayLoseAnimation()` is called when the player loses.

The project also includes visual effects:

- Win state: particle effects such as confetti/fireworks can be assigned to the `winParticles` field in the GameManager.
- Lose state: the scene light changes to a darker color and lower intensity.

These effects make the Win and Lose states clear to the player.

**Screenshot 9: Win particle effect setup**  
Insert screenshot here.

**Screenshot 10: Dark lighting after Lose state**  
Insert screenshot here.

---

## 9. UI System

The UI includes:

- Live score text.
- Win Panel.
- Lose Panel.
- Restart button.
- Menu button.

The Win and Lose panels are hidden when the game starts. They are activated only when the required condition is met. The Restart button reloads the current game scene, and the Menu button returns to the main menu scene.

The menu scene contains UI buttons handled by `MenuButtons.cs`:

- `StartGame()` loads the GameScene.
- `QuitGame()` exits the application.

The result panel buttons are handled by `UIButtons.cs`:

- `Restart()` reloads the active scene.
- `GoToMenu()` loads the MenuScene.

---

## 10. Scripts Used in the Project

### `PlayerScript.cs`

Handles player movement, score storage, and animation triggers. It uses world-space movement and avoids unstable circular movement.

### `CoinScript.cs`

Detects when the player touches a coin and calls the GameManager to increase the score.

### `TrapScript.cs`

Detects when the player touches a trap and calls the GameManager to decrease the score.

### `GameManager.cs`

Controls the main game loop, score changes, Win/Lose conditions, UI panels, music, lighting effects, and Win particles.

### `UIManager.cs`

Updates the live scoreboard when the score changes.

### `UIButtons.cs`

Controls Restart and Menu buttons in the result panels.

### `MenuButtons.cs`

Controls the Start Game and Quit buttons in the main menu.

### `CameraFollow.cs`

Makes the camera follow the player with a stable third-person view.

---

## 11. Testing

The game was tested for the main required gameplay cases:

| Test Case | Expected Result | Status |
|---|---|---|
| Press Start Game in MenuScene | GameScene loads | Passed |
| Move player with WASD/arrows | Character moves in world space | Passed |
| Player touches coin | Score increases by 1 | Passed |
| Player touches trap | Score decreases by 1 | Passed |
| Score becomes less than 0 | Lose Panel appears | Passed |
| All coins are collected | Win Panel appears | Passed |
| Restart button is pressed | GameScene reloads | Passed |
| Menu button is pressed | MenuScene loads | Passed |
| Win state starts | Win animation, music, and particles play | Passed |
| Lose state starts | Lose animation, music, and dark lighting activate | Passed |

---

## 12. Conclusion

The CoinCollection project meets the main requirements of the assignment. It includes a 3D playable scene, an animated player character, keyboard movement, a third-person camera, collectible coins, traps, a live scoreboard, a main menu, Win and Lose states, result panels, animations, sound, lighting changes, and particle effects.

The code is organized into simple scripts with clear responsibilities. The GameManager controls the overall game state, while separate scripts handle player movement, coin collection, trap collision, UI, buttons, and camera following. This makes the project easier to understand, maintain, and demonstrate.

