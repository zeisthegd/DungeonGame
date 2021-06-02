using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimmingMovement : MovementController
{
    public SwimmingMovement() : base()
    {
    }
    public SwimmingMovement(Transform objectTransform, Rigidbody rb2d, Animator animator, MovementSettings settings) : base(objectTransform, rb2d, animator, settings)
    {
        ResetState();
    }

    public override void HandleInput()
    {
        base.HandleInput();
        ApplySwimMovement();
        Levitate();
        Descend();
    }

    public override void HandleAnimationTransition()
    {

    }

    private void ApplySwimMovement()
    {
        float horInp = Input.GetAxisRaw("Horizontal");
        float vertInp = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horInp, 0F, vertInp).normalized;

        animator.SetBool("isSwimming", direction.magnitude >= 0.1F);
        
    }

    private void Levitate()
    {
        if (Input.GetButton("Levitate"))
        {
            Vector3 jumpForce = Vector3.up * settings.jumpHeight;
            rb2d.AddForce(jumpForce, ForceMode.Acceleration);
        }

    }
    private void Descend()
    {
        if (Input.GetButton("Descend"))
        {
            Vector3 jumpForce = Vector3.down * settings.jumpHeight;
            rb2d.AddForce(jumpForce, ForceMode.Acceleration);
        }

    }

    public override void ResetState()
    {
        Physics.gravity = Vector3.down * settings.swimmingGravity;
    }
}
