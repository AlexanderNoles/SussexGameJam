using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagement
{
    public static bool GetKey(KeyCode inputKey)
    {
        return inputKey != KeyCode.Alpha0 && Input.GetKey(inputKey);
    }
}
