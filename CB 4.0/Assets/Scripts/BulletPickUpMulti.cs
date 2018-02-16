using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPickUpMulti : MonoBehaviour {

    public int bulletCount;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<AudioManager>().Play("Gun_Load");
            Destroy(gameObject);
        }
    }
}
