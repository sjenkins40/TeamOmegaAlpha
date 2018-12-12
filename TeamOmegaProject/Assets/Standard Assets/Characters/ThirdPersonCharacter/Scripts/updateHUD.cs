using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class updateHUD : MonoBehaviour {

    public GameObject[] hearts;
    public int health;

    public void updateHealth(int h) {
        health = h;
        for (int i = 0; i < 3; i++) {
            GameObject tmp = hearts[i];
            if (health - 4 > 0) {
                tmp.GetComponent<updateHealth>().updateH(4);
            } else {
                if (health < 0) {
                    tmp.GetComponent<updateHealth>().updateH(0);
                } else {
                    tmp.GetComponent<updateHealth>().updateH(health);
                }
            }
            health = health - 4;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
