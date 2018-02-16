using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dinTrigger : MonoBehaviour {

	
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GetComponentInParent<Enemy1>().isDinTrow = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GetComponentInParent<Enemy1>().isDinTrow = false;
        }
    }
}
