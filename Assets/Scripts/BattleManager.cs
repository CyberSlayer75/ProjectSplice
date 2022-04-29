using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance = null;
    public List<PlayerController> activePlayers;
    public bool playerEndTurn = false;
    public PlayerController currentPlayerTurn;

    private bool playerWon = false;

    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
            StartGame();
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
    }

    public void StartGame()
    {
        StartCoroutine(PlayerTurnLoop(activePlayers[0]));
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

    public void AddNewPlayer(PlayerController pc)
    {
        activePlayers.Add(pc);
    }

    public void RemovePlayer(PlayerController pc)
    {
        activePlayers.Remove(pc);
    }

    public bool IsThisPlayersTurn(PlayerController test)
    {
        return test == currentPlayerTurn;
    }

    public void EndPlayerTurn()
    {
        playerEndTurn = true;
    }
    public bool CheckForWinState()
    {
        //Check for Player win
        bool onlyPlayer = true;
        for(int i = 0; i < activePlayers.Count; i++)
        {
            if(!activePlayers[i].PlayerControlled)
            {
                onlyPlayer = false;
            }
        }
        //Check for Enemy win
        bool onlyEnemy = true;
        for (int i = 0; i < activePlayers.Count; i++)
        {
            if (activePlayers[i].PlayerControlled)
            {
                onlyEnemy = false;
            }
        }
        if (onlyPlayer) playerWon = true;
        return (onlyEnemy || onlyPlayer);
    }

    IEnumerator PlayerTurnLoop(PlayerController playerTurn)
    {
        currentPlayerTurn = playerTurn;
        playerEndTurn = false;
        Debug.Log("It is currently " + currentPlayerTurn.PlayerName + "'s turn");
        //Let Player Draw
        currentPlayerTurn.DrawCards(3, PlayerController.DeckType.PlayerDeck, PlayerController.DeckType.PlayerHand);

        //Update Buffs
        BuffManager.Instance.ApplyAllBuffEffects(currentPlayerTurn);

        while(!playerEndTurn)
        {
            //This is where the player is playing cards and deciding actions
            if(!currentPlayerTurn.PlayerControlled)
            {
                currentPlayerTurn.RunAI();
            }
            //Check if the player has lost or won, then break out
            if(CheckForWinState())
            {
                UIManager.Instance.DisplayConclusion(playerWon);
                yield break;
            }
                
            yield return null;
        }

        //Update Debuffs
        DebuffManager.Instance.ApplyAllDebuffEffects(currentPlayerTurn);

        //Swap turn to other player
        int nextPlayer = activePlayers.IndexOf(currentPlayerTurn);
        nextPlayer = (nextPlayer + 1) >= activePlayers.Count ? 0 : (nextPlayer + 1);
        yield return PlayerTurnLoop(activePlayers[nextPlayer]);
    }
}
