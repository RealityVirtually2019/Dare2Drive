/************************************************************************************
Filename    :   popCar*.cs
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

public class popCar : MonoBehaviour {

    public GameObject[] carPool;
    public Transform nest;
    private bool gapOver = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!gapOver)
        {
            return;
        }
        else {
            StartCoroutine(PopulateGapTime());
            SendEverySomeSeconds();
            gapOver = false;
        }
	}

    IEnumerator PopulateGapTime() {
        yield return new WaitForSeconds(5f);
        gapOver = true;
    }

    private void SendEverySomeSeconds() {
        int i = Random.Range(0, carPool.Length - 1);
        carPool[i].SetActive(true);
        Instantiate(carPool[i], nest.position, Quaternion.identity);
    }
}
