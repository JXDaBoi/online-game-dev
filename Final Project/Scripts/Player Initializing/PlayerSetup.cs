using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPun
{
    [SerializeField]
    GameObject cam;

    [SerializeField]
    TextMeshProUGUI playerNameText;

    [SerializeField]
    GameObject[] PlayerSpawns;

    [SerializeField]
    GameObject[] PlayerColours;

    private void Awake()
    {
        PlayerSpawns = GameObject.FindGameObjectsWithTag("playerSpawn");
    }

    void Start()
    {
        if (photonView.IsMine)
        {
            transform.GetComponent<FirstPersonMovement>().enabled = true;
            cam.GetComponent<Camera>().enabled = true;
            gameObject.GetComponentInChildren<AudioListener>().enabled = true;
            GetComponent<Shooting>().enabled = true;
            GetComponent<Jump>().enabled = true;

            int playerIndex = 0;

            foreach (var player in PhotonNetwork.PlayerList)
            {
                transform.position = PlayerSpawns[playerIndex % 2].transform.position;

                if (player.NickName == photonView.Owner.NickName)
                {
                    PlayerColours[playerIndex].SetActive(true);
                }

                playerIndex++;
            }
        }
        else
        {
            transform.GetComponent<FirstPersonMovement>().enabled = false;
            cam.GetComponent<Camera>().enabled = false;
            gameObject.GetComponentInChildren<AudioListener>().enabled = false;
            GetComponent<Shooting>().enabled = false;
            GetComponent<Jump>().enabled = false;

            int playerIndex = 0;

            foreach (var player in PhotonNetwork.PlayerList)
            {
                transform.position = PlayerSpawns[playerIndex % 2].transform.position;

                if (player.NickName == photonView.Owner.NickName)
                {
                    PlayerColours[playerIndex].SetActive(true);
                }

                playerIndex++;
            }
        }

        SetPlayerUI();
    }

    void SetPlayerUI()
    {
        if (playerNameText != null)
        {
            playerNameText.text = photonView.Owner.NickName;
        }
    }
}
