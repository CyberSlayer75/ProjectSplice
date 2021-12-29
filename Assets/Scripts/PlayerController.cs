using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerController : MonoBehaviour
{
    //Public
    public string PlayerName;
    public Deck Deck() => m_Deck;
    public Deck Grave() => m_Grave;
    public Deck Hand() => m_Hand;
    public PlayerStats Stats() => m_Stats;
    public enum DeckType {PlayerDeck, EnemyDeck, PlayerGrave, EnemyGrave, PlayerHand, EnemyHand };
    //Private
    Deck m_Deck = new Deck(DeckType.PlayerDeck);
    Deck m_Grave = new Deck(DeckType.PlayerGrave);
    Deck m_Hand= new Deck(DeckType.PlayerHand);
    PlayerStats m_Stats;

    //Debug
    [NaughtyAttributes.HorizontalLine(color: NaughtyAttributes.EColor.Red)]
    public DeckData debugDeck;


    private void Start()
    {
        UpdateCounts();
        Deck.CardPackage[] temp = new Deck.CardPackage[debugDeck.Cards.Count];
        for(int i = 0; i < temp.Length; i++)
        {
            temp[i] = new Deck.CardPackage(new Card( debugDeck.Cards[i].card));
        }
        m_Deck.AddCardsToDeck(temp);
        m_Deck.Shuffle();
        m_Deck.PrintDeck();
        DrawCards(5, DeckType.PlayerDeck, DeckType.PlayerHand);
        m_Deck.PrintDeck();
        m_Hand.PrintDeck();
        UpdateCounts();
    }
    /// <summary>
    /// Draw cards from one deck and put them in another
    /// </summary>
    /// <param name="numToDraw">How many cards should be moved</param>
    /// <param name="from">Deck to take cards from</param>
    /// <param name="to">Deck to put cards into</param>
    public void DrawCards(int numToDraw, DeckType from, DeckType to)
    {
        int leftOver = 0;
        if(from == DeckType.PlayerDeck && to == DeckType.PlayerHand)
        {
            Deck d = GetDeck(DeckType.PlayerDeck);
            Deck h = GetDeck(DeckType.PlayerHand);
            Deck g = GetDeck(DeckType.PlayerGrave);
            if (numToDraw > d.CardsInDeck())
            {
                leftOver = numToDraw - d.CardsInDeck();
                int target = d.CardsInDeck();
                for (int i = 0; i < target; i++)
                {
                    h.AddCardsToDeck(d.Draw(1));//Get last cards from deck
                    UIManager.Instance.CreateCardOnDeckAndSendToHand(h, DeckType.PlayerDeck);
                }
                d.AddCardsToDeck(g.Draw(g.CardsInDeck()));//Shuffle graveyard back into Deck
                d.Shuffle(); //Shuffle our deck
                if (leftOver < d.CardsInDeck())
                {
                    for (int i = 0; i < leftOver; i++)//Get remaining cards from deck
                    {
                        h.AddCardsToDeck(d.Draw(1));
                        UIManager.Instance.CreateCardOnDeckAndSendToHand(h, DeckType.PlayerDeck);
                    }
                }
                else
                {
                    Debug.LogError("Trying to draw more cards than we have access to");
                }
            }
            else
            {
                for (int i = 0; i < numToDraw; i++)
                {
                    GetDeck(to).AddCardsToDeck(GetDeck(from).Draw(1));
                    UIManager.Instance.CreateCardOnDeckAndSendToHand(GetDeck(to), DeckType.PlayerDeck);
                }
            }
        }
        
        else
        {
            GetDeck(to).AddCardsToDeck(GetDeck(from).Draw(numToDraw));
        }
        UpdateCounts();
    }
    
    public void DiscardHand()
    {
        int cardsToDiscard = GetDeck(DeckType.PlayerHand).CardsInDeck();
        for (int i = 0; i < cardsToDiscard; i++)
        {
            GetDeck(DeckType.PlayerGrave).AddCardsToDeck(GetDeck(DeckType.PlayerHand).Draw(1)); //Move all the cards in hand to the grave
            UIManager.Instance.SendCardFromHandToGrave(GetDeck(DeckType.PlayerGrave).m_CardsInDeck[GetDeck(DeckType.PlayerGrave).CardsInDeck() - 1].cardObj);
        }
        UpdateCounts();
    }

    void UpdateCounts()
    {
        UIManager.Instance.UpdateDisplays(m_Deck.CardsInDeck(), m_Grave.CardsInDeck(), m_Hand.CardsInDeck());
    }


    /// <summary>
    /// Get the deck we need
    /// </summary>
    /// <param name="dt">Deck type we want to get</param>
    /// <returns></returns>
    Deck GetDeck(DeckType dt)
    {
        switch(dt)
        {
            case DeckType.PlayerDeck:
                return m_Deck;
            case DeckType.PlayerGrave:
                return m_Grave;
            case DeckType.PlayerHand:
                return m_Hand;
            default:
                Debug.LogError("Error getting deck: " + dt.ToString());
                break;
        }
        return null;
    }

    #region Debug
    [NaughtyAttributes.Button]
    public void DebugDiscardHand()
    {
        DiscardHand();
    }
    [NaughtyAttributes.Button]
    public void DebugDraw()
    {
        DrawCards(3, DeckType.PlayerDeck, DeckType.PlayerHand);
    }
    #endregion
}
