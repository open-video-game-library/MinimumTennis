# Minimum Tennis

[English version README](https://github.com/open-video-game-library/MinimumTennis/blob/main/README.md)

Minimum Tennisは、現実のテニスに則ったシンプルなテニスゲームです。

前後左右に移動しながら、タイミングよくボールを打ち返しましょう。

実験のノイズとならないように、キャラクターの見た目ではプリミティブなデザインを採用し、キャラクターへの先入観の要因となる、人種・体格・性別を排除しました。

![MinimumTennis_4](https://user-images.githubusercontent.com/77042312/221799772-c4a4f92b-da00-4079-bc77-acb19b7df975.png)

## Contents

### ルール

基本的なルールは、現実のテニスに則っています。

- 失点となる行為
	- 打ったボールが相手コートに入らない
	- 打ったボールがネットにかかる
	- 相手の打ったボールが自分のコート内で2回バウンドする前に打ち返さない
	- 打ったサーブが2回連続でフォールトとなる 

- デュース
	- 1つのゲームにおいて、両者が3回ずつ得点した場合はデュースとなる
	- デュースとなった場合、その状態からどちらかが2回連続で得点すると、そのゲームを獲得できる
	- 2回連続で得点できなかった場合は、再びデュースとなる

- 勝敗
	- パラメータ調整機能によって指定されたゲーム数を先に獲得すると勝利となる
	- ◯セットマッチではなく、◯ゲーム先取である点に注意

### 操作方法

- キーボード操作  
![MinimumTennis_操作方法_キーボード](https://github.com/open-video-game-library/MinimumTennis/assets/63552585/a2c45c0d-dd13-4382-9771-a6dff22e6a0e)


- ゲームパッド操作  
![MinimumTennis_操作方法_ゲームパッド](https://github.com/open-video-game-library/MinimumTennis/assets/63552585/2279f2b9-5d63-4dfa-907f-f76c67071a8b)


- Joy-Con操作（モーション操作）  
	- Joy-Con操作で球種を打ち分ける場合は、各球種に該当するボタンを押しながら、Joy-Conを振って下さい。<br>
![MinimumTennis_操作方法_ジョイコン](https://github.com/open-video-game-library/MinimumTennis/assets/63552585/89536f2d-8821-46b0-b4ec-23fe4846488a)

### TIPS

- 全操作方法共通で、サービスラインより前にて、ノーバウンドのボールを打ち返すと、ボレーを行えます。
- また、全操作方法共通で、サービスラインより前にて、ノーバウンドのボールを一定以上の高さで打つと、スマッシュとなります。
- サーブを打つ瞬間、左右の移動入力により、サーブを打つ方向を変えることができます。

## Features

- 通常プレイ

	- キーボードやゲームパッドを使って、コンピューターと試合形式で対戦するモードです。
 	- ホーム画面で「Normal Mode」ボタンをクリックすることにより選択できます。
	- キャラクター選択後のゲーム設定画面にて、コントローラーの接続状況を確認できます。

- 2人プレイ

	- パソコンにゲームパッドを2つ接続することで、2人で対戦プレイをすることができます。
 	- ホーム画面で「Play with two players」ボタンをクリックすることにより選択できます。
	- キャラクター選択後のゲーム設定画面にて、ゲームパッドの接続状況を確認できます。

- パラメータ設定

	- 対戦相手とラリーの練習をしながら、プレイヤのパラメータを編集できます。
 	- ホーム画面で「Paramter setting」ボタンをクリックすることにより選択できます。
 	- プレイヤー1とプレイヤー2に対して、以下のパラメータが編集できます。

		- プレイヤと対戦相手の移動速度
		- プレイヤと対戦相手の打つボールの速度
		- プレイヤの反応速度（モーション操作時）
		- 対戦相手の反応速度
		- 対戦相手のショットに辿り着くまでの移動距離

- トレーニングタスク

	- 移動・返球・ラリーのそれぞれに特化した3種類のタスクをプレイできます。
 	- ホーム画面で「Training task」ボタンをクリックすることにより選択できます。
 	- 移動に特化したMovingタスクは、サーブされるボールに追いつき、相手のコートへ打ち返すタスクです。
  	- 返球に特化したHittingタスクは、サーブされるボールを、相手のコートの赤いエリアを狙って打ち返すタスクです。
  	- ラリーに特化したRallyingタスクは、制限時間内にできるだけたくさんラリーをつなげるタスクです。
  	- Movingタスク・Hittingタスクでは、ノーバウンドでボールを打ち返すことができなくなります。

- 複数のコントローラによる操作

	- 本ゲームは、キーボード操作/ゲームパッド操作/Joy-Con操作（モーション操作） の3つに対応しています。
 	- 各モードのゲーム設定画面にて、接続されているコントローラーから使用するコントローラーを選ぶことができます。
 	- また、返球操作と移動操作それぞれを、オート操作にすることも可能です。（片方の操作のみオート化する場合、スクリプトの編集が必要です。）
  	  ![image](https://github.com/open-video-game-library/MinimumTennis/assets/77042312/9f9aca1f-cd7e-4be0-9438-7bd678a173dc)

 
- パラメータ出力機能

	- 以下のパラメータをゲーム終了時にCSVファイルとして出力できます。
 	- ファイルの出力は、"Normal Mode"および"Play with two players"モードの試合後に行うことができます。
	
		- 勝者
		- プレイヤが勝ち取ったゲーム数
		- 対戦相手が勝ち取ったゲーム数
		- プレイヤのネットした回数
		- 対戦相手のネットした回数
		- プレイヤのアウトした回数
		- 対戦相手のアウトした回数
		- プレイヤの２バウンドした回数
		- 対戦相手の２バウンドした回数
		- プレイヤのダブルフォルトした回数
		- プレイヤのダブルフォルトした回数
		- 最大ラリー回数

### 研究利用例

1. ゲームコントローラでの操作と、モーションコントローラでの操作におけるユーザエクスペリエンスの比較と評価

	- 本ゲームを、ゲームパッド操作とJoy-Con操作でそれぞれプレイしてもらい、観察・アンケート・CSV出力されたデータをもとに比較と評価を行う

2. 運動を伴うビデオゲームがユーザに与える影響の調査

	- 本ゲームを、Joy-Con操作でプレイしてもらい、観察・アンケート・CSV出力されたデータをもとに比較と評価を行う

## Requirement

OS： Windows, Mac

Unity：2021.3.5f1 or later

## Installation

Unityは[こちら](https://unity3d.com/jp/get-unity/download/archive)からインストールできます。

本リポジトリのデータは下記のコマンドを入力することでローカル環境にクローンできます。
```
git clone https://github.com/open-video-game-library/MinimumTennis.git
```

ローカル環境にクローンしたUnityのプロジェクトファイルを、上記の"Requirement"の環境で開きます。

本リポジトリでは、Joy-Conを動かすための外部プラグイン![JoyconLib](https://github.com/Looking-Glass/JoyconLib)を利用させていただいています。

それに伴い、JoyconLibにて提供されている"Joycon"スクリプトの一部を修正しています。

（"Joycon"スクリプトの370行目のコードを、以下のように修正しています。）

変更前
```
DebugPrint(string.Format("Dequeue. Queue length: {0:d}. Packet ID: {1:X2}. Timestamp: {2:X2}. Lag to dequeue: {3:s}. Lag between packets (expect 15ms): {4:s}",
```

変更後
```
DebugPrint(string.Format("Dequeue. Queue length: {0:d}. Packet ID: {1:X2}. Timestamp: {2:X2}. Lag to dequeue: {3:t}. Lag between packets (expect 15ms): {4:g}",
```

## Usage

3DモデルやSkyboxなどのアセットを変更したい場合は、その都度インポート・置き換えをして下さい。

また、プロジェクトに含まれているアセットの中には、外部が提供している再配布が認められた素材も含まれています。それらを含んだゲームデータを公開する場合は、各アセット毎に同梱されたライセンスファイルをご確認下さい。

## Licence

本コンテンツは、[MITライセンス](https://github.com/open-video-game-library/MinimumTennis/blob/main/LICENSE.md)のもとで利用が許可されています。

## Use in Research

[本コンテンツを研究利用する場合の推奨事項](https://github.com/open-video-game-library/MinimumTennis/blob/main/RESEARCH_USE.JP.md)

## Contact

意見や要望、質問などがありましたら、[こちら](https://openvideogame.cc/contact)からお問い合わせ下さい。

