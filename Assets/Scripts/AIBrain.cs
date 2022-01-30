using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AIBrain : ScriptableObject
{
    public void PlayMove(PlayerController pc)
    {
        int index = 0;
        if(pc.Stats().Energy() > 0)
        {
            while(index < pc.Hand().CardsInDeck())
            {
                if (pc.Hand().m_CardsInDeck[index].card.PlayCard())
                {
                    pc.Grave().AddCardsToDeck(new Deck.CardPackage[] { pc.Hand().m_CardsInDeck[index] });
                    UIManager.Instance.SendCardToGrave(pc.Hand().m_CardsInDeck[index].cardObj);
                    pc.Hand().RemoveCardByIndex(index);
                    break;
                }
                else
                {
                    index++;
                }
            }
            if(index >= pc.Hand().CardsInDeck())
                EndTurn(pc); //We have no cards that we will play
        }
        else
        {
            EndTurn(pc);
        }
    }

    public void EndTurn(PlayerController pc)
    {
        pc.DiscardHand();
        BattleManager.Instance.EndPlayerTurn();
    }
}
