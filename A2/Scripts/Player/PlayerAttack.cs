using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAttack : MonoBehaviourPun
{
    public static PlayerAttack instance;

    public float damage;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy" && photonView.IsMine)
        {
            other.GetComponent<EnemyManager>().TakeDamage(damage, PhotonNetwork.LocalPlayer);
        }
    }
}
