using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletCountText : MonoBehaviour {
    [HideInInspector]
    public Text text;
	// Use this for initialization
	void Start ()
    {
        text = GetComponent<Text>();
        
    }
	
	// Update is called once per frame
	void Update () {
       // Debug.Log(GetComponent<PlayerController>().bulletsCount);
        //text.text = GetComponent<PlayerController>().bulletsCount.ToString();
        
	}
}
