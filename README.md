# Minimum Tennis

[English version README](https://github.com/open-video-game-library/Minimum-Tennis/blob/main/README.EN.md)

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
![MinimumTennis_操作方法_キーボード](https://user-images.githubusercontent.com/77042312/203004649-45b6048a-7520-40ba-bafa-5c4c51674716.png)

- ゲームパッド操作  
![MinimumTennis_操作方法_ゲームパッド](https://user-images.githubusercontent.com/77042312/187409768-07f14ef2-a8f3-418d-82cd-848223f3fe47.png)

- Joy-Con操作（モーション操作）  
	- Joy-Con操作で球種を打ち分ける場合は、各球種に該当するボタンを押しながら、Joy-Conを振って下さい。  
![MinimumTennis_操作方法_ジョイコン](https://user-images.githubusercontent.com/77042312/187441191-bd86d576-2e34-45e3-9556-76f9c74dbf17.png)

## Features

- パラメータ調整機能

	- 以下のパラメータをゲーム画面上で調整できます。
	
		- プレイヤと対戦相手の移動速度
		- プレイヤと対戦相手の打つボールの速度
		- プレイヤの反応速度（モーション操作時）
		- 対戦相手の反応速度
		- 対戦相手のショットに辿り着くまでの移動距離
		- プレイスタイル
		- 試合終了条件

- パラメータ出力機能

	- 以下のパラメータをゲーム終了時にCSVファイルとして出力できます。
	
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

- 対戦プレイ

	- パソコンにゲームパッドを2つ接続することで、2人で対戦プレイをすることができます。
	- 下の画像のように、タイトル画面に現在登録されているコントローラ数が表示されます。（下の画像では2つ接続されている）
	
	![MinimumTennis_登録コントローラ数](https://user-images.githubusercontent.com/77042312/187391138-cc945035-79b5-4f0b-b90d-22efeb7b9c2e.png)
	
	- ホーム画面で「Competition」ボタンをクリックすると、対戦プレイの画面へ遷移します。


- 複数のコントローラによる操作

	- 本ゲームは、キーボード操作/ゲームパッド操作/Joy-Con操作（モーション操作） の3つに対応しています。
	- キーボード操作/ゲームパッド操作でプレイする場合は、ホーム画面で「Normal Control」をクリックして下さい。
	- Joy-Con操作（モーション操作）でプレイする場合は、ホーム画面で「Motion Control」をクリックして下さい。

### 研究利用例

1. ゲームコントローラでの操作と、モーションコントローラでの操作におけるユーザエクスペリエンスの比較と評価

	- 本ゲームを、ゲームパッド操作とJoy-Con操作でそれぞれプレイしてもらい、観察・アンケート・CSV出力されたデータをもとに比較と評価を行う

2. 運動を伴うビデオゲームがユーザに与える影響の調査

	- 本ゲームを、Joy-Con操作でプレイしてもらい、観察・アンケート・CSV出力されたデータをもとに比較と評価を行う

## Requirement

OS：Windows, Mac

Unity：2021.1.17f1

## Installation

Unityは[こちら](https://unity3d.com/jp/get-unity/download/archive)からインストールできます。

本リポジトリのデータは下記のコマンドを入力することでローカル環境にクローンできます。
```
git clone https://github.com/open-video-game-library/MinimumTennis.git
```

ローカル環境にクローンしたUnityのプロジェクトファイルを、上記の"Requirement"の環境で開きます。

開くと、下の画像のような警告が表示されますが、"Ignore"をクリックして下さい。

![warning window](https://user-images.githubusercontent.com/77042312/214867252-7a2321ce-a256-45fe-98bf-fc666866d57f.png)

読み込みが終わり、Unityのエディタが開くと、下の画像のように6件のエラーが発生しています。

![error](https://user-images.githubusercontent.com/77042312/214869138-33a29413-7143-4a01-b13c-460ff922870e.png)

Unityのエディタが開く前に警告が表示されていたのは、このエラーが理由です。

エラーの内容を簡単にまとめると、「Joy-Conを利用するためのアセットが見つからない」となります。

Joy-ConをUnityで利用するためのアセットはサードパーティ製であるため、以下ではそのアセットの導入方法について説明します。

まず、UnityでJoy-Conを利用するためのアセットを[こちらのページ](https://github.com/Looking-Glass/JoyconLib/releases)からダウンロードしていきます。

下の画像の赤枠で囲われている"JoyconLib06.unitypackage"をクリックし、ダウンロードします。

![JoyconLib_GitHub](https://user-images.githubusercontent.com/77042312/214875311-4a56ce12-4cbd-4cff-9676-d66843bee620.png)

ダウンロードした"JoyconLib06.unitypackage"を下の画像のように、UnityエディタのProjectウインドウにドラッグアンドドロップします。

![JoyconLib_UnityEditor](https://user-images.githubusercontent.com/77042312/214879097-f879126f-4785-41b9-8420-5a102b6c3094.png)

すると、下の画像のようなウインドウが表示されるので、そのまま"Import"をクリックして下さい。

![JoyconLib_Import](https://user-images.githubusercontent.com/77042312/214879647-253bfb25-87c4-451e-a431-632cf38e819f.png)

アセットの読み込みが終了するとProjectファイルにアセットがインポートされ、先程まで出ていたエラーが消えますが、このままゲームを動かすとまたエラーが出てしまいます。

このエラーは、インポートしたアセットが理由で起こっているため、アセット内のスクリプトを少し書き換えていきます。

下の画像が指すように、Projectファイルにある"JoyconLib_scripts"フォルダ内の"Joycon"スクリプトを開きます。

![JoyconLib_EditScript](https://user-images.githubusercontent.com/77042312/215122961-42ff274b-6e4c-40a0-9754-0051295fbf47.png)

開いたスクリプトの370行目のコードを、変更前のコードから以下のコードに書き換えて下さい。

変更前
```
DebugPrint(string.Format("Dequeue. Queue length: {0:d}. Packet ID: {1:X2}. Timestamp: {2:X2}. Lag to dequeue: {3:s}. Lag between packets (expect 15ms): {4:s}",
```

変更後
```
DebugPrint(string.Format("Dequeue. Queue length: {0:d}. Packet ID: {1:X2}. Timestamp: {2:X2}. Lag to dequeue: {3:t}. Lag between packets (expect 15ms): {4:g}",
```

コードを書き換えたら、保存をします。

この状態でゲームを起動し、エラーが出なければ導入成功です。

## Usage

Minimum Tennis が動作するために必要なデータはすべて同梱されています。

3DモデルやSkyboxなどのアセットを変更したい場合は、その都度インポート・置き換えをして下さい。

また、プロジェクトに含まれているアセットの中には、外部が提供している再配布が認められた素材も含まれています。それらを含んだゲームデータを公開する場合は、各アセット毎に同梱されたライセンスファイルをご確認下さい。

## Licence

本コンテンツは、[MITライセンス](https://github.com/open-video-game-library/Minimum-Tennis/blob/main/LICENSE.md)のもとで利用が許可されています。

## Citation

研究者が利用しやすいオープンなスポーツゲームの試作

[論文はこちら](http://www.interaction-ipsj.org/proceedings/2022/data/pdf/4D18.pdf)

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

意見や要望、質問などがありましたら、[こちら](https://openvideogame.cc/contact)からお問い合わせ下さい。

