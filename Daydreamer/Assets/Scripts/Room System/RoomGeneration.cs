using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Modified LevelGeneration

public class RoomGeneration : MonoBehaviour
{
    private GameObject currentRoom;         // keeps track of current room, for offset
    private GameObject nextRoom;            // next room to activate
    private Vector3 moveAmount;             // amount to move generator gameobj

    private int downCounter;

    public GameObject player;               // PLAYER
    private Collider playerCol;

    public Transform[] startingPositions;
    public GameObject[] rooms;              // make sure you're using the prefab rooms with setup, not just the plain fbxs

    private int activeNum;                  // number of active rooms
    private int activeMax = 2;              // max number of active rooms allowed
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
        activeRooms.Add(currentRoom);
        activeNum = 1;

        // Setup player colliders
        playerCol = player.transform.GetComponent<Collider>();
    }

    private void Update()
    {
        /*
            if Player collider detects an exit collider, move and generate room
            need to isolate to col_exit only, maybe layer mask or check name?
        */

        // if triggered an exit and if only one (current) room is active
        if(player.GetComponent<PlayerLocation>().isEnter == true && activeNum < activeMax) {
            Move();
            currentRoom = nextRoom;
        }

        if(player.GetComponent<PlayerLocation>().isExit == true && activeNum > 0) {
            /*  but only want to deactivate current room if player moved onto next room..
                otherwise if they walk into the transition area but backtrack to current room,
                should unload the next room.
                wha... possible ideas:
                    - tracking whether player transform is within room bounds; marker trigger loads in a random room,
                        but if they leave the bounds then that loads it out; could get complex since have to do in world space..
                    - another collider.isTrigger.. would have to get prefab root of respective collider to load it out;
                        would also need collider trigger that covers entire prefab- eh. not good.
            */
            activeRooms.RemoveAt(0);
            activeNum--;
        }
    }

    private void Move()
    {
        // Random number for next room
        int nextRoomNum = Random.Range(1, 3);   // int min inclusive, max exclusive
        nextRoom = rooms[nextRoomNum];

        // offset of current room exit
        Transform currentOffset = currentRoom.GetComponent<RoomType>().exitMarker[0].transform;
        // Debug.Log("currentOffset: " + currentOffset.position);

        // offset of next room entrance
        Transform nextOffset = nextRoom.GetComponent<RoomType>().entranceMarker.transform;
        // Debug.Log("nextOffset: " + nextOffset.position);

        // currentOffset spawns prefab pivot at exit. nextOffset offsets prefab to entrance.
        moveAmount = currentOffset.position - nextOffset.position;
        transform.position += moveAmount;       // moves RoomGenerator

        /*
            but this is if the prefab pivots are in the center
            should this be adjusted so pivots are at entrance? I mean.. prob not that different..
            plus this way might make it easier to randomize multiple entrances/exits for one room
        */

        Instantiate(nextRoom, transform.position, Quaternion.identity);
        activeRooms.Add(nextRoom);
        activeNum++;
        // Debug.Log("nextRoom INSTANTIATED!!!!");
    }

    // DEBUG; for room detection sphere
    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.cyan;
    //     Gizmos.DrawWireSphere(transform.position, 1);
    // }

}
