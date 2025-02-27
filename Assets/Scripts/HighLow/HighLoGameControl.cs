using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLoGameControl : GameControl
{
    public void StrartBB()
    {
        this.CurrentStatusText.text = "";
    }
    public override void EndRound()
    {
        base.EndRound();
        if (this.Player.HandCardValues[0] > this.Dealer.HandCardValues[0])
        {
            StartCoroutine(WaitToShowStatus("You Win", 1));
            StaticVar.Money += this.Bet;
            //this.CurrentStatusText.text = "You Win!";
        }
        else if (this.Player.HandCardValues[0] == this.Dealer.HandCardValues[0])
        {        
            ///this.CurrentStatusText.text = "You Tie!";
            StartCoroutine(WaitToShowStatus("You Tie", 1));
        }
        else if(this.Player.HandCardValues[0] < this.Dealer.HandCardValues[0])
        {
            StartCoroutine(WaitToShowStatus("You Lose", 1));
            StaticVar.Money -= this.Bet;
            
            ///this.CurrentStatusText.text = "You Lose!";
        }
    }
    private IEnumerator WaitToShowStatus(string stat,float delay)
    {
        yield return new WaitForSeconds(delay);
        this.CurrentStatusText.text = stat;
    }
}
    