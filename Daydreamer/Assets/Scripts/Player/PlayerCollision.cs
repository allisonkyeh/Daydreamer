using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCollision : MonoBehaviour
{
    public GameObject roomManager;

    public bool playerInside;
    private Collider playerCol;

    private int layerIndex => LayerMask.NameToLayer("Room");

    private void Awake() {
        playerCol = this.gameObject.transform.GetChild(0).GetComponent<Collider>();
        playerCol.isTrigger = true;


    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == layerIndex) {
            Debug.Log("entered collider!!");
            playerInside = true;

            Room r = other.gameObject.GetComponent<Room>();
            roomManager.GetComponent<RoomManager>().Setup(r);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == layerIndex) {
            Debug.Log("exited collider!!");
            playerInside = false;

        }
    }
}
