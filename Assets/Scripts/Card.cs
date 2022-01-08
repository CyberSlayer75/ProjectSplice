using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    //public
    public string Name() => m_CardName;
    public string Desc() => m_CardDesc;
    public int Cost() => m_CardCost;
    public CardData data;
    //private
    string m_CardName;
    string m_CardDesc;
    int m_CardCost;

    public Card(string name)
    {
        m_CardName = name;
    }
    public Card(CardData d)
    {
        data = d;
        m_CardName = d.CardName;
        m_CardCost = d.CardCost;
        m_CardDesc = d.CardDesc;
    }

    public bool PlayCard()
    {

        return false;//unable to play the card
    }
}
