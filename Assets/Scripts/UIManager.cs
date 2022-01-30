using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Singleton
    public static UIManager Instance = null;
    //public
    [Header("General")]
    public RectTransform playZone;
    [Header("Player Zone")]
    public GameObject handContainer;
    public GameObject deckContainer;
    public GameObject graveContainer;
    public GameObject cardPrefab;
    public GameObject energyContainer;
    public TextMeshProUGUI conclusionScreen;

    [HideInInspector]
    public Canvas canvas;
    [Header("Enemy Zone")]
    public GameObject EnemyZone;
    public GameObject enemyCardPrefab;
    //Private
    List<GameObject> CardsInHandObjs = new List<GameObject>();
    TextMeshProUGUI deckCount;
    TextMeshProUGUI graveCount;
    TextMeshProUGUI energyCount;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            deckCount = deckContainer.GetComponentInChildren<TextMeshProUGUI>();
            graveCount = graveContainer.GetComponentInChildren<TextMeshProUGUI>();
            energyCount = energyContainer.GetComponentInChildren<TextMeshProUGUI>();
            canvas = GetComponent<Canvas>();
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.mousePosition);
        UpdateDisplays();
    }

    public void UpdateDisplays()
    {
        if (BattleManager.Instance.GetPlayerController() != null)
        {
            deckCount.text = BattleManager.Instance.GetPlayerController().Deck().CardsInDeck().ToString();
            graveCount.text = BattleManager.Instance.GetPlayerController().Grave().CardsInDeck().ToString();
            energyCount.text = BattleManager.Instance.GetPlayerController().Stats().Energy().ToString();
        }
    }

    public bool isCardInPlayZone(Vector2 pos)
    {
        return playZone.rect.Contains(pos);
    }
    
    public void CreateCardOnDeckAndSendToHand(Deck deckToAddTo, PlayerController.DeckType deckToDrawFrom)
    {
        //Since we are always adding the lastest to the deck, the index will always be the deck size
        switch(deckToDrawFrom)
        {
            case PlayerController.DeckType.PlayerDeck:
                deckToAddTo.m_CardsInDeck[deckToAddTo.CardsInDeck() -1].cardObj = (Instantiate(cardPrefab, deckContainer.transform.position, Quaternion.identity, this.transform));
                deckToAddTo.m_CardsInDeck[deckToAddTo.CardsInDeck() - 1].cardObj.name = deckToAddTo.m_CardsInDeck[deckToAddTo.CardsInDeck() - 1].card.Name() + Time.timeSinceLevelLoad;
                break;
            case PlayerController.DeckType.PlayerGrave:
                deckToAddTo.m_CardsInDeck[deckToAddTo.CardsInDeck() -1].cardObj = (Instantiate(cardPrefab, graveContainer.transform.position, Quaternion.identity, this.transform));
                deckToAddTo.m_CardsInDeck[deckToAddTo.CardsInDeck() - 1].cardObj.name = deckToAddTo.m_CardsInDeck[deckToAddTo.CardsInDeck() - 1].card.Name() + Time.timeSinceLevelLoad;
                break;
        }
        //populate card
        deckToAddTo.m_CardsInDeck[deckToAddTo.CardsInDeck() - 1].cardObj.GetComponent<CardDisplay>().SetCard(deckToAddTo.m_CardsInDeck[deckToAddTo.CardsInDeck() - 1].card);
        SendCardToHand(deckToAddTo.m_CardsInDeck[deckToAddTo.CardsInDeck() - 1].cardObj);
    }
    /// <summary>
    /// Similar to CreateCardOnDeckAndSendToHand, except we make an invisible card the the enemy has behind the scenes
    /// </summary>
    /// <param name="deckToAddTo"></param>
    /// <param name="deckToDrawFrom"></param>
    public void CreateCardFromDeckAndSendToHandEnemy(Deck deckToAddTo)
    {
        //Since we are always adding the lastest to the deck, the index will always be the deck size
        deckToAddTo.m_CardsInDeck[deckToAddTo.CardsInDeck() - 1].cardObj = (Instantiate(enemyCardPrefab, EnemyZone.transform.position, Quaternion.identity, EnemyZone.transform));
        deckToAddTo.m_CardsInDeck[deckToAddTo.CardsInDeck() - 1].cardObj.name = deckToAddTo.m_CardsInDeck[deckToAddTo.CardsInDeck() - 1].card.Name() + Time.timeSinceLevelLoad;
        //populate card
        deckToAddTo.m_CardsInDeck[deckToAddTo.CardsInDeck() - 1].cardObj.GetComponent<CardDisplay>().SetCard(deckToAddTo.m_CardsInDeck[deckToAddTo.CardsInDeck() - 1].card);
    }

    public void SendCardToGrave(GameObject card)
    {
        StartCoroutine(MoveCardToGrave(card, 1f));
    }

    public void SendCardToHand(GameObject card)
    {
        StartCoroutine(MoveCardToHand(card, .5f)); //Send the latest card we added to the hand list to the hand
    }

    IEnumerator MoveCardToHand(GameObject card, float time)
    {
        float elapsed = 0;
        Vector3 startPos = card.transform.position;
        while(elapsed < time)
        {
            elapsed += Time.deltaTime;
            card.transform.position = Vector3.Lerp(startPos, handContainer.transform.position, elapsed / time);
            yield return null;
        }
        card.transform.SetParent(handContainer.transform);
        yield return null;
    }

    IEnumerator MoveCardToGrave(GameObject card, float time)
    {
        float elapsed = 0;
        Vector3 startPos = card.transform.position;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            card.transform.position = Vector3.Lerp(startPos, graveContainer.transform.position, elapsed / time);
            yield return null;
        }
        DestroyImmediate(card);
        yield return null;
    }

    public void EndTurnBtn()
    {
        if (BattleManager.Instance.currentPlayerTurn.PlayerControlled)
        {
            BattleManager.Instance.currentPlayerTurn.DiscardHand();
            BattleManager.Instance.EndPlayerTurn();
        }
    }

    public void DisplayConclusion(bool playerWon)
    {
        if (playerWon)
            conclusionScreen.text = "YOU WON!";
        else
            conclusionScreen.text = "YOU LOST!";
        conclusionScreen.enabled = true;
    }

}
