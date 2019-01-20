using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckHit : MonoBehaviour {
    public AudioSource duckHit;
    public AudioSource duckLoop;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DuckWasHit(){
        duckHit.Play();
        duckLoop.Stop();

    }
}
