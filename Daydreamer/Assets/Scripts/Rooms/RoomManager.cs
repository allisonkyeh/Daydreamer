using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    /*** PLAYER ***/
    public GameObject   player;
    // Possible player starting positions
    public Transform[]  startingPositions;

    /*** ROOM DATA ***/
    // Amount to move generator gameobj
    private GameObject  currentRoom;
    private GameObject  prevRoom;
    private GameObject  nextRoom;

    // List of all rooms; make sure using the prefabs w/setup not fbxs
    public List<GameObject> roomsAll;
    public int roomsTotalNumber => roomsAll.Count;

    // Makes sure only detect rooms and not other colliders
    public int roomMask => LayerMask.NameToLayer("Room");

    // Number of active rooms
    private int activeNum = 0;
    // Max number of active rooms allowed
    private int activeMax = 4;
    // List of rooms visited that are currently active
    private List<GameObject> roomsActive;
    // roomsActive.First() should be current room

    // List of room colliders detected from OverlapSphere
    private Collider[] roomsDetected;

    private void Start()
    {
        // Setup starting position (only one rn)
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position  = startingPositions[randStartingPos].position;

        // Setup first starting room
        currentRoom = roomsAll[0];
        Instantiate(currentRoom, transform.position, Quaternion.identity);

        FillDoors();

        roomsActive = new List<GameObject>(); roomsActive.Add(currentRoom);
        activeNum   = 1;
    }

    private void Update()
    {
        // roomsDetected = Physics.OverlapSphere(player.transform.position, 1, roomMask);

        // Debug.Log(roomDetection.Length + " rooms detected");
        // foreach(var col in roomDetection) {
        //     Debug.Log(" TYPE: " + col.gameObject.name);
        // }
    }

    /*** Runs everytime the player enters a new room;
                randomize and fill door list in room ***/
    private void FillDoors()
    {
        int randRoom = 0;
        int randEntrance = 0;
        Vector3 newOffset;
        Transform currentOffset;
        Transform nextOffset;

        foreach (GameObject door in currentRoom.GetComponent<Room>().doors) {
            randRoom = Random.Range(1, roomsTotalNumber);   // int min inclusive, max exclusive
            currentOffset = door.transform;

            nextRoom = roomsAll[randRoom];
            randEntrance = Random.Range(1, nextRoom.GetComponent<Room>().maxDoors);
            nextOffset = nextRoom.GetComponent<Room>().doors[randEntrance].transform;

            newOffset = currentOffset.position - nextOffset.position;
            // switch quaternion with transform.rotate
            Instantiate(nextRoom, newOffset, Quaternion.identity);
            roomsActive.Add(nextRoom);
            activeNum++;
        }
        Debug.Log("FillDoors() finished, all room doors should be loaded but invisible ");
    }

    /*** Sets up a room the player is going into ***/
    public void Setup(Room r)
    {
        if (r.isVisible == false && r.isDissolvingIn == false) {
            r.DissolveIn();
        }
        /*
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
    }

    /*** Cleans up a room the player is leaving ***/
    public void Cleanup()
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
