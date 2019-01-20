using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearTheNoise : MonoBehaviour {
    
    public float timeTillCreep;
    public float creepTime;
    public float maxCreepVolume;
    public float minMusicPitch;
    public float maxMusicPitch;

    public float timeSinceLastClear;

    AudioSource noiseAudioSource;
    public AudioSource musicAudioSource;

    
	// Use this for initialization
	void Start () {
        noiseAudioSource = GetComponent<AudioSource>();
		
	}
	
	// Update is called once per frame
	void Update () {
        timeSinceLastClear = timeSinceLastClear + Time.deltaTime;

        if (timeSinceLastClear > timeTillCreep){
            if (timeSinceLastClear > (timeTillCreep + creepTime)){
                noiseAudioSource.volume = maxCreepVolume;
                musicAudioSource.pitch = minMusicPitch;
            }
                else{
                noiseAudioSource.volume = (timeSinceLastClear - timeTillCreep) * maxCreepVolume / creepTime;
                musicAudioSource.pitch = maxMusicPitch - (timeSinceLastClear - timeTillCreep) * (maxMusicPitch-minMusicPitch) / creepTime;
            }


        }
        else{
            noiseAudioSource.volume = 0;
            musicAudioSource.pitch = maxMusicPitch;
        }

	}

    public void ClearThatNoise(){
        timeSinceLastClear = 0;
    }



    
}
