using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomSpawner : MonoBehaviourPun
{
    public GameObject closedRoom;
    public string spawnPointDirection;
    private int maximumRoomLayer = 3;
    /*
        Maximum Room Layer affects the amount the room exist on a level
        x = maximumRoomLayer
        maximum room = (2n+1)^2 where n starts from 0

        x = 2, maximum rooms will be (2*(2-1)+1)^2 = 9
        x = 3, maximum rooms will be (2*(3-1)+1)^2 = 25
        x = 4, maximum rooms will be (2*(4-1)+1)^2 = 49

        ofcourse, the total spawned room will hardly be near or more than half of the amount of maximum room
    */
    private RoomTemplates templates;
    private int currentLayer, roomIndex;
    private bool spawned;

    /*
        3 3 3 3 3
        3 2 2 2 3
        3 2 1 2 3
        3 2 2 2 3
        3 3 3 3 3

        The cube above is the example of Room Layer
        1 -> The center room
        2 -> The rooms that surround and next to the center room
        3 -> The rooms that surround and next to the second layered room
    */

    void Start()
    {
        templates = RoomTemplates.instance;

        if (Mathf.Max(Mathf.Abs(transform.position.x) / 20, Mathf.Abs(transform.position.z) / 20) < maximumRoomLayer)
        {
            Invoke("Spawn", .1f);
        }
    }

    void Spawn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!spawned)
            {
                currentLayer = (Mathf.Max(Mathf.Abs((int)transform.position.x) / 20, Mathf.Abs((int)transform.position.z) / 20)) + 1;

                switch (spawnPointDirection)
                {
                    case "T":
                        if (currentLayer == 2)
                        {
                            roomIndex = Random.Range(1, templates.BottomRooms.Length);
                            PhotonNetwork.Instantiate("Room Template/Double Doorway/" + templates.BottomRooms[roomIndex].name, transform.position, Quaternion.identity);
                        }
                        else if (currentLayer == maximumRoomLayer)
                        {
                            GameObject spawnedRoom = PhotonNetwork.Instantiate("Room Template/Single Doorway/" + templates.BottomRooms[0].name, transform.position, Quaternion.identity);
                            MapInfo.instance.EndRooms.Add(spawnedRoom);
                        }
                        else
                        {
                            roomIndex = Random.Range(0, templates.BottomRooms.Length);

                            if (roomIndex == 0)
                            {
                                GameObject spawnedRoom = PhotonNetwork.Instantiate("Room Template/Single Doorway/" + templates.BottomRooms[roomIndex].name, transform.position, Quaternion.identity);
                                MapInfo.instance.EndRooms.Add(spawnedRoom);
                            }
                            else
                            {
                                PhotonNetwork.Instantiate("Room Template/Double Doorway/" + templates.BottomRooms[roomIndex].name, transform.position, Quaternion.identity);
                            }
                        }

                        break;

                    case "B":
                        if (currentLayer == 2)
                        {
                            roomIndex = Random.Range(1, templates.TopRooms.Length);
                            PhotonNetwork.Instantiate("Room Template/Double Doorway/" + templates.TopRooms[roomIndex].name, transform.position, Quaternion.identity);
                        }
                        else if (currentLayer == maximumRoomLayer)
                        {
                            GameObject spawnedRoom = PhotonNetwork.Instantiate("Room Template/Single Doorway/" + templates.TopRooms[0].name, transform.position, Quaternion.identity);
                            MapInfo.instance.EndRooms.Add(spawnedRoom);
                        }
                        else
                        {
                            roomIndex = Random.Range(0, templates.TopRooms.Length);

                            if (roomIndex == 0)
                            {
                                GameObject spawnedRoom = PhotonNetwork.Instantiate("Room Template/Single Doorway/" + templates.TopRooms[roomIndex].name, transform.position, Quaternion.identity);
                                MapInfo.instance.EndRooms.Add(spawnedRoom);
                            }
                            else
                            {
                                PhotonNetwork.Instantiate("Room Template/Double Doorway/" + templates.TopRooms[roomIndex].name, transform.position, Quaternion.identity);
                            }
                        }

                        break;

                    case "L":
                        if (currentLayer == 2)
                        {
                            roomIndex = Random.Range(1, templates.RightRooms.Length);
                            PhotonNetwork.Instantiate("Room Template/Double Doorway/" + templates.RightRooms[roomIndex].name, transform.position, Quaternion.identity);
                        }
                        else if (currentLayer == maximumRoomLayer)
                        {
                            GameObject spawnedRoom = PhotonNetwork.Instantiate("Room Template/Single Doorway/" + templates.RightRooms[0].name, transform.position, Quaternion.identity);
                            MapInfo.instance.EndRooms.Add(spawnedRoom);
                        }
                        else
                        {
                            roomIndex = Random.Range(0, templates.RightRooms.Length);

                            if (roomIndex == 0)
                            {
                                GameObject spawnedRoom = PhotonNetwork.Instantiate("Room Template/Single Doorway/" + templates.RightRooms[roomIndex].name, transform.position, Quaternion.identity);
                                MapInfo.instance.EndRooms.Add(spawnedRoom);
                            }
                            else
                            {
                                PhotonNetwork.Instantiate("Room Template/Double Doorway/" + templates.RightRooms[roomIndex].name, transform.position, Quaternion.identity);
                            }
                        }

                        break;

                    case "R":
                        if (currentLayer == 2)
                        {
                            roomIndex = Random.Range(1, templates.LeftRooms.Length);
                            PhotonNetwork.Instantiate("Room Template/Double Doorway/" + templates.LeftRooms[roomIndex].name, transform.position, Quaternion.identity);
                        }
                        else if (currentLayer == maximumRoomLayer)
                        {
                            GameObject spawnedRoom = PhotonNetwork.Instantiate("Room Template/Single Doorway/" + templates.LeftRooms[0].name, transform.position, Quaternion.identity);
                            MapInfo.instance.EndRooms.Add(spawnedRoom);
                        }
                        else
                        {
                            roomIndex = Random.Range(0, templates.LeftRooms.Length);

                            if (roomIndex == 0)
                            {
                                GameObject spawnedRoom = PhotonNetwork.Instantiate("Room Template/Single Doorway/" + templates.LeftRooms[roomIndex].name, transform.position, Quaternion.identity);
                                MapInfo.instance.EndRooms.Add(spawnedRoom);
                            }
                            else
                            {
                                PhotonNetwork.Instantiate("Room Template/Double Doorway/" + templates.LeftRooms[roomIndex].name, transform.position, Quaternion.identity);
                            }
                        }

                        break;
                }

                spawned = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (other.CompareTag("spawnpoint"))
            {
                if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
                {
                    PhotonNetwork.Instantiate("Room Template/" + closedRoom.name, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                spawned = true;
            }

            if (other.CompareTag("startingRoom"))
            {
                Destroy(gameObject);
                spawned = true;
            }
        }

    }
}