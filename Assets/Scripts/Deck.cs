using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deck
{
    //public
    public string DeckName;
    public int CardsInDeck() => m_CardsInDeck.Count;

    //private
    [NaughtyAttributes.ReorderableList]
    public List<Card> m_CardsInDeck = new List<Card>();

    /// <summary>
    /// Shuffle the cards in the deck
    /// </summary>
    public void Shuffle()
    {
        for (int i = 0; i < m_CardsInDeck.Count; i++)
        {
            Card temp = m_CardsInDeck[i];
            int randomIndex = Random.Range(i, m_CardsInDeck.Count);
            m_CardsInDeck[i] = m_CardsInDeck[randomIndex];
            m_CardsInDeck[randomIndex] = temp;
        }
    }


    /// <summary>
    /// Draw cards from this deck
    /// </summary>
    /// <param name="amountToDraw">How many cards to draw</param>
    /// <returns></returns>
    public Card[] Draw(int amountToDraw)
    {
        Card[] drawnCards = new Card[amountToDraw];
        if (amountToDraw > m_CardsInDeck.Count)//Make sure we don't draw more than we have
            amountToDraw = m_CardsInDeck.Count;
        for (int i = 0; i < amountToDraw; i++)
        {
            drawnCards[i] = m_CardsInDeck[0];//pop the first card in the list
            m_CardsInDeck.RemoveAt(0);
        }
        return drawnCards;
    }

    /// <summary>
    /// Adds cards to the Deck
    /// </summary>
    /// <param name="newCards">Cards to add to the deck</param>
    public void AddCardsToDeck(Card[] newCards)
    {
        for (int i = 0; i < newCards.Length; i++)
        {
            m_CardsInDeck.Add(newCards[i]);
        }
    }

    public void PrintDeck()
    {
        string s = "";
        for (int i = 0; i < m_CardsInDeck.Count; i++)
        {
            s += m_CardsInDeck[i].Name() + "\n";
        }
        Debug.Log(s);
    }
}
