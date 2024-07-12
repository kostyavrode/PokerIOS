using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiveCardPlayer : PlayerScript
{
    public override void Startinghand()
    {
        GetCard();
        GetCard();      
        GetCard();     
        GetCard();     
        GetCard();     
    }

    private void Start()
    {
        AdjustMoney(PlayerPrefs.GetInt("Money"));
    }
}
