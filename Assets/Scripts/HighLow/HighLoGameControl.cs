using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLoGameControl : GameControl
{

    public override void EndRound()
    {
        base.EndRound();
        if (this.Player.HandCardValues[0] > this.Dealer.HandCardValues[0])
        {
            StaticVar.Money += this.Bet;
            this.CurrentStatusText.text = "You Win!";
        }
        else if (this.Player.HandCardValues[0] == this.Dealer.HandCardValues[0])
        {        
            this.CurrentStatusText.text = "You Tie!";
        }
        else if(this.Player.HandCardValues[0] < this.Dealer.HandCardValues[0])
        {
            StaticVar.Money -= this.Bet;
            this.CurrentStatusText.text = "You Lose!";
        }
    }
}
    
