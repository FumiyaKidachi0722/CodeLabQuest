using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExecutionManager : MonoBehaviour
{
    [Header("並べたブロックを格納した親Transform")]
    public RectTransform contentParent;

    [Header("動かしたいキャラの Transform")]
    public Transform player;

    [Header("実行ボタン")]
    public Button runButton;

    [Header("リセットボタン")]            // 追加
    public Button resetButton;            // 追加

    // 初期位置・回転を保持するフィールド
    private Vector3  initialPlayerPos;    // 追加
    private Quaternion initialPlayerRot;  // 追加

    void Start()
    {
        // プレイヤーの初期状態をキャッシュ
        initialPlayerPos = player.position;       
        initialPlayerRot = player.rotation;      

        // 実行ボタンのリスナー登録
        runButton.onClick.AddListener(() => StartCoroutine(RunCommands()));

        // リセットボタンのリスナー登録 ← 追加
        resetButton.onClick.AddListener(ResetAll);
    }

    private IEnumerator RunCommands()
    {
        runButton.interactable = false;
        resetButton.interactable = false;  // 実行中はリセットも不可に

        foreach (Transform blockTransform in contentParent)
        {
            var block = blockTransform.GetComponent<DraggableBlock>();
            if (block == null) continue;

            ICommand cmd = CommandFactory.Create(block.commandType);
            yield return StartCoroutine(cmd.Execute(player));
        }

        runButton.interactable = true;
        resetButton.interactable = true;
    }

    /// <summary>
    /// コマンドラインをクリアし、キャラクターを初期位置に戻す
    /// </summary>
    private void ResetAll()
    {
        // 1. Content（CommandLine）を空に
        for (int i = contentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(contentParent.GetChild(i).gameObject);
        }

        // 2. キャラの位置・回転を初期状態に戻す
        player.position = initialPlayerPos;
        player.rotation = initialPlayerRot;

        // 3. ボタンを有効化
        runButton.interactable   = true;
        resetButton.interactable = true;
    }
}
