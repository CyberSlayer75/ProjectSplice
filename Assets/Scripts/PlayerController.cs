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
    Deck m_Deck = new Deck();
    Deck m_Grave = new Deck();
    Deck m_Hand= new Deck();
    PlayerStats m_Stats;

    private void Start()
    {
        Card[] temp = new Card[20];
        for(int i = 0; i < 20; i++)
        {
            temp[i] = new Card("Test Card: " + i.ToString());
        }
        m_Deck.AddCardsToDeck(temp);
        m_Deck.PrintDeck();
        DrawCards(5, DeckType.PlayerDeck, DeckType.PlayerHand);
        m_Deck.PrintDeck();
        m_Hand.PrintDeck();
    }

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
                h.AddCardsToDeck(d.Draw(d.CardsInDeck()));//Get last cards from deck
                d.AddCardsToDeck(g.Draw(g.CardsInDeck()));//Shuffle graveyard back into Deck
                d.Shuffle(); //Shuffle our deck
                if (leftOver < d.CardsInDeck())
                {
                    h.AddCardsToDeck(d.Draw(leftOver));//Get remaining cards from deck
                }
                else
                {
                    Debug.LogError("Trying to draw more cards than we have access to");
                }
            }
            else
            {
                GetDeck(to).AddCardsToDeck(GetDeck(from).Draw(numToDraw));
            }
        }
        
        else
        {
            GetDeck(to).AddCardsToDeck(GetDeck(from).Draw(numToDraw));
        }
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
    
    
}
