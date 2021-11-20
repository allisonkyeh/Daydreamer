using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCollision : MonoBehaviour
{
    // referencing TC PlayerBodyCollisionTrigger
    public UnityEvent OnPlayerEnter;
    public UnityEvent OnPlayerExit;
    public bool playerInside;
    private Collider playerCol;

    private int layerIndex => LayerMask.NameToLayer("Exit");

    private void Awake() {
        playerCol = this.gameObject.transform.GetChild(0).GetComponent<Collider>();
        playerCol.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == layerIndex) {
            Debug.Log("entered collider!!");
            playerInside = true;
            if (OnPlayerEnter != null) {
                OnPlayerEnter.Invoke();
                // Invoke Room.Setup()
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == layerIndex) {
            Debug.Log("exited collider!!");
            playerInside = false;
            if (OnPlayerExit != null) {
                OnPlayerExit.Invoke();
                // Invoke Room.Cleanup()
            }
        }
    }
}
