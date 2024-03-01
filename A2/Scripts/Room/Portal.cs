using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Portal : MonoBehaviourPun
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            LoadNewLevel();
        }
    }

    public const byte toNewLevel = 11;

    private void LoadNewLevel()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        PhotonNetwork.RaiseEvent(toNewLevel, true, raiseEventOptions, SendOptions.SendReliable);
    }
}
