using System.Collections.Generic;
using Model;
using Services;
using UnityEngine;

namespace Defaults
{
    /// <summary>
    /// 固定値群
    /// </summary>
    public static class GameData
    {
        #region ゲームシーン
            #region ノーツ関連
                /// <summary>
                /// 各レーンのノーツ出現位置
                /// </summary>
                public static readonly Dictionary<LaneName, Vector3> LanePositions = new ()
                {
                    {LaneName.OuterRight,new Vector3(0,0,500)},
                    {LaneName.InnerRight,new Vector3(10,0,500)},
                    {LaneName.InnerLeft,new Vector3(20,0,500)},
                    {LaneName.OuterLeft,new Vector3(30,0,500)},
                    
                    {LaneName.BigRight,new Vector3(5,-0.1f,500)},
                    {LaneName.BigLeft,new Vector3(25,-0.1f,500)}
                };
                
                /// <summary>
                /// ノーツのサイズ
                /// </summary>
                public static Dictionary<LaneName, Vector3> NotesScale = new()
                {
                    {LaneName.OuterRight,new Vector3(0.9f, 1, 0.3f)},
                    {LaneName.InnerRight,new Vector3(0.9f, 1, 0.3f)},
                    {LaneName.InnerLeft,new Vector3(0.9f, 1, 0.3f)},
                    {LaneName.OuterLeft,new Vector3(0.9f, 1, 0.3f)},
                    
                    {LaneName.BigRight,new Vector3(1.9f, 1, 0.3f)},
                    {LaneName.BigLeft,new Vector3(1.9f, 1, 0.3f)}
                };
            #endregion

            #region ジャッチ関連
                /// <summary>
                /// ジャッチ位置
                /// </summary>
                public static readonly Dictionary<JudgeType, float> JudgePosition = new()
                {
                    { JudgeType.Perfect, 10 },
                    { JudgeType.Good, 20 },
                    { JudgeType.Miss, 50 }
                };
                
                /// <summary>
                /// ジャッチによるスコア加算量
                /// </summary>
                public static readonly Dictionary<JudgeType, float> JudgeScore = new()
                {
                    { JudgeType.Perfect, 1000 },
                    { JudgeType.Good, 500 },
                    { JudgeType.Miss, 10 }
                };
                
                /// <summary>
                /// ジャッチ表示時間
                /// </summary>
                public const float JudgeViewTime = 3;
                
                /// <summary>
                /// ジャッチ結果の文字色
                /// </summary>
                public static readonly Dictionary<JudgeType, Color> JudgeColors = new ()
                {
                    { JudgeType.Perfect, new Color(0.9568627f, 0.3686275f, 1) },
                    { JudgeType.Good, new Color(1, 0.5647059f, 0.372549f) },
                    { JudgeType.Miss, new Color(0.3686275f, 1, 0.8509804f) }
                };
                
                /// <summary>
                /// レーンの光る倍率
                /// </summary>
                public static readonly Dictionary<JudgeType, float> JudgeBloom = new()
                {
                    { JudgeType.Perfect, 1.4f },
                    { JudgeType.Good, 1.2f },
                    { JudgeType.Miss, 1 }
                };
                
                /// <summary>
                /// レーンの光っている時間
                /// </summary>
                public const float BloomViewTime = 0.5f;
            #endregion

            #region データ保存
                /// <summary>
                /// 曲の設定保存場所
                /// </summary>
                public static readonly string MusicDataPath = Application.dataPath + @"\Data\MusicData.json";
                /// <summary>
                /// データ保存フォルダ
                /// </summary>
                public static readonly string DataPath = Application.dataPath + @"\Data";
            #endregion

            #region シーン遷移
                /// <summary>
                /// スタートシーン
                /// </summary>
                public const string StartScene = "Start";
                /// <summary>
                /// メインシーン
                /// </summary>
                public const string InGameScene = "InGame";
            #endregion

            #region 指定なしの場合の曲のパラメータ
                /// <summary>
                /// 1分間にどれだけの行数が数えられるか
                /// </summary>
                public const int Bpm = 120;
                /// <summary>
                /// レーンの数
                /// </summary>
                public const int LaneCount = 6;
            #endregion

        #endregion

        #region スタート
            /// <summary>
            /// ボタンの連続入力禁止時間
            /// </summary>
            public const float ButtonMoveDelay = 0.3f;

            /// <summary>
            /// ステータスの調整単位
            /// </summary>
            public static readonly Dictionary<StatusType, float> StatusMass = new()
            {
                { StatusType.Delay, 0.1f },
                { StatusType.Speed, 5 },
                { StatusType.Voltage, 1 }
            };
            
            /// <summary>
            /// ステータス初期値
            /// </summary>
            public static readonly Dictionary<StatusType, float> StatusDefault = new()
            {
                { StatusType.Delay, 0f },
                { StatusType.Speed, 150 },
                { StatusType.Voltage, 5 }
            };
            
            /// <summary>
            /// ステータス最大値
            /// </summary>
            public static readonly Dictionary<StatusType, float> StatusMax = new()
            {
                { StatusType.Delay, 1000f },
                { StatusType.Speed, 300 },
                { StatusType.Voltage, 15 }
            };
            
            /// <summary>
            /// ステータス最低値
            /// </summary>
            public static readonly Dictionary<StatusType, float> StatusMin = new()
            {
                { StatusType.Delay, -1000f },
                { StatusType.Speed, 50 },
                { StatusType.Voltage, 1 }
            };
        #endregion
    }
}