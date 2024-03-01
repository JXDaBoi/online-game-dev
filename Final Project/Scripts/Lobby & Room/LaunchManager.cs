using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class LaunchManager : MonoBehaviourPunCallbacks
{
    public GameObject EnterGamePanel;
    public GameObject ConnectionStatusPanel;
    public GameObject LobbyPanel;
    public GameObject CreatePanel;
    public GameObject RoomPanel;
    public GameObject StartGameButton;

    public Text roomName, roomInfo;

    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject PlayerListItemPrefab;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        EnterGamePanel.SetActive(true);
        ConnectionStatusPanel.SetActive(false);
        LobbyPanel.SetActive(false);
        CreatePanel.SetActive(false);
        RoomPanel.SetActive(false);

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            if (!StartGameButton.activeInHierarchy)
            {
                StartGameButton.SetActive(true);
            }
        }
        else
        {
            if (StartGameButton.activeInHierarchy)
            {
                StartGameButton.SetActive(false);
            }
        }
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
                CreatePanel.SetActive(false);
                RoomPanel.SetActive(false);
            }
        }
    }

    public override void OnConnected()
    {
    }

    public override void OnConnectedToMaster()
    {
        LobbyPanel.SetActive(true);
        ConnectionStatusPanel.SetActive(false);

        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListing>().SetRoomInfo(roomList[i]);
        }
    }

    public override void OnCreatedRoom()
    {
        ConnectionStatusPanel.SetActive(false);
        EnterGamePanel.SetActive(false);
        LobbyPanel.SetActive(false);
        CreatePanel.SetActive(false);
        RoomPanel.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        ConnectionStatusPanel.SetActive(false);
        EnterGamePanel.SetActive(false);
        LobbyPanel.SetActive(false);
        CreatePanel.SetActive(false);
        RoomPanel.SetActive(true);

        roomName.text = PhotonNetwork.CurrentRoom.Name;
        roomInfo.text = "CREATE BY: " + PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.CurrentRoom.masterClientId).NickName + "               " + "NO. PLAYER : " + PhotonNetwork.CurrentRoom.PlayerCount;

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListing>().SetPlayerInfo(players[i]);
        }
    }

    public override void OnLeftRoom()
    {
        ConnectionStatusPanel.SetActive(false);
        EnterGamePanel.SetActive(false);
        LobbyPanel.SetActive(true);
        CreatePanel.SetActive(false);
        RoomPanel.SetActive(false);
        StartGameButton.SetActive(false);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        roomInfo.text = "CREATE BY: " + PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.CurrentRoom.masterClientId).NickName + "               " + "NO. PLAYER : " + PhotonNetwork.CurrentRoom.PlayerCount;
        Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListing>().SetPlayerInfo(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        roomInfo.text = "CREATE BY: " + PhotonNetwork.CurrentRoom.GetPlayer(PhotonNetwork.CurrentRoom.masterClientId).NickName + "               " + "NO. PLAYER : " + PhotonNetwork.CurrentRoom.PlayerCount;

        if (PhotonNetwork.IsMasterClient)
        {
            StartGameButton.SetActive(true);
        }
    }

    public void ToCreateRoom()
    {
        ConnectionStatusPanel.SetActive(false);
        EnterGamePanel.SetActive(false);
        LobbyPanel.SetActive(false);
        CreatePanel.SetActive(true);
        RoomPanel.SetActive(false);
    }

    public void BackLobby()
    {
        ConnectionStatusPanel.SetActive(false);
        EnterGamePanel.SetActive(false);
        LobbyPanel.SetActive(true);
        CreatePanel.SetActive(false);
        RoomPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
