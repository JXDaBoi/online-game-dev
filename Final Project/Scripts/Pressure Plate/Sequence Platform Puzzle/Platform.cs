using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public int platformIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (SequencePlatform.instance.SetSteppedPlatform(other.gameObject, platformIndex))
            {
                GetComponent<Collider>().enabled = false;
            }
        }
    }

    public void ResetPlatform()
    {
        GetComponent<Collider>().enabled = true;
    }
}
