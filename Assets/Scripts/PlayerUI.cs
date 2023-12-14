using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private TextMeshProUGUI pollenCounterText;

    public int pollenCount
    { 
        get => _pollenCount;
        set  
        {
        _pollenCount = value;
        pollenCounterText.SetText(_pollenCount.ToString());
        }
    }
    private int _pollenCount = 0;


    public void SetHealth(int health)
    {
        if (health > hearts.Length) return;

        for (int i = 0; i < hearts.Length; i++) 
        {
            hearts[i].SetActive(health > i);
        }

    }
}
