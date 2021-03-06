using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Deck
{
    //public
    public string DeckName;
    public int CardsInDeck() => m_CardsInDeck.Count;
    public PlayerController.DeckType deckType;
    public PlayerController DeckOwner() => deckOwner;
    //private
    PlayerController deckOwner;
    [NaughtyAttributes.ReorderableList]
    public List<CardPackage> m_CardsInDeck = new List<CardPackage>();

    public Deck(PlayerController.DeckType t, PlayerController pc)
    {
        deckType = t;
        deckOwner = pc;
        DeckName = t.ToString();
    }

    /// <summary>
    /// Shuffle the cards in the deck
    /// </summary>
    public void Shuffle()
    {
        for (int i = 0; i < m_CardsInDeck.Count; i++)
        {
            CardPackage temp = m_CardsInDeck[i];
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
    public CardPackage[] Draw(int amountToDraw)
    {
        CardPackage[] drawnCards = new CardPackage[amountToDraw];
        if (amountToDraw > m_CardsInDeck.Count)//Make sure we don't draw more than we have
            amountToDraw = m_CardsInDeck.Count;
        for (int i = 0; i < amountToDraw; i++)
        {
            if (drawnCards[i] == null)
                drawnCards[i] = new CardPackage();
            drawnCards[i].card = m_CardsInDeck[0].card;//pop the first card in the list
            if(m_CardsInDeck[0].cardObj != null)
                drawnCards[i].cardObj = m_CardsInDeck[0].cardObj;//pop the first card in the list
            m_CardsInDeck.RemoveAt(0);
        }
        return drawnCards;
    }
    /// <summary>
    /// Adds cards to the Deck
    /// </summary>
    /// <param name="newCards">Cards to add to the deck</param>
    public void AddCardsToDeck(CardPackage[] newCards)
    {
        for (int i = 0; i < newCards.Length; i++)
        {
            m_CardsInDeck.Add(newCards[i]);
        }
    }
    /// <summary>
    /// Debug Prints the contents of the deck
    /// </summary>
    public void PrintDeck()
    {
        string s = "";
        for (int i = 0; i < m_CardsInDeck.Count; i++)
        {
            s += m_CardsInDeck[i].card.Name() + "\n";
        }
        Debug.Log(s);
    }

    public void RemoveCardByIndex(int cardToRemove)
    {
        m_CardsInDeck.RemoveAt(cardToRemove);
        m_CardsInDeck = m_CardsInDeck.Where(x => x != null).ToList();
    }

    public void RemoveCardByReference(GameObject cardToRemove)
    {
        for(int i = 0; i < m_CardsInDeck.Count; i++)
        {
            if(GameObject.ReferenceEquals(m_CardsInDeck[i].cardObj, cardToRemove))
            {
                RemoveCardByIndex(i);
                return;
            }
        }
    }

    public CardPackage FindCardByObjReference(GameObject cardRef)
    {
        for (int i = 0; i < m_CardsInDeck.Count; i++)
        {
            if (GameObject.ReferenceEquals(m_CardsInDeck[i].cardObj, cardRef))
            {
                return m_CardsInDeck[i];
            }
        }
        return null;
    }

    public class CardPackage
    {
        public Card card;
        public GameObject cardObj;

        public CardPackage(Card c)
        {
            card = c;
        }
        public CardPackage()
        {
            card = null;
            cardObj = null;
        }
    }
}
