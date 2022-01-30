using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerController : MonoBehaviour
{
    //Public
    public string PlayerName;
    public bool PlayerControlled = false;
    public Deck Deck() => m_Deck;
    public Deck Grave() => m_Grave;
    public Deck Hand() => m_Hand;
    public PlayerStats Stats() => m_Stats;
    public enum DeckType {PlayerDeck, EnemyDeck, PlayerGrave, EnemyGrave, PlayerHand, EnemyHand };

    //Private
    Deck m_Deck;
    Deck m_Grave;
    Deck m_Hand;
    [NaughtyAttributes.HorizontalLine(color: NaughtyAttributes.EColor.Green)]
    public PlayerStats m_Stats;
    public AIBrain AI;

    //Debug
    [NaughtyAttributes.HorizontalLine(color: NaughtyAttributes.EColor.Red)]
    public DeckData debugDeck;


    private void Start()
    {
        m_Stats = new PlayerStats(this);
        CreateDeck();
        //if (PlayerControlled)
        //    UIManager.Instance.UpdateDisplays();
        //DrawCards(5, DeckType.PlayerDeck, DeckType.PlayerHand);
        //if (PlayerControlled)
        //    UIManager.Instance.UpdateDisplays();
    }

    public void CreateDeck()
    {
        m_Deck = new Deck(DeckType.PlayerDeck, this);
        m_Grave = new Deck(DeckType.PlayerGrave, this);
        m_Hand = new Deck(DeckType.PlayerHand, this);

        Deck.CardPackage[] temp = new Deck.CardPackage[debugDeck.Cards.Count];
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i] = new Deck.CardPackage(new Card(debugDeck.Cards[i].card, this));
        }
        m_Deck.AddCardsToDeck(temp);
        m_Deck.Shuffle();
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
                    if (PlayerControlled) //If we are a player then we need to physically make the cards and put them on the UI
                        UIManager.Instance.CreateCardOnDeckAndSendToHand(h, DeckType.PlayerDeck);
                    else //If we are an enemy then we want to make them behind the scenes
                        UIManager.Instance.CreateCardFromDeckAndSendToHandEnemy(h);
                }
                d.AddCardsToDeck(g.Draw(g.CardsInDeck()));//Shuffle graveyard back into Deck
                d.Shuffle(); //Shuffle our deck
                if (leftOver < d.CardsInDeck())
                {
                    for (int i = 0; i < leftOver; i++)//Get remaining cards from deck
                    {
                        h.AddCardsToDeck(d.Draw(1));
                        if (PlayerControlled) //If we are a player then we need to physically make the cards and put them on the UI
                            UIManager.Instance.CreateCardOnDeckAndSendToHand(h, DeckType.PlayerDeck);
                        else //If we are an enemy then we want to make them behind the scenes
                            UIManager.Instance.CreateCardFromDeckAndSendToHandEnemy(h);
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
                    if (PlayerControlled) //If we are a player then we need to physically make the cards and put them on the UI
                        UIManager.Instance.CreateCardOnDeckAndSendToHand(GetDeck(to), DeckType.PlayerDeck);
                    else //If we are an enemy then we want to make them behind the scenes
                        UIManager.Instance.CreateCardFromDeckAndSendToHandEnemy(GetDeck(to));
                }
            }
        }
        
        else
        {
            GetDeck(to).AddCardsToDeck(GetDeck(from).Draw(numToDraw));
        }
        //if (PlayerControlled)
        //    UIManager.Instance.UpdateDisplays();
    }
    
    public void DiscardHand()
    {
        int cardsToDiscard = GetDeck(DeckType.PlayerHand).CardsInDeck();
        for (int i = 0; i < cardsToDiscard; i++)
        {
            GetDeck(DeckType.PlayerGrave).AddCardsToDeck(GetDeck(DeckType.PlayerHand).Draw(1)); //Move all the cards in hand to the grave
            UIManager.Instance.SendCardToGrave(GetDeck(DeckType.PlayerGrave).m_CardsInDeck[GetDeck(DeckType.PlayerGrave).CardsInDeck() - 1].cardObj);
        }
        //if (PlayerControlled)
        //    UIManager.Instance.UpdateDisplays();
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

    public void RunAI()
    {
        //This is where we call the enemy AI to have them perform actions
        AI.PlayMove(this);
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
