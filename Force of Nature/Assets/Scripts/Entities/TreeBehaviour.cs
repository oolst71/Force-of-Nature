using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TreeBehaviour : MonoBehaviour
{
    public enum TreeState { ATTACKING, AIPATROLLING, IDLE, HURT }
    public TreeState currentState;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerDataScrObj playerData;
    [SerializeField] private EnemyDataScrObj treeData;
    [SerializeField] private EntityStateScrObj[] treeStates;
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float hurtTime;
    private EntityTakeDamage dmgScript;
    private float dir;
    private Rigidbody2D rb;
    private int currentTarget;
    private bool attackActive;
    private bool attackCooldown;
    [SerializeField] private SideDetection sideDetector;
    [SerializeField] private GroundDetection groundDetector;
    [SerializeField] private Vector2 atkSize;
    private TreeAnimations anim;
    int storedHP;
    int currHP;

    // Start is called before the first frame update
    void Start()
    {
        dmgScript = GetComponent<EntityTakeDamage>();
        anim = GetComponent<TreeAnimations>();
        treeStates = treeData.entityStates;
        currentState = TreeState.IDLE;
        currentTarget = 0;
        attackActive = false;
        attackCooldown = true;
        rb = GetComponent<Rigidbody2D>();
        storedHP = dmgScript.health;
        dir = -1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currHP = dmgScript.health;
        if (currHP != storedHP)
        {
            storedHP = currHP;
            currentState = TreeState.HURT;
        }
        storedHP = dmgScript.health;
        switch (currentState)
        {
            case TreeState.ATTACKING:
                //if attacking, do nothing
                //if not attacking, check cooldowns and if can attack
                //if yes and yes, attack
                //else, walk towards player
                if (!dmgScript.frozen || dmgScript.act == EntityTakeDamage.activeEffect.STUN)
                {
                    if (!attackActive)
                    {

                        if (attackCooldown && Mathf.Abs(transform.position.x - player.transform.position.x) < atkSize.x)
                        {
                            Debug.Log("should start attack");
                            attackActive = true;
                            StartCoroutine("AttackingTree");
                        }
                            if (player.transform.position.x < transform.position.x)
                                {
                                    dir = -1;
                                }
                                else
                                {
                                    dir = 1;
                                }

                                //walk towards player if not in range
                                if (Mathf.Abs(transform.position.x - player.transform.position.x) > atkSize.x / 2 && attackActive == false)
                                {
                                    rb.velocity = new Vector2(treeData.speed * 1.2f * dmgScript.moveSpeedMulti * dir, rb.velocity.y);

                                }
                                transform.localScale = new Vector2(-dir, transform.localScale.y);
                        
                        }
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }

                break;
            case TreeState.AIPATROLLING:
                if (!dmgScript.frozen || dmgScript.act == EntityTakeDamage.activeEffect.STUN)
                {
                if (Mathf.Abs(transform.position.x - player.transform.position.x) < (atkSize.x - 1))
                {
                    currentState = TreeState.ATTACKING;
                } else
                if (sideDetector.flip)
                    {
                        dir *= -1;
                        sideDetector.flip = false;
                    }
                if (groundDetector.flip)
                    {
                        transform.position = new Vector2(transform.position.x + (-0.05f * dir), transform.position.y); 
                        dir *= -1;
                    }
                rb.velocity = new Vector2(treeData.speed * dmgScript.moveSpeedMulti * dir, rb.velocity.y);
                transform.localScale = new Vector2(-dir, transform.localScale.y);
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
                //check player position



                break;
            case TreeState.IDLE:
                //check for player nearby, if player is nearby enter patrol state
                if (!dmgScript.frozen || dmgScript.act == EntityTakeDamage.activeEffect.STUN)
                {
                    if (Mathf.Abs(transform.position.x - player.transform.position.x) < 35)
                    {
                        currentState = TreeState.AIPATROLLING;
                        currentTarget = 0;
                    }
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
                break;
            case TreeState.HURT:
                StopCoroutine("AttackingTree");
                attackActive = false;
                StartCoroutine("BeHurt");
                break;
            default:
                break;
        }
    }

    private IEnumerator AttackingTree()
    {
        Debug.Log("starting atk");
        rb.velocity = Vector2.zero;
        anim.AnimateAttack(1);
        attackCooldown = false;
        Debug.Log("windup start");
        yield return new WaitForSeconds(treeData.attackWindupTime);
        //attack here
        Debug.Log("windup end, attacking");
        anim.AnimateAttack(2);
        RaycastHit2D hit = Physics2D.BoxCast(new Vector2(transform.position.x + (dir * atkSize.x * 0.5f), transform.position.y), atkSize, 0, Vector2.right * dir, 0.01f, playerLayer);
        if (hit.collider != null)
        {
            playerData.hurtTime = hurtTime;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.transform.position = new Vector2(player.transform.position.x, player.transform.position.y + 0.3f);
            player.GetComponent<PlayerController>().TakeDamage(treeData.attackDamage);
        }
        Debug.Log("recovery start");
        yield return new WaitForSeconds(treeData.attackRecoveryTime);
        Debug.Log("recovery end");
        anim.AnimateAttack(3);
        attackActive = false;
        currentState = TreeState.AIPATROLLING;
        Debug.Log("cooldown start");
        yield return new WaitForSeconds(treeData.attackCd);
        Debug.Log("cooldown end");
        attackCooldown = true;

    }

    private IEnumerator BeHurt()
    {
        anim.AnimateAttack(4);
        yield return new WaitForSeconds(0.15f);
        currentState = TreeState.IDLE;
        attackCooldown = true;
        anim.AnimateAttack(5);
    }

}
