using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu]
public class CardData : ScriptableObject
{
    public string CardName;
    public int CardCost;
    public int CardLevel = 1;
    public int CardDurability;
    public enum CColor { Red, Blue, Green, Colorless };
    public enum CType { Attack, Skill, Power };
    public enum CRarity { Common, Uncommon, Rare, Legendary }
    public CColor CardColor;
    public CType CardType;
    public CRarity CardRarity;
    bool ShowPrev() { return CardLevel > 1; }
    [ShowIf("ShowPrev")]
    public CardData previousCard;
    [Dropdown("GetCards")]
    public CardData upgradedVersion;
    [ResizableTextArea]
    public string CardDesc;


    private DropdownList<CardData> GetCards()
    {
        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:CardData", new string[] { "Assets/CardDatabase" });
        NaughtyAttributes.DropdownList<CardData> ListAsEnum = new NaughtyAttributes.DropdownList<CardData>();
        foreach (string g in guids)
        {
            CardData data = UnityEditor.AssetDatabase.LoadAssetAtPath<CardData>(UnityEditor.AssetDatabase.GUIDToAssetPath(g));
            ListAsEnum.Add(data.name, data);
        }
        return ListAsEnum;
    }
}
