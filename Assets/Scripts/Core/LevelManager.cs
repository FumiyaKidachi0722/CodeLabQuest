using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("ステージデータ一覧")]
    public StageData[] stages;

    [Header("再生用プレイヤー & ゴール配置用親")]
    public Transform player;
    public Transform goalParent;

    [Header("パレットにコマンドを並べる親 (BlockPalette/Content)")]
    public RectTransform blockPaletteContent;

    [Header("ドロップされたコマンドを表示する親 (CommandLine/Content)")]
    public RectTransform commandLineContent;

    [Header("ステージ切替ボタン")]
    public Button prevButton;
    public Button nextButton;
    public TMP_Text stageLabel;

    [Header("パレット用ブロックプレハブ")]
    public GameObject forwardBlockPrefab;
    public GameObject turnRightBlockPrefab;
    public GameObject turnLeftBlockPrefab;

    private int currentStage = 0;

    void Start()
    {
        prevButton.onClick.AddListener(() => ChangeStage(-1));
        nextButton.onClick.AddListener(() => ChangeStage(+1));

        LoadStage(currentStage);
        UpdateUI();
    }

    void ChangeStage(int delta)
    {
        int next = currentStage + delta;
        if (next < 0 || next >= stages.Length) return;
        currentStage = next;
        LoadStage(currentStage);
        UpdateUI();
    }

    void LoadStage(int stageIndex)
    {
        var data = stages[stageIndex];

        // 1) プレイヤーの初期化
        player.position    = data.playerStartPos;
        player.eulerAngles = data.playerStartEuler;

        // 2) 既存のゴールをクリア＆再配置
        foreach (Transform child in goalParent) Destroy(child.gameObject);
        if (data.goalPrefab != null)
            Instantiate(data.goalPrefab, data.goalPosition, Quaternion.identity, goalParent);

        // 3) パレットとコマンドラインをそれぞれクリア
        ClearChildren(blockPaletteContent);
        ClearChildren(commandLineContent);

        // 4) パレットに初期コマンドを並べる
        foreach (var cmd in data.initialCommands)
            AddBlockToPalette(cmd);
    }

    void ClearChildren(RectTransform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
            Destroy(parent.GetChild(i).gameObject);
    }

    void AddBlockToPalette(string commandType)
    {
        GameObject prefab = commandType switch
        {
            "Forward"   => forwardBlockPrefab,
            "TurnRight" => turnRightBlockPrefab,
            "TurnLeft"  => turnLeftBlockPrefab,
            _ => null
        };
        if (prefab == null)
        {
            Debug.LogWarning($"Unknown command type: {commandType}");
            return;
        }

        var instance = Instantiate(prefab, blockPaletteContent);
        // isPaletteBlock を明示的に true に
        var drag = instance.GetComponent<DraggableBlock>();
        if (drag != null) drag.isPaletteBlock = true;
    }

    void UpdateUI()
    {
        stageLabel.text         = $"Stage {currentStage+1}/{stages.Length}";
        prevButton.interactable = currentStage > 0;
        nextButton.interactable = currentStage < stages.Length - 1;
    }
}
