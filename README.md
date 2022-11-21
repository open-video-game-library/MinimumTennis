# Minimum Tennis

[日本語版README](https://github.com/open-video-game-library/Minimum-Tennis/blob/main/README.JP.md)

Minimum Tennis is a simple tennis game in accordance with real tennis.

While moving back and forth, left and right, hit the ball back at the right time.

To avoid noise in the experiment, we adopted a primitive design in the appearance of the characters and eliminated the factors that contribute to preconceptions about the characters: race, body size, and gender.

![MinimumTennis](https://user-images.githubusercontent.com/77042312/194984821-09b2d9e3-a723-4c8e-b5c7-d2905f4f3b8e.png)

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

## Usage

All the data necessary for Minimum Tennis to work is included.

If you want to change the 3D model, Skybox, or other assets, import and replace them each time.

Some of the assets included in the project also include materials provided by outside parties that are permitted for redistribution. If you wish to publish game data containing them, please check the included license file.

## Licence

1. Minimum Tennis are available free of charge.

2. You may use the library for any purpose, including research purposes, as long as it is not for commercial purposes or against public order and morals.

3. You may modify and use the downloaded data.

4. You are not required to report the use of the data, but please indicate if you have used Minimum Tennis, including secondary distribution of modified data.

5. If you wish to use Minimum Tennis in your research, please cite the following article within your own paper.

- [研究者が利用しやすいオープンなスポーツゲームの試作](http://www.interaction-ipsj.org/proceedings/2022/data/pdf/4D18.pdf)   
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

## Note

- Please refrain from publishing any material downloaded from Minimum Tennis under the false pretense that it was developed by you.

- We are not responsible for any trouble/damage caused by the use of Minimum Tennis.

- The content and terms of use of Minimum Tennis are subject to change without notice.

- Minimum Tennis allows you to change the parameters in the game, but please be sure to clearly state the values of the parameters you set in order to keep the study fair and reproducible.

## Contact

If you have any comments, requests or questions, please contact us [here](https://open-video-game-library.github.io/info/contact/).
