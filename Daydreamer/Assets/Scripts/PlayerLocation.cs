using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocation : MonoBehaviour
{
    // maybe get gameobject name of whatever collider this hits and store here??
    [HideInInspector]
    public bool isEnter = false;
    [HideInInspector]
    public bool isExit = false;

    // ^should these even be public lol, better way to acces?

    private int layerIndex;

    private void Start() {
        layerIndex = LayerMask.NameToLayer("Exit");
    }

    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.layer == layerIndex) {
            Debug.Log("entered collider!!");
            isEnter = true;
            isExit = false;
        }
    }

    private void OnTriggerExit(Collider col) {
        if (col.gameObject.layer == layerIndex) {
            Debug.Log("exited collider!!");
            isExit = true;
            isEnter = false;
        }
    }

}
