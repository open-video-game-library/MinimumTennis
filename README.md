# Minimum Tennis

[日本語版README](https://github.com/open-video-game-library/MinimumTennis/blob/main/README_JP.md)

Minimum Tennis is a simple tennis game in accordance with real tennis.

While moving back and forth, left and right, hit the ball back at the right time.

To avoid noise in the experiment, we adopted a primitive design in the appearance of the characters and eliminated the factors that contribute to preconceptions about the characters: race, body size, and gender.

![MinimumTennis_4](https://user-images.githubusercontent.com/77042312/221799399-7e0d1931-a4db-45f6-a162-482de88c9a6a.png)

## Contents

### Rule

The rules are in accordance with real tennis.

- Conduct that results in a loss of points
	- The ball you hit does not go into the opponent's court.
	- A batted ball hits the net.
	- Not hitting back before the ball hit by the opponent bounces twice in your court.
	- A serve you hit results in two consecutive faults.

- Deuce
	- If both players score three times each in a game, deuce is awarded.
	- If it is a deuce, the game is won if either side scores twice in a row from that situation.
	- If a player fails to score twice in a row, deuce is again awarded.

- Win or Loss
	- The first to win the number of games specified by the parameter adjustment function wins the game.
	- Note that the first to win a certain number of games wins.

### How to operate

- Keyboard Operation  

![MinimumTennis_操作方法_キーボード_英](https://github.com/open-video-game-library/MinimumTennis/assets/63552585/551b4f09-6e80-46b5-8ca5-4dc2d0397545)


- Gamepad Operation  

![MinimumTennis_操作方法_ゲームパッド_英](https://github.com/open-video-game-library/MinimumTennis/assets/63552585/f701ff26-7514-4ced-b29c-2f7e70f6a565)

- Joy-Con Operation (Motion Control)    
	- To hit different types of shot with the Joy-Con controls, hold down the button corresponding to each type of pitch and shake the Joy-Con.
	
![MinimumTennis_操作方法_ジョイコン_英](https://github.com/open-video-game-library/MinimumTennis/assets/63552585/09ab75d2-a5df-45e6-b428-58971dd7d8cd)

### TIPS

- In common with all operation methods, a volley can be made by hitting a ball back in front of the service line with no bounce.
- Also, in common with all operation methods, a smash can be made by hitting a ball with no bounce before the service line at a certain height.
- At the moment of serve, the direction of serve can be changed by inputting the left or right movement.

## Features

- Normal Play

	- In this mode, you play against the CPU in a match format using the keyboard or gamepad.
 	- This mode can be selected by clicking the "Normal Mode" button on the home screen.
	- You can check the controller connection status on the game settings screen after selecting a character.

- Two Players Play

	- Two players can play against each other by connecting two gamepads to the computer.
 	- Click the "Play with two players" button on the home screen to select this option.
	- You can check the connection status of the game pad on the game settings screen after selecting a character.

- Parameter Settings

	- You can edit your player's parameters while practicing rallies with your opponent.
 	- You can select this option by clicking on the "Parameter setting" button on the home screen.
 	- The following parameters can be edited for Player 1 and Player 2

		- Movement speed
		- Acceleration
		- Ball speed
		- Reaction Speed
		- Distance to reach the shot

- Training Tasks

	- You can play three types of tasks specialized for each of movement, ball return, and rally.
 	- You can select one by clicking the "Training task" button on the home screen.
 	- The Moving task, which specializes in moving, is a task to catch up with a served ball and hit it back to the opponent's court.
  	- The Hitting task, which specializes in returning the ball, is to hit the served ball back to the red area of the opponent's court.
  	- Rallying tasks, which focus on rallying, are tasks to connect as many rallies as possible within a time limit.
  	- In the Moving and Hitting tasks, the player cannot hit the ball back with no bounce.

- Multiple Controller Support

	- The game supports keyboard control, gamepad control, and Joy-Con (motion control).
 	- In the game setting screen of each mode, you can select the controller to be used from the connected controllers.
 	- It is also possible to set the ball return operation and the movement operation to auto operation. (If you want to set only one of the operations to auto, you will need to edit the script.)
  	  ![image](https://github.com/open-video-game-library/MinimumTennis/assets/77042312/9f9aca1f-cd7e-4be0-9438-7bd678a173dc)

 
- Parameter Export Function

	- The following parameters can be output as a CSV file at the end of the game.
 	- The file can be output after a game in "Normal Mode" or "Play with two players" mode.
	
		- Winners
		- Number of games won by the player
		- Number of games won by the opponent
		- Number of games netted by the player
		- Number of games netted by opponent
		- Number of times a player has gone out
		- Number of times opponent went out
		- Number of times a player has 2 bounces
		- Number of times opponent has 2 bounces
		- Number of times a player has double faulted
		- Number of times a player has double faulted
		- Max Rallies

### Research Applications

1. Comparison and evaluation of user experience in game controller operation and motion controller operation

	- Play this game with Gamepad and Joy-Con controls to compare and evaluate the user experience of each.

2. Investigating the impact of video games with exercise on users

	- Play this game with Joy-Con controls to compare and evaluate the user experience of each.

## Requirement

OS： Windows, Mac

Unity： 2021.3.5f1 or later

## Installation

Unity can be installed [here](https://unity3d.com/get-unity/download/archive).

Data in this repository can be cloned to the local environment by entering the following command.
```
git clone https://github.com/open-video-game-library/MinimumTennis.git
```

Open the cloned Unity project file in your local environment in the "Requirement" environment above.

In this repository, we use an external plugin for Joy-Con![JoyconLib](https://github.com/Looking-Glass/JoyconLib) to run Joy-Con.

Accordingly, some of the "Joycon" scripts provided by JoyconLib have been modified.

(The code in line 370 of the "Joycon" script is modified as follows)

before
```
DebugPrint(string.Format("Dequeue. Queue length: {0:d}. Packet ID: {1:X2}. Timestamp: {2:X2}. Lag to dequeue: {3:s}. Lag between packets (expect 15ms): {4:s}",
```

after
```
DebugPrint(string.Format("Dequeue. Queue length: {0:d}. Packet ID: {1:X2}. Timestamp: {2:X2}. Lag to dequeue: {3:t}. Lag between packets (expect 15ms): {4:g}",
```

## Usage

If you want to change the 3D model, Skybox, or other assets, import and replace them each time.

Some of the assets included in the project also include materials provided by outside parties that are permitted for redistribution. If you wish to publish game data containing such assets, please check the license file included with each asset.

## Licence

This content is licensed under the [MIT License](https://github.com/open-video-game-library/MinimumTennis/blob/main/LICENSE.md). 

## Use in Research

[Recommendations for research use of this content](https://github.com/open-video-game-library/MinimumTennis/blob/main/RESEARCH_USE.md)

## Contact

If you have any comments, requests or questions, please contact us [here](https://openvideogame.cc/contact).
