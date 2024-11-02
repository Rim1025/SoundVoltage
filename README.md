# SoundVoltage
## 概要
2024年度釧路高専電子分野工学課題実験
音ゲーの作成

## フォルダ構成
### Arduino
Ardiunoに使用したスクリプト
ポート数の関係で左右で分割

### Assets
Unityのプロジェクト

### Packages,ProjectSettings
自動生成(Unityに必要)


## Ssset内概要
### Data
ゲームの流す曲などのプレイヤーによって変更できる情報を保存している

### Fx Explosion Pack
爆発エフェクトパック

### Plagins
使用するプラグイン

### Prefabs
プレハブ

### Resource
曲データが入っている
参照
https://drive.google.com/drive/folders/1l6ubLBY7NDxXCIXa-PEhVjF1UPGig7ui?usp=drive_link

### Scenes
各シーン

### Score
スコアの保存場所

### Scripts
スクリプト群

### StatusText
未使用

## Sssets\Scripts詳細
### InGame
音ゲー本体
#### Audio
Data下から音声ファイルを読み取り管理する
#### Interfases
Interfase群
#### Notes
ノーツクラス群
#### Score
スコアの管理

### Input
入力と出力の仲介
### Serial
Ardiunoとの通信部分
### Start
曲選択及びプレイヤー設定変更シーンに使用するクラス群
### Status
固定値群
### その他
シーン遷移とエラー処理用スクリプト
