using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeScript : MonoBehaviour {
    public Transform canvas;
    // Use this for initialization
    public void ResumeGame () {
        canvas.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
