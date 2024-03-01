using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class RoomTrigger : MonoBehaviourPun
{
    public GameObject pillar;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && transform.parent.tag != "startingRoom")
        {
            MapInfo.instance.TeleportToRoom(other.transform.position);

            // To trigger monster spawning
            GetComponentInChildren<EnemySpawner>().SpawnEnemy();

            if (pillar != null)
            {
                pillar.GetComponent<EndPillarToggle>().trigger = true;
                MapInfo.instance.finishedGoal++;
            }

            Destroy(GetComponent<Collider>());
        }
    }
}
