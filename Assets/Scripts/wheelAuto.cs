/************************************************************************************
Filename    :   wheelAuto.cs
Author      :   Lingyu Li
Created     :   Janurary 20th, 2019
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

public class wheelAuto : MonoBehaviour {

    /// <summary>
    /// The range of the wheel rotation
    /// </summary>
    public static float deltaRotation = 0f;

    private bool isRecovering = false;

    public bool setRecovering
    {
        set
        {
           
                isRecovering = value; 
        }
    }

	// Update is called once per frame
	void Update () {
		
        //steering wheel recover to deltaposition = 0f
        if(isRecovering && Mathf.Abs(deltaRotation) > 1f)
        {
            
                transform.RotateAround(transform.position, transform.TransformDirection(new Vector3(0, 1, 0)), - deltaRotation * Time.deltaTime);

                deltaRotation -= Time.deltaTime * deltaRotation;
        

            if(Mathf.Abs(deltaRotation) <= 1f)
            {
                deltaRotation = 0f;
                isRecovering = false;
            }
           
        }

	}
}
