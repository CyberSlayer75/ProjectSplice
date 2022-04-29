using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    //public
    public int Health() => m_PlayerHealth;
    public int Energy() => m_PlayerEnergy;
    public List<object> m_Statuses;
    
    //private
    public int m_PlayerHealth;
    public int m_PlayerEnergy;
    private PlayerController playerRef;

    public PlayerStats(PlayerController pc)
    {
        m_Statuses = new List<object>();
        m_PlayerHealth = 50;
        m_PlayerEnergy = 5;
        playerRef = pc;
    }

    public int ReturnStatus<T>(T statType)
    {
        for (int i = 0; i < m_Statuses.Count; i++)
        {
            if (m_Statuses[i] is T)
                return (m_Statuses[i] as Status).m_Count;
        }
        return 0;
    }
    public void TakeDamage(int amount)
    {
        m_PlayerHealth -= amount;
        if (m_PlayerHealth <= 0)
            BattleManager.Instance.RemovePlayer(playerRef);
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
    public void GainStatus(string statType, int amountToGain)
    {
        //Not a huge fan of creating this kind of stuff every time we check for a status, but I don't know how to avoid it
        object obj = System.Activator.CreateInstance(System.Type.GetType(statType));
        if (obj is Status)
        {
            for (int i = 0; i < m_Statuses.Count; i++) //Add more to the status if we already have it
            {
                if (m_Statuses[i].GetType() == obj.GetType())
                {
                    (m_Statuses[i] as Status).m_Count += amountToGain;
                    return;
                }
            }

            //If we don't have it then add it
            m_Statuses.Add(obj);
        }
        else
        {
            Debug.LogError("Trying to gain something that isn't a status");
        }
    }
}
