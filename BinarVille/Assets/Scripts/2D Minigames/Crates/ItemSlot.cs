using System.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public int cauldronId;
    [SerializeField] private List<Shrivelable> itemsContained;
    [SerializeField] private int capacity;
    [SerializeField] private GameObject flowersPrefab;
    [SerializeField] private Canvas canvas;
    [SerializeField] private bool isFilledVar;

    public void OnDrop(PointerEventData eventData)
    {
       // Make flower shrivel if there's already another flower in the same crate
        if (eventData.pointerDrag != null)
        {
            //UnityEngine.Debug.Log("### " + eventData.pointerCurrentRaycast.gameObject);
            GameObject objectBelow = eventData.pointerCurrentRaycast.gameObject;

            if (objectBelow.tag == "Cauldron")
            {
                eventData.pointerDrag.GetComponent<DragItem>().SetContainingCauldron(eventData.pointerCurrentRaycast.gameObject);            
                AddItem(eventData.pointerDrag.transform.GetChild(0).GetComponent<Shrivelable>());
            }
            else if (objectBelow.tag == "Magic Flower")
            {
                eventData.pointerDrag.GetComponent<DragItem>().SetContainingCauldron(eventData.pointerCurrentRaycast.gameObject.transform.parent.GetComponent<DragItem>().GetContainingCauldron());            
                AddItem(eventData.pointerDrag.transform.GetChild(0).GetComponent<Shrivelable>());
            }
            
            
        }

        //Debug.Log("Cauldron " + cauldronId + ": Dropped in " + eventData.pointerCurrentRaycast.gameObject);

        /*
        GameObject objectBelow = eventData.pointerCurrentRaycast.gameObject;
        if (objectBelow.tag == "Cauldron")
        {
            eventData.pointerDrag.GetComponent<DragItem>().SetContainingCauldronId(Int32.Parse(objectBelow.name.Split(" ")[1]));
        }
        else
        {
            eventData.pointerDrag.GetComponent<DragItem>().SetContainingCauldronId(0);
        }
        */

        /*
        // Snap item into slot
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }
        */
    }    

    public void AddItem(Shrivelable item)
    {
        if (itemsContained == null)
        {
            itemsContained = new List<Shrivelable>();
        }

        itemsContained.Add(item);
        isFilled();
    }

    public void RemoveItem(Shrivelable item)
    {
        if (itemsContained == null)
        {
            itemsContained = new List<Shrivelable>();
        }
        else
        {
            itemsContained.Remove(item);
            isFilled();   
        }
    }

    public void isFilled()
    {
        if (itemsContained != null)
        {
            if (itemsContained.Count > this.capacity)
            {
                UnityEngine.Debug.Log("Cauldron filled!");
                foreach (Shrivelable item in itemsContained)
                {
                    item.isShrivelEnabled(true);
                }
                isFilledVar = true;
            }
            else
            {
                foreach (Shrivelable item in itemsContained)
                {
                    item.isShrivelEnabled(false);
                }
                isFilledVar = false;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            UnityEngine.Debug.Log("User is already dragging");
            return;
        }

        RectTransform containerRect = this.GetComponent<RectTransform>();
        
        var xcord = UnityEngine.Random.Range(-(containerRect.rect.xMin * .35f), -containerRect.rect.xMax * .35f);
        var ycord = UnityEngine.Random.Range(containerRect.rect.yMin * .25f, containerRect.rect.yMax * .35f);        
        Vector3 spawningPosition = new Vector3(xcord, ycord, transform.position.z) + containerRect.transform.position;
        GameObject flower = Instantiate(flowersPrefab, this.GetComponent<RectTransform>().position, Quaternion.identity) as GameObject;
        //Debug.Log("capacity: " + capacity + " x: " + containerRect.x + " width: " + containerRect.width + " y: " + containerRect.y + " height: " + containerRect.height + " 1: " + flowersPrefab.GetComponent<RectTransform>().rect.width);
        //Vector3 spawningPosition = new Vector3(Random.Range(containerRect.x, containerRect.x + containerRect.width - flowersPrefab.GetComponent<RectTransform>().rect.width), Random.Range(containerRect.y, containerRect.y + containerRect.height - flowersPrefab.GetComponent<RectTransform>().rect.height), transform.position.z);
        
        flower.GetComponent<RectTransform>().SetParent(GameObject.FindWithTag("Magic Flowers Aggregate")?.GetComponent<RectTransform>(), false);
        flower.GetComponent<RectTransform>().localScale = flower.GetComponent<RectTransform>().localScale * canvas.scaleFactor;
        flower.GetComponent<RectTransform>().position = transform.TransformPoint(spawningPosition);
        flower.GetComponent<DragItem>().SetContainingCauldron(this.gameObject);
        AddItem(flower.transform.GetChild(0).GetComponent<Shrivelable>());
        
    }
}
