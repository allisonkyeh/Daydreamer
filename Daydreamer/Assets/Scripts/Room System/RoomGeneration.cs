using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Modified LevelGeneration

public class RoomGeneration : MonoBehaviour
{
    private GameObject currentRoom;         // current room
    private GameObject nextRoom;            // next room to activate
    private Vector3 moveAmount;             // amount to move generator gameobj

    public GameObject player;               // PLAYER
    private Collider playerCol;

    public Transform[] startingPositions;
    public GameObject[] rooms;              // make sure you're using the prefab rooms with setup, not just the plain fbxs

    private int activeNum;                  // number of active rooms
    private int activeMax = 4;              // max number of active rooms allowed
    private List<GameObject> activeRooms;   // list of rooms visited that are currently active??
                                            // activeRooms.First() should be current room

    private void Start()
    {
        // picks one random starting position if multiple, maybe get rid of this if everyone starts at same place
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;

        // Setup first room
        currentRoom = rooms[0];
        Instantiate(currentRoom, transform.position, Quaternion.identity);

        activeRooms = new List<GameObject>();
        activeRooms.Add(currentRoom);
        activeNum = 1;

        Debug.Log("Start() " + activeNum);


        // Setup player colliders
        playerCol = player.transform.GetComponent<Collider>();
    }

    private void Update()
    {
        /* if Player collider detects an exit collider, move and generate room
            need to isolate to col_exit only, maybe layer mask or check name?
        */

        // if triggered an exit and if only one (current) room is active
        if(player.GetComponent<PlayerLocation>().isEnter == true && activeNum < activeMax) {
            Move();
            player.GetComponent<PlayerLocation>().isEnter = false;
            Debug.Log("Update() isEnter " + activeNum);

        }

        /* isTrigger will detect both the current exit marker and the entrance marker when it pops up.. double the signals?
        */

        // if(player.GetComponent<PlayerLocation>().isExit == true && activeNum > 0) {

        //     // maybe easier to have 'prev' room
        //     Debug.Log("Update() isExit " + activeNum);

        //     currentRoom.SetActive(false);
        //     currentRoom = nextRoom;
        //     nextRoom = null;
        //     activeRooms.RemoveAt(0);
        //     activeNum--;

        // }
    }


    private void Move()
    {
        // Random number for next room
        int nextRoomNum = Random.Range(1, 3);   // int min inclusive, max exclusive
        nextRoom = rooms[nextRoomNum];

        // offset of current room exit
        Transform currentOffset = currentRoom.GetComponent<RoomType>().exitMarker[0].transform;

        // offset of next room entrance
        Transform nextOffset = nextRoom.GetComponent<RoomType>().entranceMarker.transform;

        // currentOffset spawns prefab pivot at exit. nextOffset offsets prefab to entrance.
        moveAmount = currentOffset.position - nextOffset.position;
        transform.position += moveAmount;       // moves RoomGenerator

        /*
            but this is if the prefab pivots are in the center
            should this be adjusted so pivots are at entrance? I mean.. prob not that different..
            plus this way might make it easier to randomize multiple entrances/exits for one room
        */

        activeRooms.Add(nextRoom);
        Instantiate(nextRoom, transform.position, Quaternion.identity);
        activeNum++;
        Debug.Log("Move() " + activeNum);
    }

    // DEBUG; for room detection sphere
    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.cyan;
    //     Gizmos.DrawWireSphere(transform.position, 1);
    // }

}
