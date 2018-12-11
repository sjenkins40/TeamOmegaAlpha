using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletCreate : MonoBehaviour {
    public AudioClip ShootClip;
    public AudioSource ShootSource;

    // Use this for initialization
    void Start () {
        ShootSource.clip = ShootClip;
        ShootSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
