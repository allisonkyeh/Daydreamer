using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCollision : MonoBehaviour
{
    public GameObject   roomManager;
    private GameObject  rmRoot;
    private Room        rm;
    private Collider    playerCol;
    private int         layerIndex => LayerMask.NameToLayer("Room");
    private bool        isRmStart;


    private void Awake() {
        playerCol = this.gameObject.transform.GetChild(0).GetComponent<Collider>();
        // playerCol.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("OnTriggerEnter: PlayerCollision");
        rmRoot = other.gameObject.transform.root.gameObject;
        isRmStart = (rmRoot.name == "prm_start(Clone)");
        if (other.gameObject.layer == layerIndex && !isRmStart) {
            rm = rmRoot.GetComponent<Room>();
            roomManager.GetComponent<RoomManager>().Setup(rm);
        }
    }

    private void OnTriggerExit(Collider other) {
        // Debug.Log("OnTriggerExit");
        rmRoot = other.gameObject.transform.root.gameObject;
        if (other.gameObject.layer == layerIndex) {
            rm = rmRoot.GetComponent<Room>();
            roomManager.GetComponent<RoomManager>().Cleanup(rm);
        }
    }

    
    /******* OLD ROOM SYSTEM *******/

    // private void Awake() {
    //     playerCol = this.gameObject.transform.GetChild(0).GetComponent<Collider>();
    //     // playerCol.isTrigger = true;
    // }

    // private void OnTriggerEnter(Collider other) {
    //     Debug.Log("OnTriggerEnter: PlayerCollision");


    //     rmRoot = other.gameObject.transform.root.gameObject;
    //     isRmStart = (rmRoot.name == "prm_start(Clone)");
    //     if (other.gameObject.layer == layerIndex && !isRmStart) {
    //         rm = rmRoot.GetComponent<Room>();
    //         roomManager.GetComponent<RoomManager>().Setup(rm);
    //     }
    // }

    // private void OnTriggerExit(Collider other) {
    //     // Debug.Log("OnTriggerExit");

    //     rmRoot = other.gameObject.transform.root.gameObject;
    //     if (other.gameObject.layer == layerIndex) {
    //         rm = rmRoot.GetComponent<Room>();
    //         roomManager.GetComponent<RoomManager>().Cleanup(rm);
    //     }
    // }
}
