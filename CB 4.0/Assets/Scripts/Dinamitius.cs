using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dinamitius : MonoBehaviour {

	public GameObject explosionFX;

	public float delay;
    
	float countdown;
    

    private GameObject player;

    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Fetil");
        countdown = delay;
        
	}


    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f)
        {
            
            
            Instantiate(explosionFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }


    }
}
