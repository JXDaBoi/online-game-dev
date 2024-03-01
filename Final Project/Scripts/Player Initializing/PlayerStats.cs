using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class PlayerStats : MonoBehaviourPun, IOnEventCallback
{
    public float maximumHealth = 100f, currentHealth, maximumMana = 50f, currentMana;

    public bool isDead = false;

    private void Start()
    {
        currentHealth = maximumHealth;
        currentMana = maximumMana;
    }

    private void Update()
    {
        if (currentHealth <= 100f && currentHealth > 0)
        {
            photonView.RPC("HealthRegeneration", RpcTarget.AllBuffered, 0.75f * Time.deltaTime);
        }

        if (currentHealth <= 0)
        {
            GetComponent<FirstPersonMovement>().enabled = false;
            GetComponent<Shooting>().enabled = false;
            GetComponent<Jump>().enabled = false;
        }
    }

    [PunRPC]
    public void GetDamaged(float damage)
    {
        currentHealth -= damage;
    }

    [PunRPC]
    public void HealthRegeneration(float health)
    {
        currentHealth += health;
    }

    [PunRPC]
    public void UseMana(float mana)
    {
        currentMana -= mana;
    }

    [PunRPC]
    public void ReplenishMana(float mana)
    {
        currentMana += mana;
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public const byte teleportToRoom = 12;

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == teleportToRoom)
        {
            gameObject.transform.position = (Vector3)photonEvent.CustomData;
        }
    }
}
