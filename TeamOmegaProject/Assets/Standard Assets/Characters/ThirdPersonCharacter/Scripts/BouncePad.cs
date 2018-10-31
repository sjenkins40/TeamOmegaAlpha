using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour {
    public int speed;
    // Use this for initialization
    void Start () {

	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Rigidbody>
                    ().AddForce(Vector3.up * speed);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
