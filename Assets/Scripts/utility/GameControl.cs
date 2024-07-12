using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;



public class GameControl : MonoBehaviour
{
    public PlayerScript Player;
    public PlayerScript Dealer;
    public GameObject Deck;
    public GameObject[] PlayerCard;
    public GameObject[] DealerCard;

    public Button StartButton;
    public Button AutoplayButton;
    public Button Bet50Button;
    public Button Bet100Button;
    public Button Bet500Button;
    public Button RestBetButton;

    public Text AutoplayStatusText;
    public Text CurrentMoneyText;
    public Text CurrentBetText;
    public Text CurrentStatusText;

    public HandRank Rank;

    public bool AutoplayFlag = false;
    
    public int Bet = 0;

    public virtual void StartGame()
    { 
        if (this.Bet == 0)
        {
            this.CurrentStatusText.text = "No Bet!";
            return;
        }
        if(StaticVar.Money == 0)
        {
            this.CurrentStatusText.text = "Not enough money!";
            return;
        }
        if (StaticVar.Money < this.Bet)
        {
            this.CurrentStatusText.text = "Not enough money!";
            return;
        }
        this.CurrentStatusText.enabled = false;
        this.Player.ResetHand();
        this.Dealer.ResetHand();
        this.Deck.GetComponent<DeckScript>().shuffle();
        this.Deck.GetComponent<DeckScript>().currentIndex = 0;
        StartCoroutine(DealCardAnimation());
        this.Player.Startinghand();
        this.Dealer.Startinghand();
        if(SceneManager.GetActiveScene().name == "7CardPoker" || SceneManager.GetActiveScene().name == "Blackjack")
        {
            if(AutoplayFlag)
            {
                EndRoundMoves();
            }
        }
        else
            EndRoundMoves();
    }
    private void EndRoundMoves()
    {
        EndRound();
        EndRoundAutoPlayCheck();
    }
    private void EndRoundAutoPlayCheck()
    {
        if (this.AutoplayFlag)
            StartCoroutine(Autoplay());                      
    }

    public virtual void EndRound()
    {
        this.CurrentStatusText.enabled = true;
        for (int i = 0; i < DealerCard.Length; i++)
            DealerCard[i].GetComponent<Renderer>().enabled = true;
    }

    public void AutoplayClicked()
    {
        if (this.AutoplayFlag)
        {
            this.AutoplayFlag = false;
            this.AutoplayStatusText.text = "Autoplay: Off";
            this.CurrentStatusText.enabled = false;
            EnableButtons(true);
        }
        else
        {
            if (this.Bet == 0)
            {
                this.CurrentStatusText.text = "No Bet!";
                return;
            }
            if (StaticVar.Money == 0)
            {
                this.CurrentStatusText.text = "Not enough money!";
                return;
            }
            if (StaticVar.Money < this.Bet)
            {
                this.CurrentStatusText.text = "Not enough money!";
                return;
            }
            this.AutoplayFlag = true;
            this.AutoplayStatusText.text = "Autoplay: On";
            StartGame();  
            EnableButtons(false);     
        }
    }

     void Start()
     {
        this.CurrentMoneyText.text = "Money: " + StaticVar.Money;
        this.CurrentBetText.text = "Bet: " + this.Bet.ToString();       
        this.StartButton.onClick.AddListener(() => StartGame());
        this.AutoplayButton.onClick.AddListener(() => AutoplayClicked());
        this.Bet50Button.onClick.AddListener(() => BetButtonPressed());
        this.Bet100Button.onClick.AddListener(() => BetButtonPressed());
        this.Bet500Button.onClick.AddListener(() => BetButtonPressed());
        this.RestBetButton.onClick.AddListener(() => ResetBetPressed());
        Rank = new HandRank();
     }
   
    public void ResetBetPressed()
    {
        this.CurrentMoneyText.text = "Money: " + StaticVar.Money;
        this.Bet = 0;
        this.CurrentBetText.text = "Bet: " + this.Bet.ToString();
    }

    public void BetButtonPressed()
    {
        this.CurrentStatusText.enabled = true;
        if (StaticVar.Money - this.Bet <= 0)
        {
            this.CurrentStatusText.text = "Not enough money!";
            return;
        }           
        int amount = int.Parse(EventSystem.current.currentSelectedGameObject.name.ToString());
        if (amount > StaticVar.Money - this.Bet)
        {
            this.CurrentStatusText.text = "Not enough money!";
            return;
        }
        else
        {
            this.Bet += amount;
            this.CurrentMoneyText.text = this.CurrentMoneyText.text = "Money: " + StaticVar.Money;
            this.CurrentBetText.text = "Bet: " + this.Bet.ToString();          
        }

    }

    public IEnumerator DealCardAnimation()
    {
        iTween.MoveTo(this.Deck, new Vector3(0, -3, 0), 1.0f);       
        yield return new WaitForSeconds(1);
        this.Deck.GetComponent<Renderer>().enabled = false;
        iTween.MoveTo(Deck, new Vector3(-3, 1, 0), 0f);
        this.Deck.GetComponent<Renderer>().enabled = true;
        for (int i = 0; i < PlayerCard.Length; i++)
            PlayerCard[i].GetComponent<Renderer>().enabled = true;                 
        this.CurrentMoneyText.text = this.CurrentMoneyText.text = "Money: " + StaticVar.Money;       
    }
    private IEnumerator Autoplay()
    {   
        yield return new WaitForSeconds(1.5f);
        StartGame();
    }

    public void EnableButtons(bool status)
    {
        this.Bet50Button.interactable = status;
        this.Bet100Button.interactable = status;
        this.Bet500Button.interactable = status;
        this.StartButton.interactable = status;
        this.RestBetButton.interactable = status;
    }
}
  

