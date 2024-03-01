using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapPlatform : MonoBehaviour
{
    public GameObject[] enemySpawnSets;

    bool isDeactivated = false;

    private void Update()
    {
        bool onlyOnce = false;

        if (SequencePlatform.instance.platformIndex == 17 && !onlyOnce)
        {
            onlyOnce = true;
            isDeactivated = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player" && !isDeactivated)
        {
            SequencePlatform.instance.SetSteppedPlatform(other.gameObject, -1);

            foreach (var spawnset in enemySpawnSets)
            {
                spawnset.GetComponent<EnemySpawner>().SpawnEnemy();
            }
        }
    }
}
