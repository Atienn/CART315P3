using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Misc;

[RequireComponent(typeof(MovementControl))]
[RequireComponent(typeof(CharacterController))]
public class Runner : Entity
{
    #region Movement

    MovementControl input;
    CharacterController charCtrl;

    [Header("General")]
    [SerializeField] float speed = 4;
    [SerializeField] float gravity = -3;
    [SerializeField] float jumpForce = 55;


    //Enables strategy design pattern.
    VoidStrategy mvmtMode;
    VoidStrategy jumpMode;

    byte dashTimer;

    Vector3 dirInput;
    Vector3 velocity;


    void Start()
    {
        input = GetComponent<MovementControl>();
        charCtrl = GetComponent<CharacterController>();

        mvmtMode = AirMvmt;
    }

    void Update()
    {
        //Get direction to move runner in.
        dirInput = input.GetDirection();

        //Action 1 is jump.
        if(charCtrl.isGrounded && input.GetAction1()) {
            velocity.y = jumpForce;
            mvmtMode = AirMvmt;
        }
    }

    void FixedUpdate() { mvmtMode(); }


    #region Movement Modes

    void GroundMvmt()
    {
        if (!charCtrl.isGrounded) {
            mvmtMode = AirMvmt;
            AirMvmt(); return;
        }

        //Compute friction.
        velocity.x = Mathf.MoveTowards(charCtrl.velocity.x, 0f, 0.075f * Mathf.Abs(charCtrl.velocity.x) + 0.5f);

        if(dirInput != Vector3.zero) {
            velocity += new Vector3(dirInput.x, 0) * speed;
        }

        charCtrl.Move(velocity * Time.fixedDeltaTime);
    }

    void AirMvmt()
    {
        if (charCtrl.isGrounded && velocity.y < 0) {
            //Lower vertical velocity to a neglegible amount. 
            //Cannot be 0 as the character controller's 'isGrounded' stops working properly otherwise.
            velocity.y = -0.1f;

            //If now grounded, switch to grounded mode.
            mvmtMode = GroundMvmt;
            GroundMvmt(); return;
        }

        //Compute friction. (even mid-air)
        velocity.x = Mathf.MoveTowards(charCtrl.velocity.x, 0f, 0.075f * Mathf.Abs(charCtrl.velocity.x) + 0.5f);

        //If not grounded, add gravity. Doubled if holding down.
        velocity.y += (dirInput.y < 0 ? 2 : 1) * gravity;

        if (dirInput != Vector3.zero) {
            velocity += new Vector3(dirInput.x, 0) * speed;
        }


        //Move the character.
        charCtrl.Move(velocity * Time.fixedDeltaTime);
    }

    #endregion

    #endregion


    #region Combat


    #endregion
}