using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    //public
    public TextMeshProUGUI cardNameText;
    public TextMeshProUGUI cardDescText;
    public Image cardImage;

    //private
    Card myCard;
    
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
}
