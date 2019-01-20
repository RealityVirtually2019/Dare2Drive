using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSoundsManager : MonoBehaviour {

    public AudioSource carLoop;
    public AudioSource carStart;
    public AudioSource carOff;

	// Use this for initialization
	void Start () {

        StartTheCar();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartTheCar()
    {

        carStart.Play();
        float carStartLength = carStart.clip.length - .3f;
        carLoop.PlayDelayed(carStartLength);

    }

    public void StopCar(){

        carOff.Play();
        carLoop.Stop();

    }
}
