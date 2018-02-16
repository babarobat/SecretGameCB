using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Destroy : MonoBehaviour 
{
    public float radius;

    private void Awake()
    {
        Collider2D[] damaged = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D s in damaged)
        {
            {
                
                Destructable dest = s.GetComponent<Destructable>();

                if (dest != null)
                
                    dest.Destroy();
                    
                        
            }
        }

    }

    void Start () 
	{
		FindObjectOfType<AudioManager>().Play("Explosion");
        Destroy (gameObject, 0.5f);


	}

}
