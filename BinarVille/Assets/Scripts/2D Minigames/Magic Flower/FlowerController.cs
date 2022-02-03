using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FlowerController : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Animator animator;

    private void Awake()
    {
        rectTransform = transform.GetChild(0).GetComponent<RectTransform>();
        canvasGroup = transform.GetChild(0).GetComponent<CanvasGroup>();
        canvas = gameObject.transform.parent.parent.GetComponent<Canvas>();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .5f;
        animator.SetBool("wiggle", false);
        animator.enabled = false;
        canvasGroup.blocksRaycasts = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag + " Dragging...\n" + rectTransform.anchoredPosition + " - " + eventData.delta);
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        //rectTransform.anchorMin = eventData.delta / canvas.scaleFactor;
        //rectTransform.anchorMax = eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        animator.enabled = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Pointer down!");
    }
}
