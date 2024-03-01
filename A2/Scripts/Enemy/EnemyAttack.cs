using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            MapInfo.instance.deductConsciousnessHealth(GetComponentInParent<EnemyManager>().damage);
        }
    }
}
