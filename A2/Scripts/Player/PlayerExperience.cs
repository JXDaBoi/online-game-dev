using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerExperience : MonoBehaviourPun
{
    public static PlayerExperience instance;

    public float experience = 0, expRequiredForNextLevel;
    public int availableUpgrade = 0;

    public int playerLevel = 1;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        expRequiredForNextLevel = Mathf.Pow(2, (playerLevel - 1) / 5) * 16 * playerLevel;
    }

    private void Update()
    {

        if (experience >= expRequiredForNextLevel)
        {
            experience -= expRequiredForNextLevel;
            playerLevel++;
            availableUpgrade++;
            expRequiredForNextLevel = Mathf.Pow(2, (playerLevel - 1) / 5) * 16 * playerLevel;
        }
    }
}
