using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class CaveTrigger : MonoBehaviour, IOnEventCallback
{
    public GameObject NotificationSet, TowerTip, PortalCharge;
    public Text NotificationText;

    bool[] onlyOnce = new bool[10];

    private void OnTriggerEnter(Collider other)
    {
        if (!onlyOnce[0] && other.tag == "Player")
        {
            onlyOnce[0] = true;

            TeleportToRoom();
        }
    }

    IEnumerator ReachCave()
    {
        NotificationSet.SetActive(true);
        yield return new WaitForSeconds(5f);
        NotificationSet.SetActive(false);
        yield return new WaitForSeconds(5f);
        StartCoroutine(TowerTips());
    }

    IEnumerator TowerTips()
    {
        TowerTip.SetActive(true);
        yield return new WaitForSeconds(8f);
        TowerTip.SetActive(false);
    }

    public const byte caveTrigger = 13;
    public void TeleportToRoom()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(caveTrigger, true, raiseEventOptions, SendOptions.SendReliable);
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == caveTrigger)
        {
            NotificationText.text = "Mystic Cave";
            GameManager.instance.GamePhase = 2;
            PortalCharge.SetActive(true);
            GetComponent<Collider>().enabled = false;
            StartCoroutine(ReachCave());
        }
    }
}
