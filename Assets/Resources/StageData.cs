using UnityEngine;

/// <summary>
/// １ステージ分のデータを保持する ScriptableObject
/// </summary>
[CreateAssetMenu(
    fileName = "NewStageData",
    menuName = "CodeLabQuest/StageData",
    order    = 0)]
public class StageData : ScriptableObject
{
    [Header("ステージ基本情報")]
    public string stageName;           // ステージ名

    [Header("プレイヤー初期位置・向き")]
    public Vector3 playerStartPos;     // プレイヤー開始座標
    public Vector3 playerStartEuler;   // プレイヤー開始回転（Euler角）

    [Header("ゴールオブジェクト")]
    public Vector3 goalPosition;       // ゴールの座標
    public GameObject goalPrefab;      // ゴール用Prefab

    [Header("パレット初期コマンド")]
    public string[] initialCommands;   // 最初から並べるコマンドタイプ列
}
