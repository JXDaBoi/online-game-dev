using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using Photon.Pun;

public class EnemyManager : MonoBehaviourPun
{
    public float maximumHealth, damage, attackCooldown, chaseDistance;

    public Image healthImg;

    private int playerAmount, aimedPlayerIndex;
    private float currentHealth, attackCooldownTimer, distanceToPlayer;
    private bool isDead = false;

    NavMeshAgent agent;
    Animator anim;
    Transform aimedPlayer;


    private void Start()
    {
        currentHealth = maximumHealth;

        if (PhotonNetwork.IsMasterClient)
        {
            agent = GetComponent<NavMeshAgent>();

            playerAmount = GameObject.FindGameObjectsWithTag("Player").Length;

            if (GameManager.instance.GamePhase == 0)
            {
                int playerPicker = Random.Range(0, playerAmount);

                if (playerPicker == 0 || playerPicker == 1)
                {
                    if (GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerStats>().isDead)
                    {
                        if (playerAmount <= 2)
                        {
                            aimedPlayer = transform;
                        }
                        else
                        {
                            if (GameObject.FindGameObjectsWithTag("Player")[2].GetComponent<PlayerStats>().isDead)
                            {
                                aimedPlayer = transform;
                            }
                            else
                            {
                                aimedPlayer = GameObject.FindGameObjectsWithTag("Player")[2].transform;
                            }
                        }
                    }

                    aimedPlayer = GameObject.FindGameObjectsWithTag("Player")[0].transform;
                }
                else
                {
                    if (GameObject.FindGameObjectsWithTag("Player")[2].GetComponent<PlayerStats>().isDead)
                    {

                        if (GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerStats>().isDead)
                        {
                            aimedPlayer = transform;
                        }
                        else
                        {
                            aimedPlayer = GameObject.FindGameObjectsWithTag("Player")[0].transform;
                        }
                    }

                    aimedPlayer = GameObject.FindGameObjectsWithTag("Player")[2].transform;
                }
            }
            else if (GameManager.instance.GamePhase == 1)
            {
                aimedPlayer = GameObject.FindGameObjectsWithTag("Player")[Random.Range(0, playerAmount)].transform;
            }
            else
            {
                int playerOrtower = Random.Range(0, 2);

                if (playerOrtower == 0)
                {
                    aimedPlayer = GameObject.FindGameObjectsWithTag("Player")[Random.Range(0, playerAmount)].transform;
                }
                else
                {
                    aimedPlayer = GameObject.FindGameObjectsWithTag("chargingTower")[Random.Range(0, 5)].transform;
                }
            }
        }

        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        healthImg.rectTransform.sizeDelta = new Vector2(2 * currentHealth / maximumHealth, 0.25f);

        if (!isDead && PhotonNetwork.IsMasterClient)
        {
            if (aimedPlayer == null)
            {
                TakeDamage(99999);
            }

            distanceToPlayer = Vector3.Distance(aimedPlayer.position, transform.position);

            if (distanceToPlayer > chaseDistance)
            {
                agent.destination = aimedPlayer.position;
                anim.SetBool("isChasing", true);
            }
            else
            {
                transform.LookAt(aimedPlayer);
                anim.SetBool("isChasing", false);

                attackCooldownTimer -= Time.deltaTime;

                if (attackCooldownTimer <= 0)
                {
                    anim.SetTrigger("attack");
                    attackCooldownTimer = attackCooldown;
                }
            }
        }
    }

    [PunRPC]
    public void TakeDamage(float dmg)
    {
        attackCooldownTimer = attackCooldown;

        currentHealth -= dmg;

        anim.SetTrigger("damaged");

        if (currentHealth <= 0)
        {
            isDead = true;
            Destroy(GetComponent<Collider>());
            Destroy(GetComponent<NavMeshAgent>());

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                photonView.RPC("DestroyEnemy", RpcTarget.MasterClient, GetComponent<PhotonView>().ViewID);
            }
        }
    }

    [PunRPC]
    public void DestroyEnemy(int viewID)
    {
        PhotonNetwork.Destroy(PhotonView.Find(viewID).gameObject);
    }
}
