using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class LeaveRoom : MonoBehaviourPunCallbacks
{
    public void ToLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
}
