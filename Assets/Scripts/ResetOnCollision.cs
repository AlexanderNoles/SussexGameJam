using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOnCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player")
        {
            GameManagement.RestartLevel();
        }
    }
}
