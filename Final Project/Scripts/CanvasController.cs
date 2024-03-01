using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class CanvasController : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public GameObject[] PlayerCards;
    public Image[] health;
    public Image[] mana;
    public Image healthImg, manaImg, chargeUp;
    public GameObject PauseMenu;
    public GameObject crystalTower;
    GameObject currentPlayer;

    private void Start()
    {
        chargeUp.fillAmount = 0f;

        for (int i = 0; i < PhotonNetwork.PlayerList.Length - 1; i++)
        {
            PlayerCards[i].SetActive(true);
        }
    }

    private void Update()
    {
        int i = 0;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (!player.GetComponent<FirstPersonMovement>().enabled)
            {
                if (player.GetComponent<PlayerStats>().currentHealth > 0)
                {
                    health[i].fillAmount = player.GetComponent<PlayerStats>().currentHealth / 100f;
                    mana[i].fillAmount = player.GetComponent<PlayerStats>().currentMana / 50f;
                }
                else
                {
                    player.GetComponent<MeshRenderer>().enabled = false;
                    PlayerCards[i].GetComponent<CanvasGroup>().alpha = 0.5f;
                }

                i++;
            }
            else
            {
                currentPlayer = player;
                healthImg.fillAmount = player.GetComponent<PlayerStats>().currentHealth / 100f;
                manaImg.fillAmount = player.GetComponent<PlayerStats>().currentMana / 50f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!PauseMenu.activeInHierarchy)
            {
                currentPlayer.GetComponent<Shooting>().enabled = false;

                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;

                PauseMenu.SetActive(true);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                PauseMenu.SetActive(false);

                currentPlayer.GetComponent<Shooting>().enabled = true;
            }
        }

        chargeUp.fillAmount = crystalTower.GetComponent<PortalActivation>().PortalCharge / 100f;
    }
    public const byte allLeaveRoom = 9;

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseMenu.SetActive(false);

        currentPlayer.GetComponent<Shooting>().enabled = true;
    }

    public void LeaveGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(allLeaveRoom, true, raiseEventOptions, SendOptions.SendReliable);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            PhotonNetwork.LeaveRoom();
        }
    }

    private new void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private new void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == allLeaveRoom)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
