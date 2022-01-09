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
    public GameObject handContainer;
    public GameObject deckContainer;
    public GameObject graveContainer;
    public GameObject cardPrefab;
    public RectTransform playZone;
    [HideInInspector]
    public Canvas canvas;

    //Private
    List<GameObject> CardsInHandObjs = new List<GameObject>();
    TextMeshProUGUI deckCount;
    TextMeshProUGUI graveCount;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            deckCount = deckContainer.GetComponentInChildren<TextMeshProUGUI>();
            graveCount = graveContainer.GetComponentInChildren<TextMeshProUGUI>();
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
    }

    public void UpdateDisplays(int deckNum, int graveNum, int handNum)
    {
        deckCount.text = deckNum.ToString();
        graveCount.text = graveNum.ToString();
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
                break;
            case PlayerController.DeckType.PlayerGrave:
                deckToAddTo.m_CardsInDeck[deckToAddTo.CardsInDeck() -1].cardObj = (Instantiate(cardPrefab, graveContainer.transform.position, Quaternion.identity, this.transform));
                break;
        }
        //populate card
        deckToAddTo.m_CardsInDeck[deckToAddTo.CardsInDeck() - 1].cardObj.GetComponent<CardDisplay>().SetCard(deckToAddTo.m_CardsInDeck[deckToAddTo.CardsInDeck() - 1].card);
        StartCoroutine(MoveCardToHand(deckToAddTo.m_CardsInDeck[deckToAddTo.CardsInDeck() - 1].cardObj, .5f)); //Send the latest card we added to the hand list to the hand

    }
    
    public void SendCardFromHandToGrave(GameObject card)
    {
        StartCoroutine(MoveCardToGrave(card, 1f));
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
}
