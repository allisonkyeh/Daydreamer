using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerCollision : MonoBehaviour
{
    public GameObject   roomManager;
    private GameObject  rmRoot;
    private Room        rm;
    private Collider    playerCol;
    private int         roomLayer => LayerMask.NameToLayer("Room");
    private int         touchLayer => LayerMask.NameToLayer("Touchable");
    private bool        isRmStart;

    public GameObject   ikTarget;
    public GameObject   ikWeight;
    Rig rig = null;

    private void Awake() {
        // playerCol = this.gameObject.transform.GetChild(0).GetComponent<Collider>();
        // playerCol.isTrigger = true;

        rig = ikWeight.GetComponent<Rig>();
        rig.weight = 0;
    }

    void FixedUpdate()
    {
        RaycastHit hit;

        // Does the ray intersect any objects excluding the player layer
        // transform.TransformDirection(Vector3.forward) for local
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, ~touchLayer))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
            Debug.Log("HIT!!!!!!!!!");

            ikTarget.transform.position = hit.point;
            rig.weight = 1;

        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 1000, Color.white);
            Debug.Log("Did not Hit");

            rig.weight = 0;
            ikTarget.transform.localPosition = new Vector3(0,0,0);
        }
    }

    /******* CHEATED HAND INTERACTION *******/

    // private void OnTriggerEnter(Collider other) {
    //     Debug.Log("OnTriggerEnter: PlayerCollision");

    //     Vector3 location = rightIK.gameObject.transform.position;

    //     Vector3 closestPoint = other.ClosestPoint(location);

    //     if (other.gameObject.layer == touchLayer) {
    //         rightIK.transform.position = closestPoint;
    //         rightIK.transform.Rotate(-60.0f, -30.0f, 90.0f, Space.Self);
    //         rig.weight = 1;
    //     }

    //     // Gizmos.DrawSphere(location, 0.1f);
    //     // Gizmos.DrawWireSphere(closestPoint, 0.1f);
    // }

    // private void OnTriggerStay(Collider other) {
    //     Vector3 location = rightIK.gameObject.transform.position;

    //     Vector3 closestPoint = other.ClosestPoint(location);

    //     if (other.gameObject.layer == touchLayer) {
    //         rightIK.transform.position = closestPoint;
    //         // rightIK.transform.Rotate(-60.0f, -30.0f, 90.0f, Space.Self);
    //         rig.weight = 1;
    //     }
    // }

    // private void OnTriggerExit(Collider other) {
    //     // Debug.Log("OnTriggerExit");
    //     rig.weight = 0;
    //     rightIK.transform.localPosition = new Vector3(0,0,0);
    //     rightIK.transform.Rotate(60.0f, 30.0f, -90.0f, Space.Self);
    // }


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
