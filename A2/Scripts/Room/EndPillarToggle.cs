using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPillarToggle : MonoBehaviour
{
    [SerializeField]
    Material complete;

    public GameObject pillarSphere;
    public bool trigger;

    private void Update()
    {
        if (trigger)
        {
            if (MapInfo.instance.existingEnemy == 0)
            {
                pillarSphere.GetComponent<MeshRenderer>().material = complete;
            }
        }
    }
}
