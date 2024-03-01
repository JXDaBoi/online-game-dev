using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class EnemySpawner : MonoBehaviour, IOnEventCallback
{
    public static EnemySpawner instance;
    public const byte toSpawnEnemy = 4;
    public GameObject[] enemies;

    [SerializeField]
    List<Transform> enemySpawnpoints;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnEnemy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int enemySpawns;

            if (GameObject.FindGameObjectsWithTag("enemy").Length >= 20)
            {
                enemySpawns = 0;
            }
            else if (GameManager.instance.GamePhase == 0)
            {
                enemySpawns = Random.Range(0, 4);
            }
            else if (GameManager.instance.GamePhase == 1)
            {
                enemySpawns = Random.Range(5, 11);
            }
            else
            {
                enemySpawns = Random.Range(1, 7);
            }

            for (int i = 0; i < enemySpawns; i++)
            {
                int spawnLocation = Random.Range(0, enemySpawnpoints.Count);
                int enemyIndex = Random.Range(0, enemies.Length);

                PhotonNetwork.Instantiate("Enemies/" + enemies[enemyIndex].name, enemySpawnpoints[spawnLocation].position, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
            }
        }
        else
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
            PhotonNetwork.RaiseEvent(toSpawnEnemy, true, raiseEventOptions, SendOptions.SendReliable);
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == toSpawnEnemy)
        {
            SpawnEnemy();
        }
    }
}
