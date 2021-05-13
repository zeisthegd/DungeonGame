using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementController
{
    protected Animator animator;
    protected Transform objectTrf;
    protected Rigidbody rb2d;
    protected MovementSettings settings;

    public MovementController() { }
    public MovementController(Transform objectTransform, Rigidbody rb2d, Animator animator, MovementSettings settings)
    {
        this.objectTrf = objectTransform;
        this.rb2d = rb2d;
        this.settings = settings;
        this.animator = animator;
        ResetState();
    }

    public virtual void HandleInput()
    {
        HandleAnimationTransition();
    }

    public abstract void HandleAnimationTransition();

    public abstract void ResetState();
}
