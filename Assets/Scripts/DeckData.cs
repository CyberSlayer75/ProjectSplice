using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DeckData : ScriptableObject
{
    public List<CardContainer> Cards;

    [System.Serializable]
    public class CardContainer
    {
        [NaughtyAttributes.Dropdown("GetCards")]
        public CardData card;

        private NaughtyAttributes.DropdownList<CardData> GetCards()
        {
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:CardData", new string[] { "Assets/CardDatabase" });
            NaughtyAttributes.DropdownList<CardData> ListAsEnum = new NaughtyAttributes.DropdownList<CardData>();
            foreach (string g in guids)
            {
                CardData data = UnityEditor.AssetDatabase.LoadAssetAtPath<CardData>(UnityEditor.AssetDatabase.GUIDToAssetPath(g));
                string p = UnityEditor.AssetDatabase.GetAssetPath(data);
                string s = UnityEditor.AssetDatabase.GetAssetPath(data).Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries)[UnityEditor.AssetDatabase.GetAssetPath(data).Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries).Length - 3] + '/' +
                    UnityEditor.AssetDatabase.GetAssetPath(data).Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries)[UnityEditor.AssetDatabase.GetAssetPath(data).Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries).Length - 2] + '/' +
                   UnityEditor.AssetDatabase.GetAssetPath(data).Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries)[UnityEditor.AssetDatabase.GetAssetPath(data).Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries).Length - 1].Remove(
                       UnityEditor.AssetDatabase.GetAssetPath(data).Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries)[UnityEditor.AssetDatabase.GetAssetPath(data).Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries).Length - 1].Length - 6, 6);
                ListAsEnum.Add(s, data);
            }
            return ListAsEnum;
        }
    }


  
}

