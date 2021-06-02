using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedMovement : MovementController
{

    float horInp = 0;
    float vertInp = 0;

    public GroundedMovement() : base()
    {
    }
    public GroundedMovement(Transform objectTransform, Rigidbody rb2d, Animator animator, MovementSettings settings) : base(objectTransform, rb2d, animator, settings)
    {
        ResetState();
    }

    public override void HandleInput()
    {
        ApplyGroundMovement();
        ApplyGravity();
        base.HandleInput();
    }

    private void ApplyGroundMovement()
    {
        horInp = Input.GetAxisRaw("Horizontal");
        vertInp = Input.GetAxisRaw("Vertical");

        float speedMultiplier = (Input.GetButton("Sprint")) ? settings.sprintMultiplier : 1;
        Vector3 move = rb2d.transform.right * horInp + rb2d.transform.forward * vertInp;


        rb2d.AddForce(move * settings.moveSpeed * speedMultiplier * Time.deltaTime, ForceMode.Impulse);
    }

    private void ApplyGravity()
    {
        Vector3 jumpForce = Vector3.down * settings.onlandGravity;
        rb2d.AddForce(jumpForce, ForceMode.Force);
    }

    public override void HandleAnimationTransition()
    {
        animator.SetBool("isWalking", horInp != 0 || vertInp != 0);
        animator.SetFloat("vertVel", vertInp * rb2d.velocity.magnitude);
    }

    public override void ResetState()
    {
        Physics.gravity = Vector3.down * settings.swimmingGravity;
    }
}
