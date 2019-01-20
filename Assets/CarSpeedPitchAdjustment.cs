using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSpeedPitchAdjustment : MonoBehaviour {

    AudioSource audioSource;
    public float initialPitchValue;
    public float maxPitch;
    public float minPitch;

    public float maxSpeed;


    public Slider speedSlider;
    public float speedValue;


	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        
        audioSource.pitch = initialPitchValue;
    }
	
	// Update is called once per frame
	void Update () {
        speedValue = speedSlider.value;
		
	}

    public void ChangeThatPitch(){

        CarPitch(speedValue);
    }

    public void CarPitch(float speedValue){
   

        audioSource.pitch = minPitch + (speedValue / maxSpeed)*(maxPitch - minPitch);

    }
}
