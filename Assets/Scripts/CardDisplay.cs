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
            this.myCard.PlayCard();
        }
        else
        {
            this.transform.SetParent(originalParent);
        }
        //throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }
}
