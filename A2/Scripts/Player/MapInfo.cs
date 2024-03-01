using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class MapInfo : MonoBehaviour, IOnEventCallback
{
    public static MapInfo instance;

    public List<GameObject> EndRooms;
    public bool isFighting = false;
    public int finishedGoal = 0, existingEnemy, roomCleared = 0;
    public float maximumConsciousnessHealth = 15f, currentConsciousnessHealth;

    private int goalAmount;
    private float mapDifficulty;

    bool sendOnce = false, onlyOnce = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        goalAmount = 0;
        currentConsciousnessHealth = maximumConsciousnessHealth;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Level")
        {
            Destroy(gameObject);
        }

        mapDifficulty = Mathf.Pow(Time.time * 0.01f, 1.5f);

        if (goalAmount != EndRooms.Count)
        {
            goalAmount = EndRooms.Count;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            if (existingEnemy == 0)
            {
                if (isFighting)
                {
                    roomCleared++;
                }

                isFighting = false;

                UpdateFightingStatus();
            }
            else
            {
                isFighting = true;

                UpdateFightingStatus();
            }
        }

        if (currentConsciousnessHealth <= 0)
        {
            if (PhotonNetwork.IsMasterClient && !sendOnce)
            {
                PlayerDead();

                sendOnce = true;
            }
        }

        if (finishedGoal == goalAmount && finishedGoal != 0 && !onlyOnce && PhotonNetwork.IsMasterClient)
        {
            onlyOnce = true;

            GoalIsDone();
        }
    }

    public const byte updateFSCode = 1, updateHealthCode = 2, updateMaximumHealthCode = 8, teleportToRoom = 9, playerIsDead = 10, toNewLevel = 11, goalIsDone = 12, leaveRoom = 13;
    private void UpdateFightingStatus()
    {
        object[] content = new object[] { isFighting, roomCleared };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(updateFSCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    private void UpdateHealth()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(updateHealthCode, currentConsciousnessHealth, raiseEventOptions, SendOptions.SendReliable);
    }

    private void UpdateMaximumHealth()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(updateMaximumHealthCode, maximumConsciousnessHealth, raiseEventOptions, SendOptions.SendReliable);
    }

    public void TeleportToRoom(Vector3 position)
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(teleportToRoom, position, raiseEventOptions, SendOptions.SendReliable);
    }

    public void PlayerDead()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(playerIsDead, true, raiseEventOptions, SendOptions.SendReliable);
    }

    public void GoalIsDone()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(goalIsDone, true, raiseEventOptions, SendOptions.SendReliable);
    }

    public void AllLeaveRoom()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(leaveRoom, true, raiseEventOptions, SendOptions.SendReliable);
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

        if (eventCode == updateFSCode)
        {
            isFighting = (bool)((object[])(photonEvent.CustomData))[0];
            roomCleared = (int)((object[])(photonEvent.CustomData))[1];
        }

        if (eventCode == updateHealthCode)
        {
            currentConsciousnessHealth = (float)photonEvent.CustomData;
        }

        if (eventCode == updateMaximumHealthCode)
        {
            maximumConsciousnessHealth = (float)photonEvent.CustomData;
        }

        if (eventCode == toNewLevel)
        {
            SceneManager.LoadScene("Level");
            EndRooms.Clear();
            sendOnce = false;
            onlyOnce = false;
        }

        if (eventCode == goalIsDone)
        {
            GameObject.FindGameObjectWithTag("portalGate").GetComponent<MeshRenderer>().enabled = true;
            GameObject.FindGameObjectWithTag("portalGate").GetComponent<Collider>().enabled = true;
        }

        if (eventCode == leaveRoom)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            PhotonNetwork.LeaveRoom();
            Destroy(gameObject);
        }
    }

    public void updateEnemyCount(int enemyCount)
    {
        existingEnemy = enemyCount;
    }

    public float getMapDifficulty()
    {
        return mapDifficulty;
    }

    public void deductConsciousnessHealth(float damage)
    {
        currentConsciousnessHealth -= damage;
        UpdateHealth();
    }

    public void addMaximumHealth()
    {
        maximumConsciousnessHealth *= 1.15f;
        currentConsciousnessHealth += currentConsciousnessHealth * 0.35f;

        if (currentConsciousnessHealth > maximumConsciousnessHealth)
        {
            currentConsciousnessHealth = maximumConsciousnessHealth;
        }

        UpdateHealth();
        UpdateMaximumHealth();
    }

    public void LeaveRoom()
    {
        AllLeaveRoom();
    }
}
