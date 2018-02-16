using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour {

    public LayerMask layerMask;
    public Rigidbody2D bullet;
    public Rigidbody2D dinamite;
    private Transform gunPos;
    public float bulletSpeed;
    public float dinThrowSpeed;
    public bool isDinTrow = false;
    public bool dead = false;
    private Rigidbody2D enemy1;

    public GameObject target;
    private Animator anim;
    private bool completelyDead = false;

    private float timer = 0;
    public float timeToFire = 1;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        enemy1 = GetComponent<Rigidbody2D>();
        gunPos = transform.Find("Gun");
    }
    void FixedUpdate()
    {
        if (!dead)
        {
            LookToTarget();
            timer += Time.fixedDeltaTime;
            if (IsTounted())
            {
                Shoot();
            }
            TrowDinamite();
        }
        if (dead)
        {
            Die();
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        dead = GetComponent<Destructable>().dead;

        
    }
    void TrowDinamite()
    {
        if (isDinTrow && timer > timeToFire)
        {
            timer = 0;
            Rigidbody2D newDinamite = Instantiate(dinamite, new Vector3(-1, 9, 0), Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;//enemy1.transform.position
            newDinamite.AddForce(new Vector2(dinThrowSpeed * enemy1.transform.localScale.x, 250f)); //
        }

    }
    void Shoot()
    {
        if (timer > timeToFire)
        {
            timer = 0;
            FindObjectOfType<AudioManager>().Play("Gun_Shoot");
            Rigidbody2D newBullet = Instantiate(bullet, gunPos.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
            newBullet.AddForce(new Vector2(bulletSpeed * enemy1.transform.localScale.x, 0f));
        }

    }
    bool IsTounted()
    {

        RaycastHit2D hit = Physics2D.Raycast(enemy1.transform.position, Vector2.right * enemy1.transform.localScale.x, 100, layerMask);

        if ((hit.collider != null))
        {
            return true;
        }
        else return false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "BoxKiller" || other.tag == "Bullet")
        {
            GetComponent<Destructable>().dead = true;
        }
    }
    void LookToTarget()
    {
        if (enemy1.transform.position.x <target.transform.position.x)
        {
            enemy1.transform.localScale = new Vector3(1f, enemy1.transform.localScale.y, enemy1.transform.localScale.z);
        }
        if (enemy1.transform.position.x > target.transform.position.x)
        {
            enemy1.transform.localScale = new Vector3(-1f, enemy1.transform.localScale.y, enemy1.transform.localScale.z);
        }
       
        
    }
    bool FacingRight()
    {
        if (enemy1.transform.localScale.x == 1)
        {
            return true;
        }
        else return false;
    }
    void Die()
    {
        if (!completelyDead)
        {
            enemy1.GetComponent<Rigidbody2D>().simulated = false;
            anim.SetTrigger("IsDead");
            FindObjectOfType<AudioManager>().Play("Player_Death");
            Destroy(gameObject, 1);
            completelyDead = true;
        }
       
    }
    

}
