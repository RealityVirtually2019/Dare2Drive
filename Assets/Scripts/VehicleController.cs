using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour {
    public float deltaModifier=1f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //float sign = (wheelAuto.deltaRotation > 0f) ? 1f : (wheelAuto.deltaRotation == 0f) ? 0f : -1f; 

        if (Input.GetKey(KeyCode.W))
        {

            transform.position += transform.forward * Time.deltaTime*deltaModifier;
            transform.Rotate(Vector3.up*Time.deltaTime * wheelAuto.deltaRotation);

        }
       
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * Time.deltaTime;
            transform.Rotate(Vector3.up * Time.deltaTime * wheelAuto.deltaRotation);
        }

        //transform.RotateAround(transform.position,transform.TransformDirection(new Vector3(0, 1, 0)), wheelAuto.deltaRotation * Time.deltaTime);
        
    }
    
}
