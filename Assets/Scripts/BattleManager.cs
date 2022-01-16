using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance = null;
    public List<PlayerController> activePlayers;
    

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
    }

    /// <summary>
    /// Returns a random player that isn't the player passed in
    /// </summary>
    /// <param name="me"></param>
    /// <returns></returns>
    public PlayerController GetRandomEnemy(PlayerController me)
    {
        PlayerController pc = activePlayers[Random.Range(0, activePlayers.Count)];
        while (pc == me)
        {
            pc = activePlayers[Random.Range(0, activePlayers.Count)];
        }
        return pc;
    }
    /// <summary>
    /// Returns all the player controllers that aren't the playerCon passed in
    /// </summary>
    /// <param name="me"></param>
    /// <returns></returns>
    public List<PlayerController> GetAllEnemies(PlayerController me)
    {
        List<PlayerController> result = new List<PlayerController>();
        for(int i = 0; i < activePlayers.Count; i++)
        {
            PlayerController pc = activePlayers[i];
            if (pc != me)
            {
                result.Add(pc);
            }
        }
        return result;
    }

    public PlayerController GetPlayerController()
    {
        foreach(PlayerController p in activePlayers)
        {
            if (p.PlayerControlled)
                return p;
        }
        return null;
    }
}
