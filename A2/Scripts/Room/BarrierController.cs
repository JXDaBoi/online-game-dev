using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierController : MonoBehaviour
{
    private void Update()
    {
        if (!MapInfo.instance.isFighting)
        {
            GetComponentInChildren<MeshRenderer>().enabled = false;
            GetComponentInChildren<Collider>().enabled = false;
        }
        else
        {
            GetComponentInChildren<MeshRenderer>().enabled = true;
            GetComponentInChildren<Collider>().enabled = true;
        }
    }
}