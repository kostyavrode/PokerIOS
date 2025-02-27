using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiveCardGameControl : GameControl
{
    public GameObject winSprite;
    public GameObject loseSprite;
    public void StartBB()
    {
        loseSprite.gameObject.SetActive(false);
        winSprite.gameObject.SetActive(false);
    }
    public override void EndRound()
    {
        base.EndRound();
        this.Rank = new HandRank();
        if(this.Rank.CompareRank(this.Rank.CurrentHandRankScore(this.Player.HandCardValues, this.Player.HandCardSuits)
                                ,this.Rank.CurrentHandRankScore(this.Dealer.HandCardValues, this.Dealer.HandCardSuits)) == 1)
        {
            StaticVar.Money += this.Bet;
            //this.CurrentStatusText.text = "You Win!";
            StartCoroutine(WaitToDisableSprites(true));
        }
        else if(this.Rank.CompareRank(this.Rank.CurrentHandRankScore(this.Player.HandCardValues, this.Player.HandCardSuits)
                                     ,this.Rank.CurrentHandRankScore(this.Dealer.HandCardValues, this.Dealer.HandCardSuits)) == 0)       
            this.CurrentStatusText.text = "You Tie!";
        
        else if (this.Rank.CompareRank(this.Rank.CurrentHandRankScore(this.Player.HandCardValues, this.Player.HandCardSuits)
                                      ,this.Rank.CurrentHandRankScore(this.Dealer.HandCardValues, this.Dealer.HandCardSuits)) == -1)
        {
            StartCoroutine(WaitToDisableSprites(false));
            //loseSprite.SetActive(true);
            StaticVar.Money -= this.Bet;
           // this.CurrentStatusText.text = "You Lose!";
        }
        
    }
    private IEnumerator WaitToDisableSprites(bool win)
    { 
        yield return new WaitForSeconds(1);
        if (win)
        {
            winSprite.gameObject.SetActive(true);
        }
        else
        {
            loseSprite.gameObject.SetActive(true);
        }
        
        
    }
}