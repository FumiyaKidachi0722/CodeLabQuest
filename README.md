# CodeLabQuest

このドキュメントは、Unity プロジェクト「CodeLabQuest」のセットアップから実装手順、重要ポイントまでをまとめたものです。

---

## 1. プロジェクト作成

1. Unity Hub で新規プロジェクトを作成（テンプレートは `3D` ）
2. 適切なプロジェクト名と保存先を設定

## 2. フォルダ構成

```
Assets/
  Art/
  Audio/
  Material/
  Prefabs/
  RenderTextures/
  Resources/
  Scenes/
  Scripts/
    Core/
    UI/
    Utils/
  StageData/
  TextMesh Pro (package)
Packages/
```

- `Assets/Prefabs`：ブロックやゴールの Prefab を格納
- `Assets/RenderTextures`：RenderTexture アセットを作成
- `Assets/Scripts/Core`：ゲームロジック
- `Assets/Scripts/UI`：ドラッグ＆ドロップや UI 管理
- `Assets/StageData`：ScriptableObject でステージ設定

## 3. Prefab 準備

1. Blcok プレハブ (`ForwardBlock`, `TurnLeftBlock`, `TurnRightBlock`) を `Assets/Prefabs` に配置
2. Goal 用プレハブを同じく `Assets/Prefabs/Goal.prefab` として用意
3. 各 Prefab に `DraggableBlock` スクリプト（UI 側）, `GoalTrigger`（Goal オブジェクト）などをアタッチ

## 4. シーン構成 (MainScene)

1. シーンを新規作成し `MainScene` として保存
2. `Main Camera`, `Directional Light`, `Global Volume` などを配置
3. `Canvas` を配置し、以下の子オブジェクトを作成
   - **BlockPalette**
     - Content (HorizontalLayoutGroup + ContentSizeFitter)
     - ForwardBlock, TurnLeftBlock, TurnRightBlock (ドラッグ＆ドロップで Prefab 配置)
   - **CommandLine**
     - CommandLineBG (Image)
     - Content (HorizontalLayoutGroup + ContentSizeFitter + DropZone)
   - **GameViewContainer**
     - GameView (RawImage)
   - UI Buttons / Text
     - START_Button, RESET_Button, PREV_Button, NEXT_Button, StageLabel (TMP)
4. `EventSystem` を Canvas 配下に自動生成
5. `Player`, `Goals` の GameObject をルートに用意
6. `RenderCamera` をルートに追加（3D ゲーム用）

## 5. RenderTexture & GameView

1. `Assets/RenderTextures/GameViewRT` を右クリック →Create→Render Texture
2. `RenderCamera` の Additional Camera Data > Output > Output Texture に `GameViewRT` をセット
3. Clear Flags を **Solid Color**、背景色を `(0,0,0,0)`（透過）に設定
4. Culling Mask から UI を外し、地形・Player のみ映す
5. Canvas/GameViewContainer/GameView (RawImage) に `GameViewRT` を割り当て
6. RectTransform でアンカーを上部領域（Min=(0,0.4), Max=(1,1)）に設定
7. `AspectRatioFitter` を `Width Controls Height`, Aspect Ratio=16:9 に設定

## 6. スクリプト一覧

- **UI/DraggableBlock.cs**：ブロックをドラッグ可能にする
- **UI/DropZone.cs**：ドロップエリアでブロックを受け入れ、CommandLine に配置
- **Core/ForwardCommand.cs, TurnLeftCommand.cs, TurnRightCommand.cs**：`ICommand` 実装
- **Core/ExecutionManager.cs**：コマンドリストを読み込み、`Player` を動かす
- **Core/LevelManager.cs**：ステージ切替、初期化、Palette 生成
- **Core/GoalTrigger.cs**：ゴール到達判定
- **StageData.cs**：ScriptableObject にてステージ設定（プレイヤー初期位置・ゴール Prefab・初期コマンド配列）

## 7. 実行手順

1. `LevelManager` に各参照をインスペクターで割り当て
2. `StageData` アセットを作成し、`LevelManager.stages` に登録
3. Play ▶︎ ボタンでシーンを実行
4. ドラッグしたブロックを CommandLine にドロップ
5. START ボタンで実行、RESET でリセット、Prev/Next でステージ切替

## 8. よくあるトラブルと対策

- **DropZone に入らない**：`Content` に `DropZone` スクリプトがアタッチされているか、`Content` の `RectTransform` が Stretch しているか
- **Prefab がヒエラルキーに出ない/名前が Clone で増える**：`AddPaletteBlock` の `Instantiate(prefab, paletteContent)` と `isPaletteBlock` を確認
- **RenderTexture が真っ黒**：`RenderCamera` の Target Texture, Clear Flags, Culling Mask の設定を再確認
- **GameView が縦長/横長に歪む**：RawImage の `RectTransform` アンカー、`AspectRatioFitter` の設定
