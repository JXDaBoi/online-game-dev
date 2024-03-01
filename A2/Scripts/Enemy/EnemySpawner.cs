using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    public GameObject[] enemies;
    public int enemySpawns;

    [SerializeField]
    List<Transform> enemySpawnpoints;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            enemySpawnpoints.Add(this.transform.GetChild(i));
        }
    }

    public void SpawnEnemy()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int enemyTypeLimit;

            enemySpawns = Random.Range(2, 8);
            MapInfo.instance.updateEnemyCount(enemySpawns);

            if (MapInfo.instance.getMapDifficulty() <= 5)
            {
                enemyTypeLimit = 3;
            }
            else if (MapInfo.instance.getMapDifficulty() > 5 && MapInfo.instance.getMapDifficulty() < 10)
            {
                enemyTypeLimit = 5;
            }
            else
            {
                enemyTypeLimit = 6;
            }

            for (int i = 0; i < enemySpawns; i++)
            {
                int spawnLocation = Random.Range(0, enemySpawnpoints.Count);
                int enemyIndex = Random.Range(0, enemyTypeLimit);

                PhotonNetwork.Instantiate("Enemies/" + enemies[enemyIndex].name, enemySpawnpoints[spawnLocation].position, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
            }
        }
    }
}
