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
        Debug.DrawRay(settings.camera.transform.position, settings.camera.transform.forward * 100F, Color.blue);

        if (direction.magnitude >= 0.1F)
        {
            float horAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + settings.camera.transform.eulerAngles.y;
            float angleY = Mathf.SmoothDampAngle(objectTrf.eulerAngles.y, horAngle, ref settings.turnSmoothVelocity, settings.turnSmooth);
            objectTrf.rotation = Quaternion.Euler(0F, angleY, 0F);

            Vector3 yAxisDirection = (vertInp != 0) ? (settings.camera.transform.forward.y * Vector3.up) : Vector3.zero;
            int isMovingForwardScreen = (vertInp < 0) ? -1 : 1;
            direction = Quaternion.Euler(0F, horAngle, 0F) * Vector3.forward + yAxisDirection * isMovingForwardScreen;

            float speedMultiplier = (Input.GetButton("Sprint")) ? settings.sprintMultiplier : 1;
            rb2d.AddForce(direction * settings.moveSpeed * speedMultiplier * Time.deltaTime, ForceMode.Acceleration);
        }

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
