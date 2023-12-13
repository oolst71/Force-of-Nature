using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]float moveSpeed = 5f;
    [SerializeField] float jumpSpeed = 13f;
    private Rigidbody2D rb;
    private float movementInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        rb.velocity = new Vector2(movementInput * moveSpeed, rb.velocity.y);
    }

    void OnJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }

    void OnMovement(InputValue value)
    {
        movementInput = value.Get<float>();
    }

}
