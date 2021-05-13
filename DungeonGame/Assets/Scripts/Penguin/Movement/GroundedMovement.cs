using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedMovement : MovementController
{
    public GroundedMovement() : base()
    {
    }
    public GroundedMovement(Transform objectTransform, Rigidbody rb2d, Animator animator, MovementSettings settings) : base(objectTransform, rb2d, animator, settings)
    {
        ResetState();
    }

    public override void HandleInput()
    {
        base.HandleInput();
        ApplyGroundMovement();
        ApplyGravity();
    }

    public override void HandleAnimationTransition()
    {

    }

    private void ApplyGroundMovement()
    {
        float horInp = Input.GetAxisRaw("Horizontal");
        float vertInp = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horInp, 0F, vertInp).normalized;

        animator.SetBool("isSwimming", direction.magnitude >= 0.1F);
        Debug.DrawRay(settings.camera.transform.position, settings.camera.transform.forward * 100F, Color.blue);

        if (direction.magnitude >= 0.1F)
        {
            float horAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + settings.camera.transform.eulerAngles.y;
            float angleY = Mathf.SmoothDampAngle(objectTrf.eulerAngles.y, horAngle, ref settings.turnSmoothVelocity, settings.turnSmooth);
            objectTrf.rotation = Quaternion.Euler(0F, angleY, 0F);

            direction = Quaternion.Euler(0F, horAngle, 0F) * Vector3.forward;

            rb2d.AddForce(direction * settings.moveSpeed * settings.sprintMultiplier * Time.deltaTime, ForceMode.Acceleration);
        }

    }

    private void ApplyGravity()
    {
        Vector3 jumpForce = Vector3.down * settings.onlandGravity;
        rb2d.AddForce(jumpForce, ForceMode.Force);
    }

    public override void ResetState()
    {
        Physics.gravity = Vector3.down * settings.swimmingGravity;
    }
}
