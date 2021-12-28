using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu]
public class CardData : ScriptableObject
{
   public string CardName;
   public int CardCost;
   public int CardLevel;
   public int CardDurability;
   public enum CColor { Red, Blue, Green};
   public enum CType { Attack, Skill, Power};
   public enum CRarity { Common, Uncommon, Rare, Legendary}
   public CColor CardColor;
   public CType CardType;
   public CRarity CardRarity;
    [ResizableTextArea]
    public string CardDesc;
}
