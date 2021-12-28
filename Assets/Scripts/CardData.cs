using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardData : ScriptableObject
{
   public string CardName;
   public int CardCost;
   public int CardLevel;
   public string CardDesc;
   public int CardDurability;
   public enum CardColor { Red, Blue, Green};
   public enum CardType { Attack, Skill, Power};
   public enum CardRarity { Common, Uncommon, Rare, Legendary}
   public CardColor cColor;
   public CardType cType;
   public CardRarity cRarity;

}
