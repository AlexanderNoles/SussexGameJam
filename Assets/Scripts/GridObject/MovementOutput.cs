using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementOutput : GridObject
{
    private InputReciever target;

    public enum OutputToChange
    {
        Up,
        Down,
        Left,
        Right,
        RotateLeft,
        RotateRight
    }
    public OutputToChange outputToChange;

    protected override void Start()
    {
        RepairScreenManagement.AddGridObject(transform.localPosition, this, true);
    }

    public bool RegisterInput(InputReciever reciever)
    {
        if(target == null)
        {
            target = reciever;

            if((int)outputToChange < 4)
            {
                //Orthagonal Movement
                RepairScreenManagement.UpdateOrthagonalKeyCode((int)outputToChange, target.keyCode);
            }
            else
            {
                //Rotatational Movement
                RepairScreenManagement.UpdateRotationalKey((int)outputToChange - 4, target.keyCode);
            }

            return true;
        }

        return false;
    }

    public void UnRegisterInput()
    {
        if(target != null)
        {
            if((int)outputToChange < 4)
            {
                //Orthagonal Movement
                RepairScreenManagement.UpdateOrthagonalKeyCode((int)outputToChange, RepairScreenManagement.NullInput);
            }
            else
            {
                //Rotatational Movement
                RepairScreenManagement.UpdateRotationalKey((int)outputToChange - 4, RepairScreenManagement.NullInput);
            }
        }
    }
}
