using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spnr : MonoBehaviour
{
    public float zombieSpeed;
    public GameObject Zombie;
    public float spanDelay;
    public Text winText;

    private float timeToSpawn = 0f;
    // Use this for initialization
    void Start ()
    {
        
    }
	void Update ()
    {
        Spawn();
	}

    void Spawn()
    {
        timeToSpawn += Time.deltaTime;
        if (timeToSpawn>= spanDelay)
        {
            GameObject newZombie = Instantiate(Zombie, transform.position, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
            newZombie.GetComponent<ZJustMove>().speed = zombieSpeed;
            timeToSpawn = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            FindObjectOfType<AudioManager>().Stop("Main_Theme_2");
            FindObjectOfType<AudioManager>().Play("Win_Theme_1");
            winText.text = "U  H A V E  W O N !";
            Destroy(gameObject);
           
        }
    }
}
