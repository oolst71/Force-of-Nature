using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireMonsterBehaviuor : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float chaseSpeed;
    //[SerializeField] float attackOffset;
    //[SerializeField] float attackDowntime;
    private float startAttacktimer;
    private bool startTimer;
    //public GameObject Spit;
    [SerializeField] Vector2 moveDirection = new Vector2(1f, 0f);
    [SerializeField] GameObject rightCheck;
    [SerializeField] GameObject groundCheck;
    [SerializeField] Vector2 rightCheckSize;
    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] LayerMask Wall;
    //[SerializeField] bool goingUp = true;
    public EnemyDataScrObj slimeData;
    public float lineofsight;
    private bool seeplayer;
    private Transform player;
    private bool isflipped;
    private bool touchedGround, touchedRight;
    private Rigidbody2D EnemyRB;
    private SpriteRenderer sp;
    private BoxCollider2D coll;
    [SerializeField] private GroundDetection groundDetector;
    public BoxCollider2D GroundCollider;

    private LayerMask plyr;
    private EntityTakeDamage dmgScript;
    private bool storeFreeze;
    private Animator anim;

    bool hurt;

    private float currentSpeed;
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sp = GetComponent<SpriteRenderer>();
        EnemyRB = GetComponent<Rigidbody2D>();
        plyr = LayerMask.GetMask("Player");
        coll = GetComponent<BoxCollider2D>();
        currentSpeed = chaseSpeed;
        dmgScript = GetComponent<EntityTakeDamage>();
    }

    public void Attack()
    {
        if (Physics2D.BoxCast(gameObject.transform.position, coll.bounds.size, 0f, Vector2.down, 0.01f, plyr))
        {
            player.GetComponent<PlayerController>().TakeDamage(slimeData.attackDamage);
        }
    }

    void Update()
    {
        if (!dmgScript.frozen && dmgScript.act != EntityTakeDamage.activeEffect.STUN)
        {
            if (storeFreeze != dmgScript.frozen)
            {
                
                anim.SetTrigger("Thaw");
                storeFreeze = dmgScript.frozen;

            }

            if (moveDirection.x==-1.0f)
                {
                    isflipped = true;
                }
                else
                {
                    isflipped = false;
                }
                float distanceFromPlayer = Vector2.Distance(transform.position, player.position);
                if (distanceFromPlayer <= lineofsight)
                {
                    seeplayer = true;
                }
                else
                {
                    seeplayer = false;
                }

                if (player.position.x < transform.position.x && seeplayer && !isflipped)
                {
                    sp.flipX = true;
                }
                if (player.position.x > transform.position.x && seeplayer && !isflipped)
                {
                    sp.flipX = false;
                }
                if (player.position.x < transform.position.x && seeplayer && isflipped)
                {
                    sp.flipX = false;
                }
                if (player.position.x > transform.position.x && seeplayer && isflipped)
                {
                    sp.flipX = true;
                }
                if (!seeplayer /*&& isflipped*/)
                {
                    sp.flipX = false;
                }
        }
        else
        {
            if (storeFreeze != dmgScript.frozen)
            {

                anim.SetTrigger("Freeze");
                storeFreeze = dmgScript.frozen;

            }
            EnemyRB.velocity = Vector2.zero;
        }
      
        

        //StartSpawn();
    }

    void FixedUpdate()
    {
        if (!dmgScript.frozen && dmgScript.act != EntityTakeDamage.activeEffect.STUN)
        {
            if (!seeplayer)
            {
                EnemyRB.velocity = moveDirection * moveSpeed;
                HitLogic();
            }
            else
            {
                startTimer = true;
                //if (player.position.x - attackOffset < this.transform.position.x || player.position.x + attackOffset > this.transform.position.x)
                //{
                //    startTimer = true;
                //}
                transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(player.position.x, transform.position.y), chaseSpeed);
            }
        }
       
    }

    void HitLogic()
    {
        touchedRight = HitDetector(rightCheck, rightCheckSize, (Wall));
        touchedGround = HitDetector(groundCheck, groundCheckSize, (Wall));
        if(!touchedGround)
        {
            Flip();
        }
        if (touchedRight)
        {
            Flip();
        }
    }

    bool HitDetector(GameObject gameObject, Vector2 size, LayerMask layer)
    {
        return Physics2D.OverlapBox(gameObject.transform.position, size, 0f, layer);
    }

    private void OnTriggerEnter2D(GroundDetection GD)
    {
        if(GD==null)
        {
            Flip();
        }
    }


    void Flip()
    {
        transform.Rotate(new Vector2(0, 180));
        moveDirection.x = -moveDirection.x;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(rightCheck.transform.position, rightCheckSize);
        Gizmos.DrawWireSphere(transform.position, lineofsight);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.transform.position, groundCheckSize);
    }
    //void StartSpawn()
    //{
    //    startAttacktimer--;
    //    if (startAttacktimer < 0)
    //    {
    //        startAttacktimer = 0;
    //    }
    //    if (startTimer)
    //    {
    //        if (startAttacktimer == 0)
    //        {
    //            StartCoroutine(Attack());
    //            startAttacktimer = attackDowntime;
    //            //chaseSpeed = 0;

    //        }
    //    }
    //    if (startAttacktimer > 0)
    //    {
    //        startTimer = false;
    //    }

    //}
    //IEnumerator Attack()
    //{
    //    yield return new WaitForSeconds(1f);
    //    Instantiate(Spit, transform.position, Quaternion.identity);
    //}
}