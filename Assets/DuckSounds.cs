using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckSounds : MonoBehaviour {

    public AudioSource duckLoop;
	// Use this for initialization

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartDucks(){
        duckLoop.Play();
    }

}
