/************************************************************************************
Filename    :   gainScore*.cs
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

public class gainScore : MonoBehaviour
{

    public int score;
    public Text Score;
    public Transform coins;
    private Transform[] allCoinss;
    private int size;

    public void Start()
    {
        size = coins.childCount;
    }

    protected virtual void Update()
    {
        size = coins.childCount;
        for (int i = 0; i < size; i++) {
            coins.GetChild(i).Rotate(transform.forward);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "coin")
        {
            print(score);
            score++;
            Destroy(other.gameObject);

        }
        Score.text = "$" + score.ToString();
    }
}
