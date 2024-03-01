using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequencePlatform : MonoBehaviour
{
    public static SequencePlatform instance;

    public GameObject[] platforms;
    public GameObject sewerDoor, topDoor, sewerExit;
    public Transform originTeleport;

    public int platformIndex = 0;

    private void Awake()
    {
        instance = this;
    }

    public bool SetSteppedPlatform(GameObject player, int platIndex)
    {
        if (platIndex == platformIndex)
        {
            platformIndex++;

            if (platformIndex == 20)
            {
                sewerDoor.SetActive(false);
                topDoor.SetActive(false);
                sewerExit.SetActive(false);
                GameManager.instance.GamePhase++;
            }

            return true;
        }
        else
        {
            for (int i = 0; i < platformIndex; i++)
            {
                platforms[i].GetComponentInChildren<Platform>().ResetPlatform();
            }

            platformIndex = 0;
            player.transform.position = originTeleport.position;

            return false;
        }
    }
}
