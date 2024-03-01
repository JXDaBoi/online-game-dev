using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] TopRooms;
    public GameObject[] BottomRooms;
    public GameObject[] LeftRooms;
    public GameObject[] RightRooms;

    public static RoomTemplates instance;

    private void Awake()
    {
        instance = this;
    }
}
