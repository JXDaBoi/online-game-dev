using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;

public class PlayerSetup : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField]
    GameObject cam;

    [SerializeField]
    TextMeshProUGUI playerNameText;

    public AudioClip sfx;

    void Start()
    {
        if (photonView.IsMine)
        {
            transform.GetComponent<FirstPersonMovement>().enabled = true;
            cam.GetComponent<Camera>().enabled = true;
            gameObject.GetComponentInChildren<AudioListener>().enabled = true;
        }
        else
        {
            transform.GetComponent<FirstPersonMovement>().enabled = false;
            cam.GetComponent<Camera>().enabled = false;
            gameObject.GetComponentInChildren<AudioListener>().enabled = false;
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

    public const byte teleportToRoom = 9, playerIsDead = 10;

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == teleportToRoom)
        {
            gameObject.transform.position = (Vector3)photonEvent.CustomData;
        }

        if (eventCode == playerIsDead)
        {
            gameObject.GetComponentInChildren<Animator>().SetTrigger("Dead");
            GetComponent<AudioSource>().clip = sfx;
            GetComponent<AudioSource>().Play();
            CanvasController.instance.toggleDeadScreen();

            if (photonView.IsMine)
            {
                GetComponent<FirstPersonMovement>().enabled = false;
                GetComponentInChildren<FirstPersonLook>().enabled = false;
                GetComponent<Collider>().enabled = false;
                Destroy(GetComponent<Rigidbody>());
            }
        }
    }
}
