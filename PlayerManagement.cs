using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManagement : MonoBehaviour
{
    [Header("Financial Statistics")]
    public float current_balance;
    public float current_income;
    public float current_debt;
    public float revenue;
  
    [Header("Security Level")]
    public int current_security_level;
    public int experience;

    [Header("References")]
    public TMP_Text text_balance;
    public TMP_Text text_security_level;
    


    public void Update()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        text_balance.text = $"$ {FormatBalance(current_balance)}";
        text_security_level.text = current_security_level.ToString();
    }

    public string FormatBalance(float balance)
    {
        if (balance < 10000000) return balance.ToString("N0"); 
        else if (balance < 1000000000) return (balance / 1000000f).ToString("0.0#") + " million";
        else return (balance / 1000000000f).ToString("0.0#") + " billion";
    }
}
