using UnityEngine;
[CreateAssetMenu(fileName = "EntityStateScrObj", menuName = "ScriptableObjects/EntityState")]

public class EntityStateScrObj : ScriptableObject
{
    public string state;
    public bool canMove; 
    public bool canWalk; 
    public bool canJump; 
    public bool canDash;
    public bool canAttack;
    public bool canQueueAttacks;
    public bool canAbility;
    public bool canQueueAbilities;
    public bool canTakeDamage;
}
