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
    private float dir;
    private Rigidbody2D rb;
    private int currentTarget;
    private bool attackActive;
    private bool attackCooldown;
    [SerializeField] private Vector2 atkSize;

    // Start is called before the first frame update
    void Start()
    {
        treeStates = treeData.entityStates;
        currentState = TreeState.IDLE;
        currentTarget = 0;
        attackActive = false;
        attackCooldown = true;
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (currentState)
        {
            case TreeState.ATTACKING:
                //if attacking, do nothing
                //if not attacking, check cooldowns and if can attack
                //if yes and yes, attack
                //else, walk towards player
                if (!attackActive)
                {

                    if (attackCooldown && Mathf.Abs(transform.position.x - player.transform.position.x) < atkSize.x)
                    {
                        Debug.Log("should start attack");
                        attackActive = true;
                        StartCoroutine("AttackingTree");
                    }
                    else
                    {
                        if (player.transform.position.x < transform.position.x)
                        {
                            dir = -1;
                        }
                        else
                        {
                            dir = 1;
                        }
                        //walk towards player if not in range
                        if (Mathf.Abs(transform.position.x - player.transform.position.x) > atkSize.x / 2)
                        {
                            rb.velocity = new Vector2(treeData.speed * dir, rb.velocity.y);

                        }

                    }
                }
                break;
            case TreeState.AIPATROLLING:
                //check player position
                if (Mathf.Abs(transform.position.x - player.transform.position.x) < (atkSize.x - 1))
                {
                    currentState = TreeState.ATTACKING;
                }

                //movetowards waypoints[currentTarget]
                if (Mathf.Abs(waypoints[currentTarget].transform.position.x - transform.position.x) < 0.05f)
                        {
                            Debug.Log("target reached!");
                            currentTarget++;
                            if (currentTarget >= waypoints.Length)
                            {
                                currentTarget = 0;
                            }
                        }
                if (waypoints[currentTarget].transform.position.x < transform.position.x)
                {
                    dir = -1;
                }
                else
                {
                    dir = 1;
                }
                rb.velocity = new Vector2(treeData.speed * dir, rb.velocity.y);


                break;
            case TreeState.IDLE:
                //check for player nearby, if player is nearby enter patrol state
                if (Mathf.Abs(transform.position.x - player.transform.position.x) < 15)
                {
                    currentState = TreeState.AIPATROLLING;
                    currentTarget = 0;
                }
                break;
            case TreeState.HURT:
                //do nothing
                break;
            default:
                break;
        }
    }

    private IEnumerator AttackingTree()
    {
        Debug.Log("starting atk");
        attackCooldown = false;
        Debug.Log("windup start");
        yield return new WaitForSeconds(treeData.attackWindupTime);
        //attack here
        Debug.Log("windup end, attacking");
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
        attackActive = false;
        currentState = TreeState.AIPATROLLING;
        Debug.Log("cooldown start");
        yield return new WaitForSeconds(treeData.attackCd);
        Debug.Log("cooldown end");
        attackCooldown = true;

    }

}
