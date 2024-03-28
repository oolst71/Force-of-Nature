using UnityEngine;
[CreateAssetMenu(fileName = "PlayerDataScrObj", menuName = "ScriptableObjects/PlayerData")]
public class PlayerDataScrObj : ScriptableObject
{

    public enum eqElement { BLIZZARD, WILDFIRE, TSUNAMI}

    public eqElement[] loadout = new eqElement[3]; //the player's current loadout
    public int equipped; //keeps track of which element the player currently has equipped
    public int maxHealth = 100; //max health of player
    public int health = 100; //current health of player
    public bool abilitiesUnlocked = false; //set to true after getting the artifact
    public bool accelBasedMovement = false; //use this for testing the acceleration system vs the flat movement
    public bool freeDirectionDash = true; //if this is on, the player can dash in all directions, if this is off, 8-directional dash
    public float accel = 0.25f; //acceleration
    public float decel = 0.25f; //deceleration
    public float currAccel; //current speed
    public float deadzoneX; //X axis stick deadzone
    public float deadzoneY; //Y axis stick deadzone
    public float maxWalkSpeed = 5f; //max speed
    public float baseGravity; //character gravity
    public float moveSpeed = 5f; //character movement speed - currently unused
    public float jumpSpeed = 3f; //jump power
    public float dashPower = 50f; //dash power
    public float dashDuration = 0.2f; //time that the dash takes
    public float dashCooldown = 0.5f; //dash cooldown - currently unused
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.15f; 
    
}

