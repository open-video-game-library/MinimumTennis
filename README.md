# Minimum Tennis

[日本語版README](https://github.com/open-video-game-library/Minimum-Tennis/blob/main/README_JP.md)

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

![MinimumTennis_操作方法_キーボード_英](https://user-images.githubusercontent.com/77042312/203004255-956e3fef-0bc3-4d09-a3d7-a66b2c334ac6.png)

- Gamepad Operation  

![MinimumTennis_操作方法_ゲームパッド_英](https://user-images.githubusercontent.com/77042312/202995633-a79ff877-ac26-46b0-ba55-473e36c0155b.png)

- Joy-Con Operation (Motion Control)    
	- To hit different types of shot with the Joy-Con controls, hold down the button corresponding to each type of pitch and shake the Joy-Con.
	
![MinimumTennis_操作方法_ジョイコン_英](https://user-images.githubusercontent.com/77042312/202995765-ef5b195e-d090-4518-a386-82dc9346b3ee.png)

## Features

- Parameter Adjustment Function

	- The following parameters can be adjusted on the game.
	
		- Speed of player and opponent movement
		- Speed of balls hit by players and opponents
		- Time of player's reaction (during motion operation)
		- Time of opponent's reaction
		- Distance traveled to reach opponent's shot
		- Playstyle
		- Match ending condition

- Parameter Output Functions

	- The following parameters can be output as a CSV file at the end of the game.
	
		- Winner
		- Number of games won by the player
		- Number of games won by the opponents
		- Number of times the player netted
		- Number of times the opponent netted
		- Number of times the player's ball went out
		- Number of times the opponent's ball went out
		- Number of times the player's 2 bounces
		- Number of times the opponent's 2 bounces
		- Number of times the player's double-fault.
		- Number of times the player's double-fault.
		- Maximum number of rallies

- Competitive Play

	- By connecting two gamepads to a computer, two players can play against each other.
	- As shown in the image below, the number of controllers currently registered is displayed on the title screen. (Two are connected in the image below)
	
	![MinimumTennis_登録コントローラ数](https://user-images.githubusercontent.com/77042312/187391138-cc945035-79b5-4f0b-b90d-22efeb7b9c2e.png)
	
	- Clicking on the "Competition Button" on the home screen will take you to the competition play screen.


- Operation with Multiple Controllers

	- The game supports three types of operation: keyboard operation, gamepad operation, and Joy-Con operation (motion operation).
	- To play with keyboard control/gamepad control, click "Normal Control" on the home screen.
	- To play with Joy-Con control (motion control), click "Motion Control" on the home screen.

### Research Applications

1. Comparison and evaluation of user experience in game controller operation and motion controller operation

	- Play this game with Gamepad and Joy-Con controls to compare and evaluate the user experience of each.

2. Investigating the impact of video games with exercise on users

	- Play this game with Joy-Con controls to compare and evaluate the user experience of each.

## Requirement

OS：Windows, Mac

Unity：2021.1.17f1

## Installation

Unity can be installed [here](https://unity3d.com/get-unity/download/archive).

Data in this repository can be cloned to the local environment by entering the following command.
```
git clone https://github.com/open-video-game-library/Minimum-Tennis.git
```

Open the cloned Unity project file in your local environment in the "Requirement" environment described above.

When it opens, you will see a warning message like the image below, click on "Ignore".

![warning window](https://user-images.githubusercontent.com/77042312/214867252-7a2321ce-a256-45fe-98bf-fc666866d57f.png)

When loading is finished and the Unity editor opens, there are 6 errors as shown in the image below.

![error](https://user-images.githubusercontent.com/77042312/214869138-33a29413-7143-4a01-b13c-460ff922870e.png)

The warnings displayed before opening the Unity editor are due to these errors.

The error can be summarized simply as "Asset not found to use Joy-Con".

Since the assets for using Joy-Con with Unity are third-party, the following explains how to install these assets.

First, download the assets for using Joy-Con in Unity from [this page](https://github.com/Looking-Glass/JoyconLib/releases).

Click on the "JoyconLib06.unitypackage" highlighted in red in the image below to download the file.

![JoyconLib_GitHub](https://user-images.githubusercontent.com/77042312/214875311-4a56ce12-4cbd-4cff-9676-d66843bee620.png)

Drag and drop the downloaded "JoyconLib06.unitypackage" into the Unity Editor's Project window as shown in the image below.

![JoyconLib_UnityEditor](https://user-images.githubusercontent.com/77042312/214879097-f879126f-4785-41b9-8420-5a102b6c3094.png)

Then, a window like the one in the image below will be displayed, and click on "Import".

![JoyconLib_Import](https://user-images.githubusercontent.com/77042312/214879647-253bfb25-87c4-451e-a431-632cf38e819f.png)

When the assets are finished loading, the assets are imported into the Project file and the errors disappear, but if you run the game, you will get a new error.

Since this error is caused by the imported assets, rewrite the scripts in the assets.

Open the "Joycon" script in the "JoyconLib_scripts" folder in the Project file as shown in the image below.

![JoyconLib_EditScript](https://user-images.githubusercontent.com/77042312/215122961-42ff274b-6e4c-40a0-9754-0051295fbf47.png)

Replace the code on line 370 of the opened script with the following code from the code before the change.

before
```
DebugPrint(string.Format("Dequeue. Queue length: {0:d}. Packet ID: {1:X2}. Timestamp: {2:X2}. Lag to dequeue: {3:s}. Lag between packets (expect 15ms): {4:s}",
```

after
```
DebugPrint(string.Format("Dequeue. Queue length: {0:d}. Packet ID: {1:X2}. Timestamp: {2:X2}. Lag to dequeue: {3:t}. Lag between packets (expect 15ms): {4:g}",
```

After rewriting the code, save it.

Start the game in this state, and if no error occurs, the installation was successful.

## Usage

All the data necessary for Minimum Tennis to work is included.

If you want to change the 3D model, Skybox, or other assets, import and replace them each time.

Some of the assets included in the project also include materials provided by outside parties that are permitted for redistribution. If you wish to publish game data containing such assets, please check the license file included with each asset.

## Licence

This content is licensed under the [MIT License](https://github.com/open-video-game-library/Minimum-Tennis/blob/main/LICENSE.md). 

## Citation

研究者が利用しやすいオープンなスポーツゲームの試作  
Prototype of open sports game accessible to researchers

[Click here for the paper page.](http://www.interaction-ipsj.org/proceedings/2022/data/pdf/4D18.pdf)

### BiBTeX

```
@conference{飯田:2022, 
   author	 = "和也,飯田 and 拓也,岡 and 拓也,川島 and 洋平,簗瀬 and 恵太,渡邊",
   title	 = "研究者が利用しやすいオープンなスポーツゲームの試作"
   booktitle	 = "インタラクション2022予稿集",
   institution	= "明治大学総合数理学部先端メディアサイエンス学科, 明治大学大学院先端数理科学研究科先端メディアサイエンス専攻, 明治大学大学院先端数理科学研究科先端メディアサイエンス専攻, ユニティ・テクノロジーズ・ジャパン株式会社, 明治大学総合数理学部先端メディアサイエンス学科",
   year		 = "2022",
   month	 = "feb"
}
```

## Contact

If you have any comments, requests or questions, please contact us [here](https://openvideogame.cc/contact).
