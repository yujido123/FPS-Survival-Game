using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController charactorCtr;
    private Vector3 moveDirection;

    private float gravity = 20f;

    public float jumpForce = 10f;
    private float verticalVelocity;

    public float speed = 5f;

    void Awake()
    {
        charactorCtr = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveThePlayer();
    }

    void MoveThePlayer()
    {
        moveDirection = new Vector3(Input.GetAxis(Axis.HORIZONTAL), 0f, Input.GetAxis(Axis.VERTICAL));
        moveDirection = transform.TransformDirection(moveDirection);

        moveDirection *= speed;

        ApplyGravity();

        charactorCtr.Move(moveDirection * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (charactorCtr.isGrounded)
        {
            
            PlayerJump();
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        moveDirection.y = verticalVelocity;
    }

    private void PlayerJump()
    {
        if(charactorCtr.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            verticalVelocity = jumpForce;
        }
    }
}
