using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class MainMenuControlScript : MonoBehaviour
{
    public TMP_Text CurrentMoneyText;
    public void changeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);      
    }

    public void Add1000Money()
    {
        StaticVar.Money += 1000;
        CurrentMoneyText.text = StaticVar.Money.ToString();
    }

    private void Start()
    {
        if (CurrentMoneyText != null) 
        CurrentMoneyText.text =  StaticVar.Money.ToString();
    }
}

