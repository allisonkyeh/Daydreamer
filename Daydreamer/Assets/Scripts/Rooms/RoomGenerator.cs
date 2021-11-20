using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    /*** PLAYER ***/
    public GameObject   player;
    // Possible player starting positions
    public Transform[]  startingPositions;

    /*** ROOM DATA ***/
    // Amount to move generator gameobj
    private Vector3     moveAmount;
    private GameObject  currentRoom;
    private GameObject  prevRoom;
    private GameObject  nextRoom;

    // List of all rooms; make sure using the prefabs w/setup not fbxs
    public List<GameObject> roomsAll;
    // Makes sure only detect rooms and not other colliders
    public int roomMask => LayerMask.NameToLayer("Room");

    // Number of active rooms
    private int activeNum = 0;
    // Max number of active rooms allowed
    private int activeMax = 4;
    // List of rooms visited that are currently active
    private List<GameObject> roomsActive;
    // activeRooms.First() should be current room

    private void Start()
    {
        // Setup starting position (only one rn)
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position  = startingPositions[randStartingPos].position;

        // Setup first starting room
        currentRoom = roomsAll[0];
        Instantiate(currentRoom, transform.position, Quaternion.identity);

        roomsActive = new List<GameObject>(); roomsActive.Add(currentRoom);
        activeNum   = 1;
    }

    private void Update()
    {
        /*
            Collider[] roomsDetected = Physics.OverlapSphere(player.transform.position, 1, roomMask);

            foreach (Collider col in roomsDetected)
                set active          (Room.DissolveIn() the new room)
                if linger for more than 5 sec (coroutines?)
                    set inactive    (Room.DissolveOut() the old room)
                    RoomGenerator.RoomSetup()
                else (left)
                    set inactive    (Room.DissolveOut() the new room)

            if currentroom is not in collider array
                currentroom.cleanup()
        */

        // moveAmount = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        // Debug.Log(roomDetection.Length + " rooms detected");
        // foreach(var col in roomDetection) {
        //     Debug.Log(" TYPE: " + col.gameObject.name);
        // }
        //roomDetection[0]
    }

    private void Setup()
    {
        /*
            - currentRoom.GetComponent<Room>().Cleanup(); (destroys room)
            - set new room as current room
            - randomize and generate new rooms
        */
    }

    // DEBUG; for room detection sphere
    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.cyan;
    //     Gizmos.DrawWireSphere(transform.position, 1);
    // }

}
