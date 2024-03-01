using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shooting : MonoBehaviourPun
{
    public GameObject[] bulletPrefab;
    public Transform shootOrigin;
    public Camera cam;

    [SerializeField] private float fireRate = 1f;
    private float fireCooldown = 0f;

    private void Update()
    {
        fireCooldown += Time.deltaTime;

        if (Input.GetButton("Fire1") && GetComponent<PlayerStats>().currentMana >= 5f)
        {
            if (fireCooldown > fireRate)
            {
                int myIndex = 0;

                foreach (var player in PhotonNetwork.PlayerList)
                {
                    if (player.NickName == photonView.Owner.NickName)
                    {
                        photonView.RPC("Shoot", RpcTarget.All, shootOrigin.position, myIndex);
                    }

                    myIndex++;
                }

                fireCooldown = 0f;
                photonView.RPC("UseMana", RpcTarget.AllBuffered, 5f);
            }
        }

        if (GetComponent<PlayerStats>().currentMana <= 50f)
        {
            photonView.RPC("ReplenishMana", RpcTarget.AllBuffered, 2.5f * Time.deltaTime);
        }
    }

    [PunRPC]
    public void Shoot(Vector3 shootPosition, int playerIndex)
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        GameObject bullet = Instantiate(bulletPrefab[playerIndex], shootPosition, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().AddForce(ray.direction * 10f, ForceMode.Impulse);
    }
}
