using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SevenCardCardScript : CardScript
{
    public GameObject player;
    private void Start()
    {   
        player.GetComponent<SevenCardPlayer>().PlayerSelectValue = new List<int>();
        player.GetComponent<SevenCardPlayer>().PlayerSelectSuit = new List<string>();
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
    private void OnMouseDown()
    {
        Debug.Log(this.GetCardValue());
        if (this.gameObject.GetComponent<SpriteRenderer>().color == Color.red)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            player.GetComponent<SevenCardPlayer>().PlayerSelectValue.Remove(GetCardValue());
            player.GetComponent<SevenCardPlayer>().PlayerSelectSuit.Remove(GetCardSuit());
        }
        else
        {
            if (player.GetComponent<SevenCardPlayer>().PlayerSelectValue.Count == 5)         
                return;           
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            player.GetComponent<SevenCardPlayer>().PlayerSelectValue.Add(this.GetCardValue());
            player.GetComponent<SevenCardPlayer>().PlayerSelectSuit.Add(GetCardSuit());
        }
    }
}
