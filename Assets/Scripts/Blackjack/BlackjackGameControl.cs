using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BlackjackGameControl : GameControl
{
    public Button HitButton;
    public Button StandButton;

    public Text AutoplaySetPointText;

    public int PlayerHandCount = 2;
    public int DealerHandCount = 2;

    public Slider AutoplayValueSlide;

    void Start()
    {
        this.AutoplaySetPointText.text = "Point: " + AutoplayValueSlide.value;
        this.CurrentMoneyText.text = "Money: " + StaticVar.Money;

        this.HitButton.onClick.AddListener(() => HitPressed());
        this.StandButton.onClick.AddListener(() => StandPressed());
        this.StartButton.onClick.AddListener(() => StartGame());
        this.AutoplayButton.onClick.AddListener(() => AutoplayClicked());
        this.Bet50Button.onClick.AddListener(() => BetButtonPressed());
        this.Bet100Button.onClick.AddListener(() => BetButtonPressed());
        this.Bet500Button.onClick.AddListener(() => BetButtonPressed());
        this.RestBetButton.onClick.AddListener(() => ResetBetPressed());
        this.AutoplayValueSlide.onValueChanged.AddListener(delegate { ChangeAutoplayValueText(); });
    }

    private void ChangeAutoplayValueText()
    {
        this.AutoplaySetPointText.text = "Point: " + this.AutoplayValueSlide.value;
    }
 
    private void StandPressed()
    {
        this.Dealer.HandCard[1].GetComponent<SpriteRenderer>().enabled = true;
        if (this.Dealer.HandCardValues.Sum() > this.Player.HandCardValues.Sum())
            EndRound();
        else
            HitDealer();
    }

    private void HitDealer()
    {
        while(this.Dealer.HandCardValues.Sum() <= this.Player.HandCardValues.Sum())
        {
            this.Dealer.GetCard();
            this.Dealer.HandCard[this.DealerHandCount].GetComponent<SpriteRenderer>().enabled = true;
            this.DealerHandCount++;
            if (this.Dealer.HandCardValues.Sum() > this.Player.HandCardValues.Sum())
                EndRound();
        }
    }

    private void PlayerAutoplay()
    {
        if (this.Player.HandCardValues.Sum() >= this.AutoplayValueSlide.value)
        {
            while (this.Dealer.HandCardValues.Sum() <= this.Player.HandCardValues.Sum())
            {
                this.Dealer.GetCard();
                this.Dealer.HandCard[this.DealerHandCount].GetComponent<SpriteRenderer>().enabled = true;
                this.DealerHandCount++;
                if (this.Dealer.HandCardValues.Sum() > this.Player.HandCardValues.Sum())
                    return;
            }
        }

        while (this.Player.HandCardValues.Sum() < this.AutoplayValueSlide.value)
        {
            this.Player.GetCard();
            this.Player.HandCard[this.PlayerHandCount].GetComponent<SpriteRenderer>().enabled = true;
            this.PlayerHandCount++;
            if (Player.HandCardValues.Sum() >= AutoplayValueSlide.value && Player.HandCardValues.Sum() < 20)
            {
                while (Dealer.HandCardValues.Sum() <= Player.HandCardValues.Sum())
                {
                    Dealer.GetCard();
                    Dealer.HandCard[DealerHandCount].GetComponent<SpriteRenderer>().enabled = true;
                    DealerHandCount++;
                    if (Dealer.HandCardValues.Sum() > Player.HandCardValues.Sum())
                        return;
                }
            }
            else if(Player.HandCardValues.Sum() > 20)
                return;                           
        }
    }

    private void HitPressed()
    {             
        Player.GetCard();
        Player.HandCard[PlayerHandCount].GetComponent<SpriteRenderer>().enabled = true;
        PlayerHandCount++;
        if (Player.HandCardValues.Sum() > 20)
            EndRound();
    }

    public override void EndRound()
    {
        if (AutoplayFlag)
        {
            PlayerAutoplay();
            this.HitButton.interactable = false;
            this.StandButton.interactable = false;
        }
        Dealer.HandCard[1].GetComponent<SpriteRenderer>().enabled = true;
        this.CurrentStatusText.enabled = true;

        bool PlayerBust = Player.HandCardValues.Sum() > 21;
        bool DealerBust = Dealer.HandCardValues.Sum() > 21;

        if(PlayerBust || (!DealerBust && Dealer.HandCardValues.Sum() > Player.HandCardValues.Sum()))
        {
            this.CurrentStatusText.text = "You Lose!";
            StaticVar.Money -= this.Bet;
        }
        else if(DealerBust || (!PlayerBust && Player.HandCardValues.Sum() > Dealer.HandCardValues.Sum()))
        {
            this.CurrentStatusText.text = "You Win!";
            StaticVar.Money += this.Bet;
        }
        else if(Player.HandCardValues.Sum() == Dealer.HandCardValues.Sum())
            this.CurrentStatusText.text = "You Tie!";     
        
        this.CurrentMoneyText.text = "Money: " + StaticVar.Money;     
    }

    public override void StartGame()
    {
        PlayerHandCount = 2;
        DealerHandCount = 2;
        this.HitButton.interactable = true;
        this.StandButton.interactable = true;
        base.StartGame();
        Dealer.HandCard[0].GetComponent<SpriteRenderer>().enabled = true;
    }
}
