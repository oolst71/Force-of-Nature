using UnityEngine;
[CreateAssetMenu(fileName = "EnemyDataScrObj", menuName = "ScriptableObjects/EnemyData")]

public class EnemyDataScrObj : ScriptableObject
{
    public enum DamageType { MELEE_NOBOOST, MELEE_FORWARDBOOST, MELEE_BACKBOOST,MELEE_UPBOOST, MELEE_UPLEFTBOOST, MELEE_UPRIGHTBOOST}
    public DamageType dmgType;
    public int maxHealth;
    public float knockUp;
}
