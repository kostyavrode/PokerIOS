using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    private int value = 0;
    private int suit;
    public int GetCardValue()
    {
        return this.value;
    }

    public void SetCardValue(int NewValue)
    {
        this.value = NewValue;
    }

    public string GetCardSuit()
    {
        if (this.suit == 0)
            return "club";
        else if (this.suit == 1)
            return "diamond";
        else if (this.suit == 2)
            return "heart";
        else
            return "spade";
    }

    public void SetCardSuit(int NewSuit)
    {
        this.suit = NewSuit;
    }

    public void SetCardStripe(Sprite newSprite)
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
    }

    public void ResetCard()
    {
        Sprite reset = GameObject.Find("deck").GetComponent<DeckScript>().TurnToCardBack();
        this.gameObject.GetComponent<SpriteRenderer>().sprite = reset;
        this.value = 0;
    }
}
