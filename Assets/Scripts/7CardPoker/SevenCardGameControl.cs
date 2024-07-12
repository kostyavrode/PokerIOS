using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SevenCardGameControl : GameControl
{
    public GameObject[] SelectedCards;
    public SevenCardPlayer SevenPlayer;
    public Button ConfirmButton;

    private string PlayerScore;

    private List<List<int>> AllPossibleHand = new List<List<int>>
    {
        new List<int>{0,1,2,3,4},
        new List<int>{0,1,2,3,5},
        new List<int>{0,1,2,3,6},
        new List<int>{0,1,2,4,5},
        new List<int>{0,1,2,4,6},
        new List<int>{0,1,2,5,6},
        new List<int>{0,1,3,4,5},
        new List<int>{0,1,3,4,6},
        new List<int>{0,1,3,5,6},
        new List<int>{0,1,4,5,6},
        new List<int>{0,2,3,4,5},
        new List<int>{0,2,3,4,6},
        new List<int>{0,2,3,5,6},
        new List<int>{0,2,4,5,6},
        new List<int>{0,3,4,5,6},
        new List<int>{1,2,3,4,5},
        new List<int>{1,2,3,4,6},
        new List<int>{1,2,4,5,6},
        new List<int>{1,2,4,5,6},
        new List<int>{1,3,4,5,6},
        new List<int>{2,3,4,5,6}
    };
  
    private string GetAutoCompareHandScore(PlayerScript CurrentPlayer)
    {
        List<int> TempValues = new List<int>();
        List<string> TempSuits = new List<string>();
        List<string> AllPossibleScores = new List<string>();

        for(int i = 0; i< this.AllPossibleHand.Count; i++)
        {
            TempValues.Clear();
            TempSuits.Clear();
            for (int j = 0; j < 5; j++)
            {
                TempValues.Add(CurrentPlayer.HandCardValues[AllPossibleHand[i][j]]);
                TempSuits.Add(CurrentPlayer.HandCardSuits[AllPossibleHand[i][j]]);
            }
            AllPossibleScores.Add(Rank.CurrentHandRankScore(TempValues, TempSuits));
        }
        string MaxScore = AllPossibleScores[0];
        for(int i = 1; i < 21; i++)
        {
            if (this.Rank.CompareRank(MaxScore, AllPossibleScores[i]) == 1)
                continue;
            else if (this.Rank.CompareRank(MaxScore, AllPossibleScores[i]) == 0)
                continue;
            else if (this.Rank.CompareRank(MaxScore, AllPossibleScores[i]) == -1)
                MaxScore = AllPossibleScores[i];
        }
        return MaxScore;
    }

    public override void EndRound()
    {       
        base.EndRound();
        if (AutoplayFlag)
        {
            EnableButtons(false);
            this.ConfirmButton.interactable = false;
            if (Rank.CompareRank(GetAutoCompareHandScore(Player), GetAutoCompareHandScore(Dealer)) == 1)
            {
                StaticVar.Money += this.Bet;
                this.CurrentStatusText.text = "You Win!";
            }
            else if (Rank.CompareRank(GetAutoCompareHandScore(Player), GetAutoCompareHandScore(Dealer)) == 0)
            {
                this.CurrentStatusText.text = "You Tie!";
            }
            else if (Rank.CompareRank(GetAutoCompareHandScore(Player), GetAutoCompareHandScore(Dealer)) == -1)
            {
                StaticVar.Money -= this.Bet;
                this.CurrentStatusText.text = "You Lose!";
            }
        }
        else
        {       
            if (Rank.CompareRank(PlayerScore, GetAutoCompareHandScore(Dealer)) == 1)
            {
                StaticVar.Money += this.Bet;
                this.CurrentStatusText.text = "You Win!";
            }
            else if (Rank.CompareRank(PlayerScore, GetAutoCompareHandScore(Dealer)) == 0)
            {
                this.CurrentStatusText.text = "You Tie!";
            }
            else if (Rank.CompareRank(PlayerScore, GetAutoCompareHandScore(Dealer)) == -1)
            {
                StaticVar.Money -= this.Bet;
                this.CurrentStatusText.text = "You Lose!";
            }
            
            EnableButtons(true);
        }
        this.CurrentMoneyText.text = "Money: " + StaticVar.Money;
        SevenPlayer.PlayerSelectValue.Clear();
        SevenPlayer.PlayerSelectSuit.Clear();
        foreach (var item in SevenPlayer.HandCard)
            item.GetComponent<SpriteRenderer>().color = Color.white;
        this.ConfirmButton.interactable = true;
    }

    public override void StartGame()
    {
        EnableButtons(false);
        base.StartGame();
    }

    private void Start()
    {
        this.CurrentMoneyText.text = "Money: " + StaticVar.Money;
        ConfirmButton.onClick.AddListener(() => ConfirmButtonClicked());
        this.StartButton.onClick.AddListener(() => StartGame());
        this.AutoplayButton.onClick.AddListener(() => AutoplayClicked());
        this.Bet50Button.onClick.AddListener(() => BetButtonPressed());
        this.Bet100Button.onClick.AddListener(() => BetButtonPressed());
        this.Bet500Button.onClick.AddListener(() => BetButtonPressed());
        this.RestBetButton.onClick.AddListener(() => ResetBetPressed());
        Rank = new HandRank();
    }

    private void ConfirmButtonClicked()
    {
        if (SevenPlayer.PlayerSelectValue.Count != 5)     
            return;
        
        PlayerScore = Rank.CurrentHandRankScore(SevenPlayer.PlayerSelectValue, SevenPlayer.PlayerSelectSuit);
        EndRound();
    }
}
