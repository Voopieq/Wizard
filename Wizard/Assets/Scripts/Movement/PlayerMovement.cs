using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float _movementSpeed; //Consider making this public in future
    Rigidbody2D rb;
    Vector2 _moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveHorz = Input.GetAxisRaw("Horizontal");
        float moveVert = Input.GetAxisRaw("Vertical");

        _moveDirection = new Vector2(moveHorz, moveVert).normalized;
    }

    // Use FixedUpdate to set rigidbody force
    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        rb.velocity = new Vector2(_moveDirection.x * _movementSpeed, _moveDirection.y * _movementSpeed);
    }
}
