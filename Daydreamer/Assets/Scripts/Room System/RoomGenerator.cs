using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    private GameObject currentRoom;         // current room
    private GameObject nextRoom;            // next room to activate
    private Vector3 moveAmount;             // amount to move generator gameobj

    public GameObject player;               // PLAYER
    private Collider playerCol;

    public Transform[] startingPositions;
    public GameObject[] rooms;              // make sure you're using the prefab rooms with setup, not just plain fbxs
    public LayerMask roomMask;              // makes sure only detect rooms and not other objs w/colliders

    private int activeNum;                  // number of active rooms
    private int activeMax = 4;              // max number of active rooms allowed
    private List<GameObject> activeRooms;   // list of rooms visited that are currently active??
                                            // activeRooms.First() should be current room

    private void Start()
    {
        // Setup starting position (only one rn)
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;

        // Setup first room
        currentRoom = rooms[0];
        Instantiate(currentRoom, transform.position, Quaternion.identity);

        activeRooms = new List<GameObject>();
        activeRooms.Add(currentRoom);
        activeNum = 1;

        // Setup player colliders
        playerCol = player.transform.GetComponent<Collider>();
    }

    private void Update()
    {
        /*
            if in OverlapSphere array (for over a second?)
                DissolveIn() the new room
                if linger for more than 5 sec
                    DissolveOut() the old room
                    RoomSetup() - set new room as current room
                                - randomize and generate new rooms

            if currentroom is not in collider array
                currentroom.cleanup()

        */


        // Vector3 offset = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        // Collider[] roomDetection = Physics.OverlapSphere(player.transform.position, 1, roomMask);
        // Debug.Log(roomDetection.Length + " rooms detected");
        // foreach(var col in roomDetection) {
        //     Debug.Log(" TYPE: " + col.gameObject.name);
        // }
        //roomDetection[0].GetComponent<RoomData>().Cleanup();
    }


    private void RoomSetup()
    {

    }

    // DEBUG; for room detection sphere
    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.cyan;
    //     Gizmos.DrawWireSphere(transform.position, 1);
    // }

}
