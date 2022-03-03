using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController character_controller;
    private Vector3 move_direction;
    public float speed = 5f;
    private float gravity = 20f;
    public float jump_Force = 10f;
    private float vertical_Velocity;

    private void Awake()
    {
        character_controller = GetComponent<CharacterController>();
    }
   

    // Update is called once per frame
    void Update()
    {
        movetheplayer();
    }
    void movetheplayer()
    {
        move_direction = new Vector3(Input.GetAxis(Axis.HORIZONTAL), 0f, Input.GetAxis(Axis.VERTICAL));
        move_direction = transform.TransformDirection(move_direction);
        move_direction *= speed * Time.deltaTime;
        ApplyGravity();
        character_controller.Move(move_direction);
    }
    void ApplyGravity()
    {
        
            vertical_Velocity -= gravity * Time.deltaTime;
            PlayerJump();
        
            move_direction.y = vertical_Velocity * Time.deltaTime;
    }
    void PlayerJump()
    {
        if(character_controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            vertical_Velocity = jump_Force;
        }

    }
}
