using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class MidhallTrigger : MonoBehaviour, IOnEventCallback
{
    public GameObject NotificationSet, RightBigDoor;
    public Text NotificationText;
    public GameObject enemySpawnSet;

    [SerializeField] bool triggerSpecial = false;
    bool[] onlyOnce = new bool[10];

    private void Update()
    {
        if (GameObject.FindGameObjectsWithTag("enemy").Length == 0 && triggerSpecial && !onlyOnce[1])
        {
            onlyOnce[1] = true;

            RightBigDoor.SetActive(false);
            Debug.Log("Door is opened");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!onlyOnce[0] && other.tag == "Player")
        {
            onlyOnce[0] = true;

            TeleportToRoom();
        }
    }

    void spawnEnemy()
    {
        enemySpawnSet.GetComponent<EnemySpawner>().SpawnEnemy();
    }

    IEnumerator ReachMidHall()
    {
        NotificationSet.SetActive(true);
        yield return new WaitForSeconds(5f);
        NotificationSet.SetActive(false);
    }

    public const byte teleportToRoom = 12;
    public void TeleportToRoom()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(teleportToRoom, new Vector3(0f, 0f, 0f), raiseEventOptions, SendOptions.SendReliable);
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

        if (eventCode == teleportToRoom)
        {
            spawnEnemy();
            NotificationText.text = "MidHall of Highblock Halls";
            GameManager.instance.GamePhase = 1;
            triggerSpecial = true;
            GetComponent<Collider>().enabled = false;
            StartCoroutine(ReachMidHall());
        }
    }
}
