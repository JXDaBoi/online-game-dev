using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StartingRoom : MonoBehaviour
{
    public GameObject[] startingRoom;
    private int currentRoomLayer;

    void Start()
    {
        int roomIndex = Random.Range(0, startingRoom.Length);
        currentRoomLayer = Mathf.Max(Mathf.FloorToInt(Mathf.Abs(transform.position.x) / 20), Mathf.FloorToInt(Mathf.Abs(transform.position.z) / 20));

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject spawnedRoom;

            if (roomIndex == 0)
            {
                spawnedRoom = PhotonNetwork.Instantiate("Room Template/Quad Doorway/" + startingRoom[roomIndex].name, transform.position, Quaternion.identity);
            }
            else
            {
                spawnedRoom = PhotonNetwork.Instantiate("Room Template/Triple Doorway/" + startingRoom[roomIndex].name, transform.position, Quaternion.identity);
            }

            spawnedRoom.tag = "startingRoom";
            Destroy(spawnedRoom.GetComponentInChildren<ObjectSpawner>());
            Destroy(spawnedRoom.GetComponentInChildren<EnemySpawner>());
            Destroy(spawnedRoom.GetComponentInChildren<RoomTrigger>());
        }
    }
}
