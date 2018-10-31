using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

    GameObject player;
    GameObject canvas;

    float health = 12;
    bool invincible;

	// Use this for initialization
	void Start () {
        invincible = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void takeDamage(int dmg) {
        if (!invincible) {
            health -= dmg;
        }
    }
}
