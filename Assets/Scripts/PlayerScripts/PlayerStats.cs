using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    [SerializeField]
    private Image health_Stats, stamina_Stats;

    public void DisplayHealthStats(float healthValue, float maxValue)
    {
        health_Stats.fillAmount = healthValue / maxValue;
    }

    public void DisplayStaminaStats(float staminaValue, float maxValue)
    {
        stamina_Stats.fillAmount = staminaValue / maxValue;
    }
}
