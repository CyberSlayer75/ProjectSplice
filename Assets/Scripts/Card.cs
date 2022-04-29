using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class Card
{
    //public
    public string Name() => m_CardName;
    public string Desc() => m_CardDesc;
    public int Cost() => m_CardCost;
    public CardData data;
    public PlayerController originalOwner;
    public PlayerController currentOwner;
    //private
    string m_CardName;
    string m_CardDesc;
    int m_CardCost;

    public Card(string name)
    {
        m_CardName = name;
    }
    /// <summary>
    /// Used to make cards whenever without assigning an owner
    /// </summary>
    /// <param name="d"></param>
    public Card(CardData d)
    {
        data = d;
        m_CardName = d.CardName;
        m_CardCost = d.CardCost;
        m_CardDesc = d.CardDesc;
    }
    /// <summary>
    /// Strictly used when creating cards for a deck during battle time
    /// </summary>
    /// <param name="d"></param>
    /// <param name="pc"></param>
    public Card(CardData d, PlayerController pc)
    {
        data = d;
        data.InitializeStartingCurrentValues();
        m_CardName = d.CardName;
        m_CardCost = d.CardCost;
        m_CardDesc = d.CardDesc;
        originalOwner = pc;
        currentOwner = originalOwner;
    }
    public bool PlayCard()
    {
        bool cardPlayed = false;
        if (data.CurrentCardCost > currentOwner.Stats().Energy())
            return false; //Not enough energy to play
        for(int i = 0; i < data.CardEffects.Count; i++)
        {
            List<PlayerController> targets = GetTargets(data.CardEffects[i].target); //Get the targets of the card
            
            if(data.CardEffects[i].conditional) //If this effect has a condition then lets make sure we meet it
            {
                bool conditionMet = false;
                List<PlayerController> conTar = GetTargets(data.CardEffects[i].conditionalStatement.conditionTarget);
                for(int k = 0; k < conTar.Count; k++)
                {
                    conditionMet = CheckCondition(conTar[k], data.CardEffects[i].conditionalStatement);
                    if (conditionMet)
                        break;
                }
                if (!conditionMet) //Stop with this effect because we didn't meet the condition
                    break;
            }
            cardPlayed = true;
            //This is where we apply statuses and damage
            foreach (PlayerController pc in targets)
            {
                if (data.CardEffects[i].damage > 0) //Player Takes Damage
                {
                    //TODO: Check for Damage Mods

                    pc.Stats().TakeDamage(data.CardEffects[i].damage);
                }
                if(data.CardEffects[i].buffs != null) //Player gets or loses Buffs
                {
                    for(int b = 0; b < data.CardEffects[i].buffs.Count; b++)
                    {
                        if (data.CardEffects[i].buffs[b].count > 0) //If positive number gain the buff
                        {
                            pc.Stats().GainStatus(data.CardEffects[i].buffs[b].buff, data.CardEffects[i].buffs[b].count);
                        }
                    }
                }
                if (data.CardEffects[i].debuffs != null) //Player gets or loses Debuffs
                {
                    for (int b = 0; b < data.CardEffects[i].debuffs.Count; b++)
                    {
                        if (data.CardEffects[i].debuffs[b].count > 0) //If positive number gain the buff
                        {
                            pc.Stats().GainStatus(data.CardEffects[i].debuffs[b].debuff, data.CardEffects[i].debuffs[b].count);
                        }
                    }
                }
                //TODO: This is where unique buffs will go
            }
        }
       if(cardPlayed)
        {
            currentOwner.Stats().LoseEnergy(data.CurrentCardCost);
        }

        return cardPlayed;
    }

    public List<PlayerController> GetTargets(CardEffects.TargetingType tar)
    {
        List<PlayerController> targets = new List<PlayerController>(); //Get the targets of the card
        switch (tar)// get the target
        {
            case CardEffects.TargetingType.Self:
                {
                    targets.Add(this.currentOwner);
                    break;
                }
            case CardEffects.TargetingType.SingleEnemy:
                //wait for player to pick target
                break;
            case CardEffects.TargetingType.RandomEnemy:
                {
                    targets.Add(BattleManager.Instance.GetRandomEnemy(this.currentOwner));

                    break;
                }
            case CardEffects.TargetingType.MultipleEnemies:
                //Wait for player to pick multiple targets
                break;
            case CardEffects.TargetingType.AllEnemies:
                {
                    targets.AddRange(BattleManager.Instance.GetAllEnemies(this.currentOwner));
                    break;
                }
            default:
                Debug.LogError("Card has no target");
                break;
        }
        return targets;
    }

    bool CheckCondition(PlayerController target, CardEffects.ConditionalEffect conditionalEffect)
    {
        switch(conditionalEffect.condition)
        {
            case CardEffects.ConditionalEffect.Conditionals.Equal:
                {
                    return conditionalEffect.countOf == GetStats(target, conditionalEffect);
                }
            case CardEffects.ConditionalEffect.Conditionals.LessThan:
                {
                    return GetStats(target, conditionalEffect) < conditionalEffect.countOf;
                }
            case CardEffects.ConditionalEffect.Conditionals.GreaterThan:
                {
                    return  GetStats(target, conditionalEffect) > conditionalEffect.countOf;
                }
        }
        return false;
    }

    int GetStats(PlayerController target, CardEffects.ConditionalEffect conEffect)
    {
        switch(conEffect.conTarget)
        {
            case CardEffects.ConditionalEffect.ConTarget.Damage:
                {
                    return target.Stats().Health();
                }
            case CardEffects.ConditionalEffect.ConTarget.Energy:
                {
                    return target.Stats().Energy();
                }
            case CardEffects.ConditionalEffect.ConTarget.Buff:
                {
                    return target.Stats().ReturnStatus(conEffect.buffStatus);
                }
            case CardEffects.ConditionalEffect.ConTarget.Debuff:
                {
                    return target.Stats().ReturnStatus(conEffect.debuffStatus);
                }
            case CardEffects.ConditionalEffect.ConTarget.Unique:
                {
                    return target.Stats().ReturnStatus(conEffect.uniqueStatus);
                }
        }
        return 0;
    }

}
