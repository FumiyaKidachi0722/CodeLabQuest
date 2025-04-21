using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    [Tooltip("ドロップされたブロックを格納する親 RectTransform")]
    public RectTransform contentParent;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop called on " + gameObject.name);

        var dragObj = eventData.pointerDrag;
        if (dragObj == null) return;

        var draggable = dragObj.GetComponent<DraggableBlock>();
        if (draggable == null) return;

        if (draggable.isPaletteBlock)
        {
            // パレット由来の元データ => クローンを生成
            var clone = Instantiate(dragObj);
            clone.transform.SetParent(contentParent, false);
            var rt = clone.GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;

            // クローンはコマンドライン用なのでフラグをオフに
            var cloneDraggable = clone.GetComponent<DraggableBlock>();
            cloneDraggable.isPaletteBlock = false;
        }
        else
        {
            // 既存クローンの移動 => 親を再設定
            dragObj.transform.SetParent(contentParent, false);
            var rt = dragObj.GetComponent<RectTransform>();
            rt.anchoredPosition = Vector2.zero;
        }
    }
}
