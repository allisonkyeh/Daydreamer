using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocation : MonoBehaviour
{
    // maybe get gameobject name of whatever collider this hits and store here??
    
    private void OnTriggerEnter(Collider col) {
        Debug.Log("hit detected!!");
    }
}
