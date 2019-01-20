/************************************************************************************
Filename    :   countDownTimer*.cs
Author      :   Geyao Zhang
Created     :   Janurary *th, 2019
MIT License
Copyright (c) 2019 Reality Virtually Hachathon
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is furnished
to do so, subject to the following conditions:
The above copyright notice and this permission notice (including the next
paragraph) shall be included in all copies or substantial portions of the
Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS
OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF
OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class countDownTimer : MonoBehaviour {

    public float Timer = 300f;
    private string Minute;
    private string Second;
    private AudioSource AlarmSound;
    private bool CountDownRunning;
    private bool AlarmIsPlaying;
    public bool StartToCountDown;
    public bool isArrived;

    public GameObject TimeUp;


    void Awake()
    {
        StartToCountDown = true;
        //AlarmSound = GetComponent<AudioSource>();
        CountDownRunning = true;
        AlarmIsPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!StartToCountDown)
        {
            return;
        }
        else
        {
            CountDown();
            StartToCountDown = true;
        }
    }

    private void CountDown()
    {

        //Countdown then destroy this gameObject
        //Debug.Log(Timer);
        if (CountDownRunning)
        {
            Timer -= Time.deltaTime;
        }
        else
        {
            Timer = 0f;
        }



        Minute = Mathf.Floor((int)Timer / 60).ToString("00");
        Second = Mathf.Floor((int)Timer % 60).ToString("00");
        //print((int)Timer);
        GetComponent<Text>().text = Minute + " : " + Second;

        if ((int)Timer == 0 && !AlarmIsPlaying)
        {
            CountDownRunning = false;
            AlarmSound.Play();
            AlarmIsPlaying = true;
        }

        if ((int)Timer == 0) {
            CountDownRunning = false;
            if (isArrived)
            {
                //Win
                print("Win");
            }
            else
            {
                //Game Over
                TimeUp.SetActive(true);
                print("Game Over");
            }
        }


        //if (AlarmIsPlaying)
        //{
        //    //transform.Rotate(Vector3.forward * Mathf.Clamp((Mathf.Cos(Time.time * 60f) * 180) / Mathf.PI * 0.5f, -2f, 2f), Space.Self);
        //}
        //else
        //{
        //    if (!CountDownRunning)
        //    {
        //        Destroy(gameObject);
        //    }

        //}
    }

    //public void SilenceAlarm()
    //{
    //    AlarmIsPlaying = false;
    //}
}
