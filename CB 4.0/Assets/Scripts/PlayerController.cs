using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float jumpForce;
    [HideInInspector]
    public bool facingRight = true;
    [HideInInspector]
    public float speed;
    public float throwSpeed;

    public Text bulletCountText;
    public Text dinCountText;
    public Text dethText;




    public bool dead = false;
    private Scene scene;
    public int bulletsCount = 0;
    public int dinCount = 0;
    public Rigidbody2D bullet;
    public Rigidbody2D dinamite;

    private Rigidbody2D player;
    private Animator anim;
    private Transform gunPos;
    private Transform groundChek;
    private bool dieAlready = false;
    private bool grounded = false;
    
    private float bulletSpeed = 1200f;


    void Awake()
    {
        
        scene = SceneManager.GetActiveScene();
        groundChek = transform.Find("groundChek");
        player = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gunPos = transform.Find("Gun");  
    }
    void Update()
    {
        bulletCountText.text = bulletsCount.ToString() + " ( Lctrl)";
        dinCountText.text = dinCount.ToString() + " ( G)";
        
        dead = GetComponent<Destructable>().dead;
        grounded = Physics2D.Linecast(transform.position, groundChek.position, 1 << LayerMask.NameToLayer("Ground"));
    }
    void FixedUpdate()
    {
        
        if (dead == false)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float j = Input.GetAxisRaw("Jump");
            
            Jump(j);
            Animating(h);
            Fire();
            DinThrow();
            player.transform.position += new Vector3(h * speed * Time.fixedDeltaTime, 0f, 0f);
        }
        if (dead)
        {
            Die();
        }
        
    }
    public void RestartLvl()
    {
        
            SceneManager.LoadScene(scene.name);
        
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    void Animating(float h)
    {
        if (h != 0)
        {
            anim.SetBool("IsRunning", true);
        }
        if (h == 0)
        {
            anim.SetBool("IsRunning", false);
        }

        if (facingRight && h < 0 || !facingRight && h > 0)
        {
            Flip();
        }

    }
    void Jump(float j)
    {
        if (j > 0 && grounded && player.velocity.y <= 0.1f)
        {
            player.AddForce(new Vector2(0f, jumpForce));
        }
    }
    void DinThrow()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (dinCount >0)
            {
                dinCount--;
                Rigidbody2D newDin = Instantiate(dinamite, gunPos.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
                newDin.AddForce(new Vector2(throwSpeed*player.transform.localScale.x, 0f));
            }
            else
            {
                FindObjectOfType<AudioManager>().Play("Gun_NoBullets");
            }


        }
    }
    void Fire()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            anim.Play("PlayerFire");
            if (bulletsCount > 0)
            {
                FindObjectOfType<AudioManager>().Play("Gun_Shoot");
                bulletsCount--;
                if (facingRight)
                {
                    Rigidbody2D newBullet = Instantiate(bullet, gunPos.position, Quaternion.Euler(new Vector3(0, 0, 0))) as Rigidbody2D;
                    newBullet.AddForce(new Vector2(bulletSpeed, 0f));
                }
                else
                {
                    
                    Rigidbody2D newBullet = Instantiate(bullet, gunPos.position, Quaternion.Euler(new Vector3(0, 0, 180))) as Rigidbody2D;
                    newBullet.AddForce(new Vector2(-bulletSpeed, 0f));
                }
            }
            else
            {
                FindObjectOfType<AudioManager>().Play("Gun_NoBullets");
            }

        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Zombie"|| other.tag == "Bullet"|| other.tag == "Enemy")
        {
            GetComponent<Destructable>().dead = true;

        }
        if (other.tag == "BulletPickUp")
        {
            bulletsCount += other.GetComponent<BulletPickUpMulti>().bulletCount;
            
        }
        if (other.tag == "DinPickUp")
        {
            dinCount += other.GetComponent<DinPickUp>().dinCount;
        }
    }
    void Die()
    {
        if (!dieAlready)
        {
            FindObjectOfType<AudioManager>().Stop("Main_Theme_2");
            FindObjectOfType<AudioManager>().Play("Die_Theme_1");
            
            FindObjectOfType<AudioManager>().Play("Player_Death");
            anim.SetBool("IsRunning", false);
            anim.SetTrigger("IsDead");
            if (grounded)
            {
                player.simulated = false;
            }
            dethText.text = "U  R  D E A D !";
            dieAlready = true;
        }
        
    }
   
}
