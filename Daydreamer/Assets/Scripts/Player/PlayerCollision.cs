using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCollision : MonoBehaviour
{
    public GameObject   roomManager;
    private Collider    playerCol;
    private int layerIndex  => LayerMask.NameToLayer("Room");
    private bool isPrmStart;

    private void Awake() {
        playerCol = this.gameObject.transform.GetChild(0).GetComponent<Collider>();
        playerCol.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (this.gameObject.transform.root.name == "prm_start") {
            isPrmStart = true;
        } else {
            isPrmStart = false;
        }
        if (other.gameObject.layer == layerIndex && !isPrmStart) {
            Debug.Log("entered collider!!");
            Room r = other.gameObject.transform.root.gameObject.GetComponent<Room>();
            roomManager.GetComponent<RoomManager>().Setup(r);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == layerIndex) {
            Debug.Log("exited collider!!");
            Room r = other.gameObject.transform.root.gameObject.GetComponent<Room>();
            roomManager.GetComponent<RoomManager>().Cleanup(r);
        }
    }
}
