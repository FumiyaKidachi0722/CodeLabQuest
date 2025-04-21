using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class DraggableBlock : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Tooltip("Forward/TurnRight/TurnLeft などの命令タイプ")]
    public string commandType;

    [HideInInspector]
    public bool isPaletteBlock = true;

    private Canvas        parentCanvas;
    private RectTransform rectTransform;
    private CanvasGroup   canvasGroup;
    private Transform     originalParent;
    private int           originalSiblingIndex;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup   = GetComponent<CanvasGroup>();
        parentCanvas  = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent       = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();

        // Canvas直下へ移動。ローカル設定を維持。
        transform.SetParent(parentCanvas.transform, false);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha           = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // スクリーン座標 → ワールド座標 へ
        Vector3 worldPoint;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                parentCanvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out worldPoint))
        {
            rectTransform.position = worldPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha           = 1f;

        // パレット由来の元データだけ戻す
        if (isPaletteBlock && transform.parent == parentCanvas.transform)
        {
            transform.SetParent(originalParent, false);
            transform.SetSiblingIndex(originalSiblingIndex);
        }
    }
}
