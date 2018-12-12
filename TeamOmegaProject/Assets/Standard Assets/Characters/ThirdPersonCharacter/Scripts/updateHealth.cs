using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class updateHealth : MonoBehaviour
{

    public Sprite[] hearts;
    public SpriteRenderer sr;
    public int test;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void updateH(int h)
    {
        test = h;
        sr.sprite = hearts[h];
    }
}
