using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class EnemyManager : MonoBehaviourPun, IOnEventCallback
{
    public float maximumHealth, damage, attackCooldown, chaseDistance;

    private int playerAmount, aimedPlayerIndex;
    private float currentHealth, attackCooldownTimer, distanceToPlayer;
    private bool isDead = false;

    NavMeshAgent agent;
    Animator anim;
    Transform aimedPlayer;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            maximumHealth = maximumHealth + MapInfo.instance.getMapDifficulty();
            currentHealth = maximumHealth;
            UpdateEnemyCurrentHealth();
            UpdateEnemyMaximumHealth();

            agent = GetComponent<NavMeshAgent>();

            playerAmount = GameObject.FindGameObjectsWithTag("Player").Length;
            aimedPlayer = GameObject.FindGameObjectsWithTag("Player")[Random.Range(0, playerAmount)].transform;
        }

        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isDead && PhotonNetwork.IsMasterClient && MapInfo.instance.currentConsciousnessHealth > 0)
        {
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

    public void TakeDamage(float dmg, Player player)
    {
        attackCooldownTimer = attackCooldown;

        currentHealth -= dmg;
        UpdateEnemyCurrentHealth();

        anim.SetTrigger("damaged");

        if (currentHealth <= 0)
        {
            PlayerExperience.instance.experience += maximumHealth;
            isDead = true;
            Destroy(GetComponent<Collider>());
            Destroy(GetComponent<NavMeshAgent>());
            anim.SetTrigger("dead");

            if (PhotonNetwork.IsMasterClient)
            {
                MapInfo.instance.existingEnemy--;
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                photonView.RPC("UpdateEnemy", RpcTarget.MasterClient);
                photonView.RPC("DestroyEnemy", RpcTarget.MasterClient, GetComponent<PhotonView>().ViewID);
            }
        }
    }

    [PunRPC]
    public void DestroyEnemy(int viewID)
    {
        PhotonNetwork.Destroy(PhotonView.Find(viewID).gameObject);
    }

    [PunRPC]
    public void UpdateEnemy()
    {
        MapInfo.instance.existingEnemy--;
    }

    public const byte updateEnemyMaximumHealth = 6, updateEnemyCurrentHealth = 7;
    private void UpdateEnemyMaximumHealth()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(updateEnemyMaximumHealth, currentHealth, raiseEventOptions, SendOptions.SendReliable);
    }
    private void UpdateEnemyCurrentHealth()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(updateEnemyCurrentHealth, currentHealth, raiseEventOptions, SendOptions.SendReliable);
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == updateEnemyMaximumHealth)
        {
            maximumHealth = (float)photonEvent.CustomData;
        }

        if (eventCode == updateEnemyCurrentHealth)
        {
            currentHealth = (float)photonEvent.CustomData;
        }
    }
}
