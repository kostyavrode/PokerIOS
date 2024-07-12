using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SevenCardPlayer : PlayerScript
{
    public List<int> PlayerSelectValue;
    public List<string> PlayerSelectSuit;
    public override void Startinghand()
    {
        GetCard();
        GetCard();
        GetCard();
        GetCard();
        GetCard();
        GetCard();
        GetCard();
    }
}
