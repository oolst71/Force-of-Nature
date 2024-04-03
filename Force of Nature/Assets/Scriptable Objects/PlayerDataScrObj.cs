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
    public float dashCooldown = 0.25f; //dash cooldown - currently unused
    public float coyoteTime = 0.1f;
    public float jumpBufferTime = 0.15f;
    public bool airDashed = false;
    public bool dashCd;
    public bool sideAttackBoosted;
    public bool upAttackBoosted;
    public float sideAttackPower = 60f;
    public float upAttackPower = 60f;

    public EntityStateScrObj[] playerStates = new EntityStateScrObj[8]; //add to this list whenever a new state is created

    //0 - Ability
    //1 - Attacking
    //2 - Dashing
    //3 - Idle
    //4 - Running
    //5 - Ulting

    public enum playerState { ABILITY, ATTACKING, DASHING, IDLE, RUNNING, ULTING, JUMPING, ATTACKINGUNLOCKED
    }

    public playerState currentState;
    
}

