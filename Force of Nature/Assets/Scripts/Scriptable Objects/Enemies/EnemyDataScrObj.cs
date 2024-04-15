using UnityEngine;
[CreateAssetMenu(fileName = "EnemyDataScrObj", menuName = "ScriptableObjects/EnemyData")]

public class EnemyDataScrObj : ScriptableObject
{
    public enum DamageType { MELEE_NOBOOST, MELEE_FORWARDBOOST, MELEE_BACKBOOST,MELEE_UPBOOST, MELEE_UPLEFTBOOST, MELEE_UPRIGHTBOOST}
    public DamageType dmgType;


    public EntityStateScrObj[] entityStates = new EntityStateScrObj[4];

    public enum EnemyState { ATTACKING, AIPATROLLING, IDLE, HURT }

    public float speed;
    public int maxHealth;
    public float knockUp;
    public float attackCd;
    public float attackWindupTime;
    public float attackRecoveryTime;
}
