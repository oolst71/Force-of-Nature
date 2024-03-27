using UnityEngine;
[CreateAssetMenu(fileName = "PlayerDataScrObj", menuName = "ScriptableObjects/PlayerData")]
public class PlayerDataScrObj : ScriptableObject
{

    public enum eqElement { BLIZZARD, WILDFIRE, TSUNAMI}

    public eqElement[] loadout = new eqElement[3];
    public int equipped;
    public int maxHealth = 100;
    public int health = 100;
    public bool abilitiesUnlocked = false;
    public bool accelBasedMovement = false; //use this for testing the acceleration system vs the flat movement
    public float accel = 0.25f;
    public float currAccel;
    public float deadzoneX;
    public float maxWalkSpeed = 5f;
    public float baseGravity;
    public float moveSpeed = 5f;
    public float jumpSpeed = 3f;
    public float dashPower = 50f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;
    public float coyoteTime = 0.1f;
    
}
