using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: worry about this class later lol
public class VignetteManager : MonoBehaviour
{
    /*** PLAYER ***/
    public GameObject   player;
    // Possible player starting positions
    public Transform[]  startingPositions;

    /*** ROOM TRACKING ***/
    private GameObject  currentRoom;
    private GameObject  prevRoom;
    private GameObject  nextRoom;
    private int currentRoomNumber => currentRoom.GetComponent<Room>().roomNumber;

    // List of all rooms; make sure using the prefabs w/setup not fbxs
    public List<GameObject> roomsAll;
    public int roomsTotalNumber => roomsAll.Count;

    // List of rooms visited that are currently active
    // roomsActive.First() should be current room
    private List<GameObject> roomsActive;

    private void Start()
    {
        // Set up starting position (only one rn) and first starting room
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position  = startingPositions[randStartingPos].position;

        GameObject startRm = (GameObject) Instantiate(roomsAll[0], transform.position, Quaternion.identity);
        currentRoom = startRm;

        FillDoors();

        roomsActive = new List<GameObject>();
        roomsActive.Add(startRm);
    }

    private void Update()
    {

    }

    /*** Runs everytime the player enters a new room;
                randomize and fill doors to other rooms ***/
    private void FillDoors()
    {
        int randRoomNum = 0;
        int randEntranceNum = 0;
        GameObject randEntrance;
        Vector3     newPos;
        Vector3     currentPos;
        Vector3     nextPos;

        Debug.Log("filling doors for " + currentRoom.name);
        foreach (GameObject door in currentRoom.GetComponent<Room>().doors) {
            currentPos = door.transform.position;

            // Randomize new room to generate; Range int is: minInclusive, maxExclusive
            // Range starts from 1 to exclude rm_start
            do randRoomNum = Random.Range(1, roomsTotalNumber);
            while (randRoomNum == currentRoomNumber);
            nextRoom = roomsAll[randRoomNum];
        
            // Picks random entrance/door of the new room; once per new room
            randEntranceNum = Random.Range(0, nextRoom.GetComponent<Room>().maxDoors);
            randEntrance = nextRoom.GetComponent<Room>().doors[randEntranceNum];
            // TODO: can't fill door that you are entering the room from,
            // since the prev room is still there. Maybe only fill that room
            // during Cleanup? Also, store the 'lineup' door in the room, so it can be
            // skipped when doing fillDoors on that room (if player enters)

            // New position offset to line up the new room to the current door
            nextPos = randEntrance.transform.position;
            newPos = currentPos - nextPos;

            // Generates new room
            GameObject n = (GameObject) Instantiate(nextRoom, newPos, Quaternion.identity);

            // FIXME:
            Debug.Log("adding to doorsRooms: " + n.name);
            currentRoom.GetComponent<Room>().doorsRooms.Add(n);

            // WHEN SETTING UP DOORS
            // pull left, from bottom of rotation gizmo (from top view, z up)
            // aka clockwise so every rot is positive

            // Instance n entrance transform; rotation
            Transform nextEntrance = n.GetComponent<Room>().doors[randEntranceNum].transform;
            Transform currentExit = door.transform;

            float nextAngle = nextEntrance.eulerAngles.y;
            float currentAngle = currentExit.eulerAngles.y;

            // rotating around nextEntrance
            n.transform.RotateAround(nextEntrance.position, Vector3.up, (currentAngle - nextAngle + 180));

            // roomsActive.Add(nextRoom);

            // TODO: case to prevent generating duplicate rooms
        }
        // Debug.Log("FillDoors() finished, all room doors should be loaded and invisible");
    }

    /*** Sets up a room the player is going into ***/
    public void Setup(Room r)
    {
        if (r.isDissolvedIn == false && r.isDissolvingIn == false) {
            r.DissolveIn();
            nextRoom = r.gameObject;

            // nextRoom.DissolveIn()
            // currentRoom.DissolveOut();

        }
        // TODO: if finished dissolving in (this means player stayed in for full dissolve
        // duration since exiting should switch to dissolveOut)
        // TODO: switch currentRoom (do this in cleanup?) and filldoors()
        // oh, filldoors in setup even if they dont go in (currently in Cleanup)
    }

    /*** Cleans up a room the player is leaving ***/
    public void Cleanup(Room r)
    {
        if (r.isDissolvedIn == true && r.isDissolvingIn == false) {

            List<GameObject> neighbors = r.GetComponent<Room>().doorsRooms;
            // Debug.Log("Neighbors: " + neighbors.Count);
            foreach (GameObject n in neighbors){
                if (n == nextRoom) {
                    Debug.Log("skipping " + n.name);
                    continue;
                }
                Destroy(n);
                Debug.Log(n.name + " destroyed.");

                // FIXME: MissingReferenceException? bc destroyed here, but ontriggerexit being hit
            }
            neighbors.Clear();
            // Debug.Log("Neighbors Cleared: " + neighbors.Count);

            r.DissolveOut();


            prevRoom = currentRoom;
            currentRoom = nextRoom;

            // Debug.Log("current room: " + currentRoom.name);

            FillDoors();

            // TODO: WAIT. should dissolveOut happen when the new room is being
            // dissolvedIn? and then cleanup is just getting rid of them
        }
        // TODO: case for if r is currently dissolving in, switching to dissolving out
        // StopCoroutine
        // TODO: get rid of extra loaded rooms in r
    }

    // DEBUG; for room detection sphere
    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.cyan;
    //     Gizmos.DrawWireSphere(transform.position, 1);
    // }

}
