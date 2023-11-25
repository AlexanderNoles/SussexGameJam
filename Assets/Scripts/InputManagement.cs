using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagement
{
    public static bool GetKey(KeyCode inputKey)
    {
        return inputKey != KeyCode.Alpha0 && Input.GetKey(inputKey);
    }

    public static bool GetKeyDown(KeyCode inputKey)
    {
        return Input.GetKeyDown(inputKey);
    }

    public static bool GetMouseButtonDown(int mouseIndex)
    {
        return Input.GetMouseButtonDown(mouseIndex);
    }

    public static bool GetMouseButton(int mouseIndex)
    {
        return Input.GetMouseButton(mouseIndex);
    }
}
