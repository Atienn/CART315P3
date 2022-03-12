using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMovement2D : MovementControl
{

    public override Vector3 GetDirection()
    {
        return new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    public override bool GetAction1()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public override bool GetAction2() {
        return Input.GetKeyDown(KeyCode.LeftShift);
    }
}
