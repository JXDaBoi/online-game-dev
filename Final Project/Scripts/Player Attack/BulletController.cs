using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletController : MonoBehaviour
{
    public float bulletDamage = 1.5f;
    [SerializeField] float decayTime = 2f;

    float decayCountdown;

    private void Start()
    {
        decayCountdown = decayTime;

        if (PhotonNetwork.PlayerList.Length == 3)
        {
            bulletDamage = 1f;
        }
        else if (PhotonNetwork.PlayerList.Length == 4)
        {
            bulletDamage = 0.75f;
        }
    }

    private void Update()
    {
        if (decayCountdown > 0)
        {
            decayCountdown -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);

        if (other.tag == "enemy")
        {
            other.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, bulletDamage);
        }
    }
}
