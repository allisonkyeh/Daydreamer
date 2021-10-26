using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Modified LevelGeneration

public class RoomGeneration : MonoBehaviour
{
    private int currentRoom;    // keeps track of current room, need offset
    private int nextRoom;     // next room to activate
    private Vector3 moveAmount;   // amount to move generator gameobj

    public LayerMask roomMask;  // makes sure only detect rooms and not other objs w/colliders
    private int downCounter;

    public GameObject player;   // for detecting collisions/near
    // private Transform childCollider;
    private Collider col;

    public Transform[] startingPositions;
    public GameObject[] rooms;
    // make sure you're using the prefab rooms with setup, not just the plain fbxs

    // Start is called before the first frame update
    private void Start()
    {
        // picks one random starting position if multiple
        // maybe get rid of this if everyone starts at same place
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;

        // instantiate first room
        Instantiate(rooms[0], transform.position, Quaternion.identity);
        currentRoom = 0;

        // Setup player colliders
        // childCollider = player.transform.Find("PlayerCollider")
        col = player.transform.GetComponent<Collider>();

        Move();
    }

    private void Update()
    {
        // if Player collider detects an exit collider, move and generate room
        // need to isolate to col_exit only, maybe layer mask or check name?

        // if (col.isTrigger) {
        //     Move();
        // }
    }

    private void Move()
    {
        // Random number for next room
        // nextRoom = Random.Range(1, 4); // int min inclusive, max exclusive
        nextRoom = 1;

        Transform currentOffset = rooms[currentRoom].GetComponent<RoomType>().exitMarker[0].transform;
        Debug.Log("currentOffset: " + currentOffset.position);

        // negate next offset
        Transform nextOffset = rooms[nextRoom].GetComponent<RoomType>().entranceMarker.transform;
        Debug.Log("nextOffset: " + nextOffset.position);

        moveAmount = currentOffset.position - nextOffset.position;
        transform.position += moveAmount;   // moves RoomGenerator

        Instantiate(rooms[nextRoom], transform.position, Quaternion.identity);
        Debug.Log("nextRoom INSTANTIATED!!!!");



        /***************** OLD *****************/

        // if (roomNumber == 1 || roomNumber == 2) { // move RIGHT
        //     if (transform.position.x < maxX){
        //         downCounter = 0;

        //         Vector3 newPos = new Vector3(transform.position.x + moveAmount, transform.position.y, transform.position.z);
        //         transform.position = newPos;

        //         int rand = Random.Range(0, rooms.Length); // use any room type
        //         Instantiate(rooms[rand], transform.position, Quaternion.identity);

        //         roomNumber = Random.Range(1, 6);
        //         if (roomNumber == 3) { // if going LEFT, ie. to prev room, go RIGHT instead
        //             roomNumber = 2;
        //         } else if (roomNumber == 4) { // if going LEFT, ie. to prev room, go DOWN instead
        //             roomNumber = 5;
        //         }
        //     } else {
        //         roomNumber = 5;
        //     }

        // } else if (roomNumber == 3 || roomNumber == 4) { // move LEFT
        //     if (transform.position.x < maxX){
        //         downCounter = 0;

        //         Vector3 newPos = new Vector3(transform.position.x - moveAmount, transform.position.y, transform.position.z);
        //         transform.position = newPos;

        //         int rand = Random.Range(0, rooms.Length); // use any room type
        //         Instantiate(rooms[rand], transform.position, Quaternion.identity);

        //         roomNumber = Random.Range(3, 6); // won't move RIGHT

        //     } else {
        //         roomNumber = 5;
        //     }

        // } else if (roomNumber == 5) { // move DOWN

        //     downCounter++;

        //     if (transform.position.z > minZ) {

        //         // cast sphere to detect current 0: this is before moving
        //         Vector3 offset = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        //         Collider[] roomDetection = Physics.OverlapSphere(transform.position, 1, roomMask);

        //         // Debug.Log(roomDetection.Length + " rooms detected");
        //         // foreach(var col in roomDetection) {
        //         //     Debug.Log(" TYPE: " + col.gameObject.name);
        //         // }

        //         // if no bottom opening, destroy and spawn a correct room
        //         if (roomDetection[0].GetComponent<RoomType>().type != 1 && roomDetection[0].GetComponent<RoomType>().type != 3) {
        //              if (downCounter >= 2) {
        //                 roomDetection[0].GetComponent<RoomType>().RoomDestruction();
        //                 Debug.Log(roomDetection[0].gameObject.name + " DESTROYED!!!!!!!!!");

        //                 Instantiate(rooms[3], transform.position, Quaternion.identity);
        //                 Debug.Log(rooms[3].gameObject.name + " INSTANTIATED!!!!!!!!");
        //              } else {
        //                 roomDetection[0].GetComponent<RoomType>().RoomDestruction();
        //                 Debug.Log(roomDetection[0].gameObject.name + " DESTROYED!!!!");

        //                 int randBottomRoom = Random.Range(1, 4);
        //                 if (randBottomRoom == 2) randBottomRoom = 1;
        //                 Instantiate(rooms[randBottomRoom], transform.position, Quaternion.identity);
        //                 Debug.Log(rooms[randBottomRoom].gameObject.name + " INSTANTIATED!!!!");
        //              }
        //         }
        //         Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - moveAmount);
        //         transform.position = newPos;

        //         // make sure next room has top opening
        //         int rand = Random.Range(2, 4);
        //         Instantiate(rooms[rand], transform.position, Quaternion.identity);
        //         // make sure prev room has bottom opening

        //         roomNumber = Random.Range(1, 6); // go anywhere
        // }
    }

    // DEBUG; for room detection sphere
    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.cyan;
    //     Gizmos.DrawWireSphere(transform.position, 1);
    // }

}
