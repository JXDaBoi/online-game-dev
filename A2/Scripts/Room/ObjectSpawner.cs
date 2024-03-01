using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objects;
    public float objectSpawnrate = .3f;

    [SerializeField]
    List<Transform> objectSpawnpoints;

    void Start()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            objectSpawnpoints.Add(this.transform.GetChild(i));
        }

        SpawnObjects();
    }

    void SpawnObjects()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < objectSpawnpoints.Count; i++)
            {
                float spawnDecision = Random.Range(0f, 1f);

                if (spawnDecision <= objectSpawnrate)
                {
                    int objectIndex = Random.Range(0, objects.Length);

                    float scaleRandomizer = Random.Range(.8f, 4f);
                    GameObject spawnedObject = PhotonNetwork.Instantiate("Obstacles/" + objects[objectIndex].name, objectSpawnpoints[i].position, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
                    spawnedObject.transform.localScale = new Vector3(scaleRandomizer, scaleRandomizer, scaleRandomizer);
                }
            }
        }
    }
}
