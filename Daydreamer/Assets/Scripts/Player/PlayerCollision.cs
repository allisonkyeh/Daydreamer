using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerCollision : MonoBehaviour
{
    private GameObject  vignetteRoot;
    private Vignette    v;
    // private bool        isStart;

    private Collider    playerCol;
    private int         vignetteLayer => LayerMask.NameToLayer("Vignette");
    // private int         touchLayer => LayerMask.NameToLayer("Touchable");
    // public GameObject   ikTarget;
    // public GameObject   ikWeight;
    // Rig rig = null;

    private PlayerShell p;

    private void Awake() {
        playerCol = this.gameObject.transform.GetChild(0).GetComponent<Collider>();
        // playerCol = this.gameObject.GetComponent<Collider>();
        playerCol.isTrigger = true;

        p = this.gameObject.GetComponent<PlayerShell>();

        // rig = ikWeight.GetComponent<Rig>();
        // rig.weight = 0;
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("OnTriggerEnter: PlayerCollision " + other.name);

        if (other.gameObject.layer == vignetteLayer){
            vignetteRoot = other.gameObject.transform.root.gameObject;
            v = vignetteRoot.GetComponent<Vignette>();
            switch (v.temp) {
                case Vignette.Temperature.Cold:
                    p.corrupting = true;
                    break;
                case Vignette.Temperature.Neutral:
                    p.corrupting = false;
                    break;
                case Vignette.Temperature.Warm:
                    p.corrupting = false;
                    break;
                default:
                    p.corrupting = true;
                    break;
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        // Debug.Log("OnTriggerExit: PlayerCollision");
        // FIXME: it doesn't start corrupting? also check playershell conditions
        if (other.gameObject.layer == vignetteLayer){
            p.corrupting = true;
        }
    }

}

    /********** RAYCAST INTERACTION **********/
    // void FixedUpdate()
    // {
    //     RaycastHit hit;
    //     // Does the ray intersect any objects excluding the player layer
    //     // transform.TransformDirection(Vector3.forward) for local
    //     if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, ~touchLayer))
    //     {
    //         Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
    //         Debug.Log("HIT!!!!!!!!!");
    //         ikTarget.transform.position = hit.point;
    //         rig.weight = 1;
    //     }
    //     else
    //     {
    //         Debug.DrawRay(transform.position, transform.forward * 1000, Color.white);
    //         Debug.Log("Did not Hit");
    //         rig.weight = 0;
    //         ikTarget.transform.localPosition = new Vector3(0,0,0);
    //     }
    // }

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
    //     vignetteRoot = other.gameObject.transform.root.gameObject;
    //     = (vignetteRoot.name == "prm_start(Clone)");
    //     if (other.gameObject.layer == layerIndex && ) {
    //         rm = vignetteRoot.GetComponent<Room>();
    //         roomManager.GetComponent<RoomManager>().Setup(rm);
    //     }
    // }
    // private void OnTriggerExit(Collider other) {
    //     // Debug.Log("OnTriggerExit");
    //     vignetteRoot = other.gameObject.transform.root.gameObject;
    //     if (other.gameObject.layer == layerIndex) {
    //         rm = vignetteRoot.GetComponent<Room>();
    //         roomManager.GetComponent<RoomManager>().Cleanup(rm);
    //     }
    // }

