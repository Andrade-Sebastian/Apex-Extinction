using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCounter : MonoBehaviour
{
    public Text killCounter;
    private int killCount = 0;

    void Start()
    {
        UpdateKillCountText();
    }

    private void Update()
    {
        killCounter.text = "Kills: " + killCount.ToString();
    }
    public void IncrementKillCount()
    {
        killCount++;
        UpdateKillCountText();
    }

    void UpdateKillCountText()
    {
        //killCounter.text = "Kills: " + killCount.ToString();
    }
}