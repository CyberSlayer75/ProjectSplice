using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    //Public enums
    public enum BuffStatus { None, Armor };
    public enum DebuffStatus { None, Bleed, Fracture, Stun };
    public enum UniqueStatus { None, Discard };
    //public
    public int Health() => m_PlayerHealth;
    public int Energy() => m_PlayerEnergy;
    public Dictionary<BuffStatus, int> buffStatuses;
    public Dictionary<DebuffStatus, int> debuffStatuses;
    public Dictionary<UniqueStatus, int> uniqueStatuses;
    
    //private
    public int m_PlayerHealth;
    public int m_PlayerEnergy;

    public PlayerStats()
    {
        buffStatuses = new Dictionary<BuffStatus, int>();
        debuffStatuses = new Dictionary<DebuffStatus, int>();
        uniqueStatuses = new Dictionary<UniqueStatus, int>();
        m_PlayerHealth = 50;
        m_PlayerEnergy = 5;
    }

    public int ReturnBuffStatus(BuffStatus s)
    {
        if (buffStatuses.ContainsKey(s))
            return buffStatuses[s];
        else
            return 0;
    }
    public int ReturnDebuffStatus(DebuffStatus s)
    {
        if (debuffStatuses.ContainsKey(s))
            return debuffStatuses[s];
        else
            return 0;
    }
    public int ReturnUniqueStatus(UniqueStatus s)
    {
        if (uniqueStatuses.ContainsKey(s))
            return uniqueStatuses[s];
        else
            return 0;
    }
    public void TakeDamage(int amount)
    {
        m_PlayerHealth -= amount;
    }
    public void HealDamage(int amount)
    {
        m_PlayerHealth += amount;
    }
    public void SetHealth(int amount)
    {
        m_PlayerHealth = amount;
    }
    public void GainEnergy(int amount)
    {
        m_PlayerEnergy += amount;
    }
    public void LoseEnergy(int amount)
    {
        m_PlayerEnergy -= amount;
    }
    public void SetEnergy(int amount)
    {
        m_PlayerEnergy = amount;
    }
    public void GainBuff(BuffStatus buff, int amountToGain)
    {
        int check;
        if (buffStatuses.TryGetValue(buff, out check))
        {
            buffStatuses[buff] += amountToGain;
        }
        else
        {
            buffStatuses.Add(buff, amountToGain);
        }
    }
    public void GainDebuff(DebuffStatus debuff, int amountToGain)
    {
        int check;
        if (debuffStatuses.TryGetValue(debuff, out check))
        {
            debuffStatuses[debuff] += amountToGain;
        }
        else
        {
            debuffStatuses.Add(debuff, amountToGain);
        }
    }
    public void GainUnique(UniqueStatus unique, int amountToGain)
    {
        int check;
        if (uniqueStatuses.TryGetValue(unique, out check))
        {
            uniqueStatuses[unique] += amountToGain;
        }
        else
        {
            uniqueStatuses.Add(unique, amountToGain);
        }
    }
    public void LoseBuff(BuffStatus buff, int amountToLose)
    {
        int count;
        if(buffStatuses.TryGetValue(buff, out count))
        {
            buffStatuses[buff] = Mathf.Min(0, count - amountToLose);
        }
    }
    public void LoseDebuff(DebuffStatus debuff, int amountToLose)
    {
        int count;
        if (debuffStatuses.TryGetValue(debuff, out count))
        {
            debuffStatuses[debuff] = Mathf.Min(0, count - amountToLose);
        }
    }
    public void LoseUnique(UniqueStatus unique, int amountToLose)
    {
        int count;
        if (uniqueStatuses.TryGetValue(unique, out count))
        {
            uniqueStatuses[unique] = Mathf.Min(0, count - amountToLose);
        }
    }
}
