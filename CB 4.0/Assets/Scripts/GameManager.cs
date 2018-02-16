using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject zombie;
    public GameObject zombieMoving;
    //public GameObject bulletCountText;
    


    void Start()
    {
        
        zombie.GetComponent<ZSimpleController>().target = player;
        zombieMoving.GetComponent<ZJustMove>().target = player;
    }

    void Update()
    {
        //bulletCountText.GetComponent<BulletCountText>().text.text = player.GetComponent<PlayerController>().bulletsCount.ToString();
    }

	
}
