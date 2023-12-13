using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]float moveSpeed = 5f;
    [SerializeField] float jumpSpeed = 3f;
    private Rigidbody2D rb;
    private float movementInput; //the horizontal axis input of the player
    private Vector2 aim; //the stick input of the player

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        rb.velocity = new Vector2(aim.x * moveSpeed, rb.velocity.y);
    }

    void OnJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }

    void OnStickInput(InputValue value)
    {
        aim = value.Get<Vector2>();
    }

    void OnTestAxis(InputValue value)
    {
        movementInput = value.Get<float>();
    }

}
