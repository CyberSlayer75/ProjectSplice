using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    //public
    public string Name() => m_CardName;
    public int Cost() => m_CardCost;
    public CardData data;
    //private
    string m_CardName;
    int m_CardCost;

    public Card(string name)
    {
        m_CardName = name;
    }
}
