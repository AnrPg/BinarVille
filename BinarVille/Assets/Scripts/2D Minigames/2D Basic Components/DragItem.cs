using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{

    private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject shrivelableObject;
    [SerializeField] private Shrivelable shrivel;
    [SerializeField] private GameObject containingCauldron; // Zero value means no cauldron contains the item, otherwise this field has the cauldron's capacity
    //[SerializeField] private GameObject hoveringCauldron; // Zero value means no cauldron contains the item, otherwise this field has the cauldron's capacity

    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        shrivel = shrivelableObject.GetComponent<Shrivelable>();
    }

    public void SetContainingCauldron(GameObject containingCauldron)
    {
        this.containingCauldron = containingCauldron;
    }

    public GameObject GetContainingCauldron()
    {
        return this.containingCauldron;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .5f;
        canvasGroup.blocksRaycasts = false;

        containingCauldron.GetComponent<ItemSlot>().RemoveItem(shrivel);
        SetContainingCauldron(null);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        shrivel.isShrivelEnabled(false);

        /*
        GameObject objectBelow = eventData.pointerCurrentRaycast.gameObject;
        
        if (objectBelow.tag == "Magic Flower")
        {            
            //hoveringCauldronId = Int32.Parse(objectBelow.name.Split(" ")[1]); // All Cauldrons must be named in accordance with the pattern: Cauldron [capacity] - [N-ads] 
            //Debug.Log("hoveringCauldronId: " + hoveringCauldronId + "\ncontainingCauldronId: " + containingCauldronId);

            hoveringCauldron = objectBelow;
        }
        else
        {
            //hoveringCauldronId = 0;
            //Debug.Log("Out of cauldron...");
            hoveringCauldron = null;
        }
        */
        /*
        if (shrivel != null)
        {
            shrivel.isShrivelEnabled(true);
        }
        */
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        GameObject objectBelow = eventData.pointerCurrentRaycast.gameObject;
        
        if (objectBelow.tag == "Magic Flower")
        {   
            SetContainingCauldron(objectBelow.transform.parent.GetComponent<DragItem>().containingCauldron);
            objectBelow.transform.parent.GetComponent<DragItem>().containingCauldron?.SendMessage( "OnDrop", eventData );
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            UnityEngine.Debug.Log("User is already dragging");
            return;
        }

        //UnityEngine.Debug.Log("" + containingCauldron);
        if (containingCauldron != null)
        {   
            containingCauldron.SendMessage( "OnPointerClick", eventData );
        }
    }
}
