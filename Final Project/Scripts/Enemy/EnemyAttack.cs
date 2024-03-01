using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PhotonView>().RPC("GetDamaged", RpcTarget.AllBuffered, gameObject.transform.parent.GetComponent<EnemyManager>().damage);
        }
    }
}
