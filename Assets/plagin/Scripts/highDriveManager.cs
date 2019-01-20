/************************************************************************************
Filename    :   fly*.cs
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

public class highDriveManager : MonoBehaviour
{

    public RectTransform[] ts;
    public bool isHigh = false;
    public bool isVeryHigh = false;
    public bool allowFire = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isHigh)
        {
            return;
        }
        else
        {
            if (!allowFire)
            {
                return;
            }
            else
            {
                StartCoroutine(InstantiateWs());
            }
        }
    }
    IEnumerator InstantiateWs()
    {
        allowFire = false;
        yield return new WaitForSeconds(1f);
        int interger = Random.Range(0, ts.Length - 1);
        float height = Random.Range(-300f, 300f);
        Vector3 wordNewPos = new Vector3(1000f, height, 329f);
        RectTransform word = (RectTransform)Instantiate(ts[interger], wordNewPos, Quaternion.identity);
        word.SetParent(transform.GetChild(0));
        StartCoroutine(DestroyTs(word.transform));
        //word.position += word.right * -.08f;
        allowFire = true;
    }
    IEnumerator DestroyTs(Transform _T)
    {
        yield return new WaitForSeconds(20f);
        Destroy(_T.gameObject);
    }
}
