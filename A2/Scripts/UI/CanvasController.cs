using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CanvasController : MonoBehaviour
{
    public static CanvasController instance;

    public Image HealthBarFill;
    public Image ExperienceBarFill;

    public Text hasAvailableUpgrades, timeSurvived, levelGained, maximumHealth, roomCleared;

    public GameObject upgradeMenu;

    public GameObject DeadScreen;

    public GameObject PauseScreen;

    float timeElapsed = 0f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        HealthBarFill.rectTransform.sizeDelta = new Vector2(1300 * MapInfo.instance.currentConsciousnessHealth / MapInfo.instance.maximumConsciousnessHealth, 69);
        ExperienceBarFill.rectTransform.sizeDelta = new Vector2(50, 700 * PlayerExperience.instance.experience / PlayerExperience.instance.expRequiredForNextLevel);

        if (PlayerExperience.instance.availableUpgrade > 0)
        {
            hasAvailableUpgrades.gameObject.SetActive(true);
            hasAvailableUpgrades.text = "There is " + PlayerExperience.instance.availableUpgrade + " upgrade(s) available!";
        }
        else if (PlayerExperience.instance.availableUpgrade == 0)
        {
            hasAvailableUpgrades.gameObject.SetActive(false);
        }
    }

    public void toggleUpgradeMenu()
    {
        if (!upgradeMenu.activeInHierarchy)
        {
            upgradeMenu.SetActive(true);
        }
        else
        {
            upgradeMenu.SetActive(false);
        }

    }

    public void toggleDeadScreen()
    {
        if (upgradeMenu.activeInHierarchy)
        {
            upgradeMenu.SetActive(false);
        }

        DeadScreen.SetActive(true);

        timeSurvived.text = "Time Survived : " + (int)(timeElapsed / 60) + " min " + (int)(timeElapsed % 60) + " sec";
        levelGained.text = "Level Gained : " + (PlayerExperience.instance.playerLevel - 1);
        maximumHealth.text = "Maximum Health : " + MapInfo.instance.maximumConsciousnessHealth;
        roomCleared.text = "Room Cleared : " + MapInfo.instance.roomCleared;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void ReturnGame()
    {
        PauseScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
