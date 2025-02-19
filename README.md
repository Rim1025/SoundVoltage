# SoundVoltage
音ゲーを作りたい  
そうメンバーが言い出したことで作成を開始した  
## 使用方法
筐体と接続し、Unityでビルドすることで使用可能  
### 確認事項
InGameシーン内のSerialオブジェクトにアタッチされているSerialManagerに適切な値が設定されているか  
InputProviderInstallerのバインド先をがSerialManagerになっているか  
ProjectSettings -> Player -> OtherSettings -> ApiCompatibilityLevelが.NETFrameworkになっているか
  
本来筐体とシリアル通信によって信号をやり取りするが
IInputProviderのバインド先をKeyInputに変更することでPCのキーボードでも操作可能

## 曲テンプレート
https://drive.google.com/drive/folders/1l6ubLBY7NDxXCIXa-PEhVjF1UPGig7ui
