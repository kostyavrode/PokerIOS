using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BliackjackPlayer : PlayerScript
{
    public List<CardScript> aces = new List<CardScript>();

    public override void Startinghand()
    {
        GetCard();
        GetCard();
    }

    public override void GetCard()
    {
        this.handCardValueSuits = this.deck.Deal(this.HandCard[this.cardIndex].GetComponent<CardScript>());
        if (handCardValueSuits.Item1 >= 10)
            this.HandCardValues.Add(10);
        else if (handCardValueSuits.Item1 == 1)
        {          
            this.HandCardValues.Add(1);
            aces.Add(HandCard[cardIndex].GetComponent<CardScript>());
        }
        else
            HandCardValues.Add(handCardValueSuits.Item1);
        this.HandCardSuits.Add(handCardValueSuits.Item2);
        if(aces.Count>0)
            AceCheck();
        cardIndex++;
    }

    private void AceCheck()
    {
        foreach(CardScript ace in aces)
        {
            if(HandCardValues.Sum() + 10 < 22 && ace.GetCardValue() == 1)
            {
                ace.SetCardValue(11);
                HandCardValues[cardIndex] += 10;
            }
            else if(HandCardValues.Sum() > 21 && ace.GetCardValue() == 11)
            {
                ace.SetCardValue(1);
                HandCardValues[cardIndex] -= 10;
            }           
        }
    }

    public override void ResetHand()
    {
        base.ResetHand();
        aces = new List<CardScript>();
    }
}
