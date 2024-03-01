using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class LaunchManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public GameObject EnterGamePanel;
    public GameObject ConnectionStatusPanel;
    public GameObject LobbyPanel;
    public GameObject WaitingForPlayerPanel;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        EnterGamePanel.SetActive(true);
        ConnectionStatusPanel.SetActive(false);
        LobbyPanel.SetActive(false);
        WaitingForPlayerPanel.SetActive(false);

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void ConnectToPhotonServer()
    {
        if (!PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.NickName.Length != 0)
            {
                PhotonNetwork.ConnectUsingSettings();
                ConnectionStatusPanel.SetActive(true);
                EnterGamePanel.SetActive(false);
                LobbyPanel.SetActive(false);
                WaitingForPlayerPanel.SetActive(false);
            }
            else
            {
                Debug.Log("Name is empty");
            }
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room successfully");

        ConnectionStatusPanel.SetActive(false);
        EnterGamePanel.SetActive(false);
        LobbyPanel.SetActive(false);
        WaitingForPlayerPanel.SetActive(true);
    }

    public const byte toStartGame = 12;
    private void StartGame()
    {
        PhotonNetwork.LoadLevel("Level");
    }

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
            PhotonNetwork.RaiseEvent(toStartGame, true, raiseEventOptions, SendOptions.SendReliable);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " " + PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.NickName + " CONNECTED to the photon server");
        LobbyPanel.SetActive(true);
        ConnectionStatusPanel.SetActive(false);

        PhotonNetwork.JoinLobby();
    }

    public override void OnConnected()
    {
        Debug.Log("CONNECTED to Internet");
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == toStartGame)
        {
            StartGame();
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("test");
    }
}
