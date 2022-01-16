using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IMoveHandler, IPointerEnterHandler, IPointerExitHandler
{
    //public
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI cardDescText;
    public Image cardImage;

    //private
    Card myCard;
    Transform originalParent;
    Canvas cardCanvas;


    private void Awake()
    {
        cardCanvas = GetComponent<Canvas>();
    }

    /// <summary>
    /// Set the info on this card 
    /// </summary>
    /// <param name="c">card to use</param>
    public void SetCard(Card c)
    {
        myCard = c;
        cardNameText.text = myCard.Name();
        cardDescText.text = myCard.Desc();
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UIManager.Instance.canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out pos);
        this.transform.localPosition = pos;
        //throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnMove(AxisEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        originalParent = this.transform.parent;
        this.transform.SetParent(UIManager.Instance.canvas.transform);
        Vector2 pos; 
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UIManager.Instance.canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out pos);
        this.transform.localPosition = pos;
        //this.transform.localPosition = UIManager.Instance.canvas.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint( eventData.position));
        //throw new System.NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (UIManager.Instance.isCardInPlayZone(this.transform.localPosition))
        {
            if (this.myCard.PlayCard())//If we successfully played it
            {
                UIManager.Instance.SendCardToGrave(this.gameObject);
            }
            else //If we couldn't play the card then return it to our hand
            {
                UIManager.Instance.SendCardToHand(this.gameObject);
            }
        }
        else
        {
            this.transform.SetParent(originalParent);
        }
        //throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.transform.localScale = Vector3.one * 1.5f;
        this.transform.localPosition += Vector3.up * 50f;
        cardCanvas.sortingOrder = 10;
        //throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.transform.localScale = Vector3.one;
        this.transform.localPosition += Vector3.down * 50f;
        cardCanvas.sortingOrder = 0;
        //throw new System.NotImplementedException();
    }
}
