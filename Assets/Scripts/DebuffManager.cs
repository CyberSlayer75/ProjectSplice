using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffManager : MonoBehaviour
{
    public static DebuffManager Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
    }
    public void ApplyAllDebuffEffects(PlayerController player)
    {
        for (int i = 0; i < player.Stats().m_Statuses.Count; i++)
        {
            if ((player.Stats().m_Statuses[i] as Status).m_StatusType == Status.StatusType.Debuff)
            {

            }
        }
    }
}
