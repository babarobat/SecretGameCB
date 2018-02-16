using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinPickUp : MonoBehaviour {
    public int dinCount;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<AudioManager>().Play("Gun_Load");
            Destroy(gameObject);
        }
    }
}
