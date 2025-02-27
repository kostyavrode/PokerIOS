using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerScript : MonoBehaviour
{
    public DeckScript deck;
    public CardScript card;

    public Tuple<int, string> handCardValueSuits;

    public GameObject[] HandCard;

    public int cardIndex = 0;

    public List<int> HandCardValues;
    public List<string> HandCardSuits;

    public int money;

    public virtual void Startinghand()
    {
        GetCard();
    }

    public virtual void GetCard()
    {
        this.handCardValueSuits = this.deck.Deal(this.HandCard[this.cardIndex].GetComponent<CardScript>());
        if (handCardValueSuits.Item1 == 1)
            this.HandCardValues.Add(14);
        else
            this.HandCardValues.Add(handCardValueSuits.Item1);
        this.HandCardSuits.Add(handCardValueSuits.Item2);
        cardIndex++;
    }

    public virtual void ResetHand()
    {
for(int i=0; i< this.HandCard.Length; i++)
        {
            this.HandCard[i].GetComponent<CardScript>().ResetCard();
            this.HandCard[i].GetComponent<Renderer>().enabled = false;
        }

        this.cardIndex = 0;
        this.HandCardValues.Clear();
        this.HandCardSuits.Clear();
    }

    public void AdjustMoney(int newMoney)
    {
        this.money += newMoney;
    }

    public int GetMoney()
    {
        return this.money;
    }

    void Start()
    {
        this.HandCardValues = new List<int>();
        this.HandCardSuits = new List<string>();
    }
}