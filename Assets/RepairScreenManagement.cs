using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairScreenManagement : MonoBehaviour
{
    private static RepairScreenManagement _instance;

    private void Awake() {
        _instance = this;
    }

    public const KeyCode NullInput = KeyCode.Alpha0;

    [Header("Orthagonal Input")]
    public List<KeyCode> upDownLeftRight = new List<KeyCode>();

    private static Vector2[] orthagonalDirections = new Vector2[4]
    {
        new Vector2(0, 1),
        new Vector2(0, -1),
        new Vector2(-1, 0),
        new Vector2(1, 0)
    };

    [Header("Rotational Input")]
    public KeyCode LeftRotationalInput = NullInput;
    public KeyCode RightRotationalInput = NullInput;

    public static Vector2 GetOrthagonalInput()
    {
        Vector2 toReturn = new Vector2();

        for(int i = 0; i < orthagonalDirections.Length; i++)
        {
            if(InputManagement.GetKey(_instance.upDownLeftRight[i]))
            {
                toReturn += orthagonalDirections[i];
            }
        }

        return toReturn;
    }

    public static float GetRotationalInput()
    {
        float toReturn = 0.0f;

        if(InputManagement.GetKey(_instance.LeftRotationalInput))
        {
            toReturn += 1.0f;
        }

        
        if(InputManagement.GetKey(_instance.RightRotationalInput))
        {
            toReturn -= 1.0f;
        }

        return toReturn;
    }
}
