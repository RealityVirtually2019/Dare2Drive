/************************************************************************************
Filename    :   eyeClose*.cs
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

public class eyeClose : MonoBehaviour
{

    public Transform otherEyelip;
    private Vector3 thisRotate;
    private Vector3 thisRotateDegree;
    private Vector3 reverseedRotateDegree;
    public Vector3 moveFactor;
    private float timer;
    private float mX;
    private bool eyeIsOpen = true;
    private bool isOpen = false;
    private bool stopEye = false;
    public bool startTired = false;
    public bool decideToDrive;
    private float speed;
    private float timeMultiplyer;
    // Use this for initialization
    IEnumerator OpenEye()
    {
        mX = -0.6f*speed;
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(StopEye(isOpen));
        isOpen = true;
    }

    IEnumerator CloseEye()
    {
        mX = 0.6f*speed;
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(StopEye(isOpen));
        isOpen = false;
    }

    IEnumerator StopEye(bool _isEyeOpen)
    {
        stopEye = true;
        float duration = Random.Range(2f,4f);
        yield return new WaitForSeconds(4f*timeMultiplyer);
        stopEye = false;
        eyeIsOpen = _isEyeOpen;
    }
    void Start()
    {
        thisRotate = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (decideToDrive)
        {
            speed = 2f;
            timeMultiplyer = 0.5f;
        }
        else {
            speed = 1f;
            timeMultiplyer = 1f;
        }
        if (!startTired)
        {
            return;
        }
        else
        {
            if (stopEye)
            {
                mX = 0f;
            }
            else
            {
                if (!eyeIsOpen)
                {
                    StartCoroutine(OpenEye());
                }
                else
                {
                    StartCoroutine(CloseEye());
                }
            }


            moveFactor = new Vector3(mX, moveFactor.y, moveFactor.z);
            transform.Rotate(moveFactor);
            thisRotate = transform.rotation.eulerAngles - thisRotateDegree;
            thisRotateDegree = transform.rotation.eulerAngles;
            //Vector3 factor = new Vector3(-1f, 1f, 1f);
            reverseedRotateDegree = otherEyelip.rotation.eulerAngles + thisRotate;
            otherEyelip.rotation = Quaternion.Euler(reverseedRotateDegree);
        }
    }
}
