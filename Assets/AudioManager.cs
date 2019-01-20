using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource startingNarration;
    public AudioSource tutorialMusic;
    public AudioSource sleepyMusic;
    public AudioSource sleepNoise;
    public AudioSource finalRelief;

    public AudioSource partyMusic;
    public CarSoundsManager carSoundsManager;

    public AudioSource pullOverSuggestion;
    public AudioSource wakeUpTired;



    public AudioSource gameOverCrash;
    public AudioSource gameOverCops;
    public AudioSource gameOverTime;

	// Use this for initialization
	void Start () {

        startingNarration.Play();
        tutorialMusic.Play();
        carSoundsManager.StartTheCar();


	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ReachBobsHouse(){

        carSoundsManager.StopCar();
        tutorialMusic.Stop();
        partyMusic.Play();


    }

    public void StartSleepyDriving(){

        partyMusic.Stop();
        sleepNoise.Play();
        sleepyMusic.Play();
        wakeUpTired.Play();
        carSoundsManager.StartTheCar();

    }

    public void FinishAlive(){

        sleepNoise.Stop();
        sleepyMusic.Stop();
        carSoundsManager.StopCar();

    }

    public void GameOver(string typeOfGameOver){

        sleepNoise.Stop();
        sleepyMusic.Stop();
        carSoundsManager.StopCar();
        tutorialMusic.Stop();
        if (typeOfGameOver == "Cops"){
            gameOverCops.Play();
        }
        if (typeOfGameOver == "Time")
        {
            gameOverTime.Play();
        }
        if (typeOfGameOver == "Crash")
        {
            gameOverCrash.Play();
        }



    }

    public void FirstMistake(){
        pullOverSuggestion.Play();
    }
}
