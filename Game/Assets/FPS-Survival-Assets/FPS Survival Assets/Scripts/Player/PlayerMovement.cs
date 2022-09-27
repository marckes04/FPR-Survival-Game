using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController Character_controller;

    private Vector3 MoveDirection;

    public float speed = 5f;
    private float gravity = 20f;

    public float jump_Force = 10f;
    private float vertical_velocity;

    void Awake()
    {
        Character_controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        MoveThePlayer();
    }

    void MoveThePlayer()
    {
        MoveDirection = new Vector3(Input.GetAxis(Axis.HORIZONTAL), 0f,
                                     Input.GetAxis(Axis.VERTICAL));

        MoveDirection = transform.TransformDirection(MoveDirection);
        MoveDirection *= speed * Time.deltaTime;

        ApplyGravity();
        Character_controller.Move(MoveDirection);
    }

    void ApplyGravity()
    {
        if (Character_controller.isGrounded)
        {
            vertical_velocity -= gravity * Time.deltaTime;

            //jump
            playerJump();
        }
        else
        {
            vertical_velocity -= gravity * Time.deltaTime;
        }
        MoveDirection.y = vertical_velocity * Time.deltaTime;
        //MoveDirection.y = vertical_velocity;
    }

    void playerJump()
    {
        if (Character_controller.isGrounded && Input.GetKeyDown (KeyCode.Space))
        {
            vertical_velocity = jump_Force;
        }
    }
}
