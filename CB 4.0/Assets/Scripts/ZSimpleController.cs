using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZSimpleController : MonoBehaviour
{
    public float speed;
    public float patrolDistance;
    public float tountDistance;
    public float tountSpeed;

    public LayerMask layerMask;

    [HideInInspector]
    public bool facingRight = true;
    [HideInInspector]
    public bool dead = false;
    [HideInInspector]
    public GameObject target;

    private Rigidbody2D zombie;
    private Animator anim;
    private bool completelyDead = false;
    private float attackDistance = 1f;
    private float normalSpeed;
    private float timeBtwnAttck = 1f;
    private float attackTimer = 1f;
    
    
    private Vector3 startPosition;
    private Transform groundChek;
    private bool grounded = false;
    private float direction;
    
  

    void Update()
    {
        direction = zombie.transform.localScale.x;
        dead = GetComponent<Destructable>().dead;
        grounded = Physics2D.Linecast(transform.position, groundChek.position, 1 << LayerMask.NameToLayer("Ground"));
    }
    void Start ()
    {
        groundChek = transform.Find("groundChek");
        normalSpeed = speed;
        FindObjectOfType<AudioManager>().Play("Zombie_Rize");
        FindObjectOfType<AudioManager>().Play("Zombie_Move");
        zombie = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (!dead)
        {
            Animating();
            Attack();
            if (!IsTounted(zombie.transform.position, target.transform.position, tountDistance))
            {
                speed = normalSpeed * zombie.transform.localScale.x;
                if (!InPatrolArea())
                {
                    MoveTo(startPosition.x);
                }
                else
                {
                    Patroling();
                }
            }
            else
            {
                speed = tountSpeed * zombie.transform.localScale.x;
                MoveTo(target.transform.position.x);  
            }
        }
        if (dead)
        {
            Die();
        }
        
    }
    /// <summary>
    /// простое движение
    /// </summary>
    void Move()
    {
        zombie.transform.position += new Vector3(speed * Time.fixedDeltaTime, 0f, 0f);        
    }
    /// <summary>
    /// анимация движения
    /// </summary>
    void Animating()
    {
        if (speed < 0)
        {
            AnimIsWalking();
        }
        if (speed > 0)
        {
            AnimIsWalking();
        }
        if (speed == 0)
        {
            AnimIsIdle();
        }
        if (facingRight && speed < 0 || !facingRight && speed > 0)
        {
            Flip();
        }
    }
    void AnimIsWalking()
    {
        anim.SetBool("IsWalking", true);
        anim.SetBool("IsIdle", false);
    }
    void AnimIsIdle()
    {
        anim.SetBool("IsWalking", false);
        anim.SetBool("IsIdle", true);
    }
    /// <summary>
    /// Столкновение с триггером
    /// </summary>
    /// <param name="Объект с которым столкнулись"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            GetComponent<Destructable>().dead = true;
        } 
    }
    /// <summary>
    /// переворот
    /// </summary>
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1f;
        transform.localScale = theScale;
    }
    /// <summary>
    /// атака
    /// </summary>
    void Attack()
    {       
        attackTimer += Time.fixedDeltaTime;
        if (attackTimer >= timeBtwnAttck)
        {
            if (target.GetComponent<PlayerController>().dead == false)
                //&& ((facingRight && (transform.position.x < target.transform.position.x))
                //|| (!facingRight && (transform.position.x > target.transform.position.x))))
            {
                if (target != null && IsTounted(zombie.transform.position, target.transform.position,attackDistance))//Mathf.Abs(transform.position.x - target.transform.position.x)<= attackDistance)
                {
                    attackTimer = 0;
                    anim.SetBool("IsAttacking", true);
                }
                else
                {
                    anim.SetBool("IsAttacking", false);
                }
            }
            else
            {
                anim.SetBool("IsAttacking", false);
            }
        }
    }
    /// <summary>
    /// патрулирование
    /// </summary>
    /// <param name="центр по X"></param>
    /// <param name="позиция по X в настоящее время"></param>
    /// <param name="расстояние от центра до края патрулирования по X"></param>
    void Patroling()
    {
        Move();
        if(Mathf.Abs(zombie.transform.position.x-startPosition.x) >=patrolDistance) //(Vector2.Distance(startPosition, zombie.transform.position)
        {  
            speed = speed * -1;
        }
    }
    /// <summary>
    /// движение к точке X
    /// </summary>
    /// <param name="точка X"></param>
    void MoveTo(float position)
    {        
        if  (Mathf.Abs(zombie.transform.position.x-position) > 0.1) 
        {
            if (facingRight && zombie.transform.position.x < position 
                || !facingRight && zombie.transform.position.x > position)
            {
                Move();
            }
            else
            {
                speed = speed * (-1);
                Move();
            }
        }
        else speed = 0; 
    }
    /// <summary>
    /// Находится ли патрульный в зоне патрулирования
    /// </summary>
    /// <param name="координата патрульного по X в настоящее время"></param>
    /// <param name="центр патрулирования по X"></param>
    /// <param name="расстояние от центра до края патрулирования"></param>
    /// <returns></returns>
    bool InPatrolArea()
    {
        if (Mathf.Abs(zombie.transform.position.x - startPosition.x)<patrolDistance)
        {
            return true;
        }
        else return false;
    }
    /// <summary>
    /// Попал ли один объект A в зону внимания объекта B
    /// </summary>
    /// <param name="позиция объекта B по координате X"></param>
    /// <param name="позиция объекта A по координате X"></param>
    /// <param name="Расстояние привлечения внимания"></param>
    /// <returns></returns>
    bool IsTounted(Vector2 currentPosition, Vector2 targetPosition, float tountDistance)
    {
        RaycastHit2D hit = Physics2D.Raycast(currentPosition,Vector2.right*zombie.transform.localScale.x, tountDistance, layerMask);
        if (hit.collider !=null)
        {
            return true;
        }
        else return false;
    }
    void Die()
    {
        if (!completelyDead)
        {
            
            speed = 0;
            //anim.SetBool("IsAttacking", false);
            anim.SetTrigger("IsDead");
            if (grounded)
            {
                zombie.GetComponent<Rigidbody2D>().simulated = false;
            }
            zombie.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject, 1);
            completelyDead = true;
        }
        
    }
    
}
