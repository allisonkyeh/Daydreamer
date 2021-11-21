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
    private int currentRoomNumber => currentRoom.GetComponent<Room>().roomNumber;

    // List of all rooms; make sure using the prefabs w/setup not fbxs
    public List<GameObject> roomsAll;
    public int roomsTotalNumber => roomsAll.Count;

    /*** MAYBE don't need this stuff ***/
    // Number of active rooms
    private int activeNum = 0;
    // Max number of active rooms allowed
    private int activeMax = 4;
    // List of rooms visited that are currently active
    private List<GameObject> roomsActive;
    // roomsActive.First() should be current room

    private void Start()
    {
        // Set up starting position (only one rn) and first starting room
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position  = startingPositions[randStartingPos].position;

        currentRoom = roomsAll[0];
        Instantiate(currentRoom, transform.position, Quaternion.identity);

        FillDoors();

        roomsActive = new List<GameObject>(); roomsActive.Add(currentRoom);
        activeNum   = 1;
    }

    private void Update()
    {

    }

    /*** Runs everytime the player enters a new room;
                randomize and fill doors to other rooms ***/
    private void FillDoors()
    {
        int randRoomNumber = 0;
        int randEntrance = 0;
        Vector3     newPos;
        Quaternion  newRot;
        Vector3     currentPos;
        Quaternion  currentRot;
        Vector3     nextPos;
        Quaternion  nextRot;

        foreach (GameObject door in currentRoom.GetComponent<Room>().doors) {
            currentPos = door.transform.position;
            currentRot = door.transform.rotation;

            // randomize new room to generate; Range int is: minInclusive, maxExclusive
            do randRoomNumber = Random.Range(1, roomsTotalNumber);
            while (randRoomNumber == currentRoomNumber);
            nextRoom = roomsAll[randRoomNumber];

            // picks random entrance/door of the new room
            randEntrance = Random.Range(1, nextRoom.GetComponent<Room>().maxDoors);
            nextPos = nextRoom.GetComponent<Room>().doors[randEntrance].transform.position;
            nextRot = nextRoom.GetComponent<Room>().doors[randEntrance].transform.rotation;

            // new offset to line up the new room to the current door
            newPos = currentPos - nextPos;
            newRot = currentRot * Quaternion.Inverse(nextRot); // essentially subtracts

            // generates new room
            Instantiate(nextRoom, newPos, newRot);
            // roomsActive.Add(nextRoom);
            // activeNum++;
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

        /*
            - set new room as current room
            - randomize and generate new rooms
        */
    }

    /*** Cleans up a room the player is leaving ***/
    public void Cleanup(Room r)
    {
        if (r.isVisible == true && r.isDissolvingIn == false) {
            r.DissolveOut();
        }
        // need case for if r is currently dissolving in, switching to dissolving out
    }

    // DEBUG; for room detection sphere
    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.cyan;
    //     Gizmos.DrawWireSphere(transform.position, 1);
    // }

}
