using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour {

    public bool dead = false;

	public void Destroy () 
    {
        dead = true;
        
        //Player.GetComponent<Player>().dead=true;
	}
	
	
}
