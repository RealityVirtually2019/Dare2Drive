using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitOther : MonoBehaviour {

    public gainTickets thisGain;
    // Use this for initialization
    void Start()
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "playerCar")
        {
            thisGain.mistakes += 3;
            Destroy(gameObject);
        }
    }
}
