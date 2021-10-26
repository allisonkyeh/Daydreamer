using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Modified LevelGeneration

public class RoomGeneration : MonoBehaviour
{
    // edited to move along x and z axis; x being LR, z being down
    private int direction;
    public float moveAmount;

    private float timeBtwRoom;
    public float startTimeBtwRoom = 0.25f;

    public float minX; // based on transform value; units
    public float maxX;
    public float minZ;
    private bool stopGeneration;

    public LayerMask roomMask; // makes sure only detect rooms and not other objs w/colliders
    private int downCounter;

    public Transform[] startingPositions;
    public GameObject[] rooms;
    // index 0 -> LR, 1 -> LRB, 2, -> LRT, 3 -> LRTB
    // make sure room types^ are in the right order in the input array in Inspector,
    // and set correct int in RoomType script component on each room prefab
    // also lol make sure you're using the prefab rooms with setup, not just the plain fbxs

    // Start is called before the first frame update
    private void Start()
    {
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;
        Instantiate(rooms[0], transform.position, Quaternion.identity);

        direction = Random.Range(1, 6); // not 0-based, so gets first to fifth

    }

    private void Update()
    {

        // TODO - switch to if player nears exit
        if (timeBtwRoom <= 0 && stopGeneration == false) {
            Move();
            timeBtwRoom = startTimeBtwRoom;
        } else {
            timeBtwRoom -= Time.deltaTime;
        }

    }

    private void Move()
    {
        if (direction == 1 || direction == 2) { // move RIGHT
            if (transform.position.x < maxX){
                downCounter = 0;

                Vector3 newPos = new Vector3(transform.position.x + moveAmount, transform.position.y, transform.position.z);
                transform.position = newPos;

                int rand = Random.Range(0, rooms.Length); // use any room type
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6);
                if (direction == 3) { // if going LEFT, ie. to prev room, go RIGHT instead
                    direction = 2;
                } else if (direction == 4) { // if going LEFT, ie. to prev room, go DOWN instead
                    direction = 5;
                }
            } else {
                direction = 5;
            }

        } else if (direction == 3 || direction == 4) { // move LEFT
            if (transform.position.x < maxX){
                downCounter = 0;

                Vector3 newPos = new Vector3(transform.position.x - moveAmount, transform.position.y, transform.position.z);
                transform.position = newPos;

                int rand = Random.Range(0, rooms.Length); // use any room type
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(3, 6); // won't move RIGHT

            } else {
                direction = 5;
            }

        } else if (direction == 5) { // move DOWN

            downCounter++;

            if (transform.position.z > minZ) {

                // cast sphere to detect current 0: this is before moving
                Vector3 offset = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                Collider[] roomDetection = Physics.OverlapSphere(transform.position, 1, roomMask);

                // Debug.Log(roomDetection.Length + " rooms detected");
                // foreach(var col in roomDetection) {
                //     Debug.Log(" TYPE: " + col.gameObject.name);
                // }

                // if no bottom opening, destroy and spawn a correct room
                if (roomDetection[0].GetComponent<RoomType>().type != 1 && roomDetection[0].GetComponent<RoomType>().type != 3) {
                     if (downCounter >= 2) {
                        roomDetection[0].GetComponent<RoomType>().RoomDestruction();
                        Debug.Log(roomDetection[0].gameObject.name + " DESTROYED!!!!!!!!!");

                        Instantiate(rooms[3], transform.position, Quaternion.identity);
                        Debug.Log(rooms[3].gameObject.name + " INSTANTIATED!!!!!!!!");
                     } else {
                        roomDetection[0].GetComponent<RoomType>().RoomDestruction();
                        Debug.Log(roomDetection[0].gameObject.name + " DESTROYED!!!!");

                        int randBottomRoom = Random.Range(1, 4);
                        if (randBottomRoom == 2) randBottomRoom = 1;
                        Instantiate(rooms[randBottomRoom], transform.position, Quaternion.identity);
                        Debug.Log(rooms[randBottomRoom].gameObject.name + " INSTANTIATED!!!!");
                     }


                }

                Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - moveAmount);
                transform.position = newPos;

                // make sure next room has top opening
                int rand = Random.Range(2, 4);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);
                // make sure prev room has bottom opening

                direction = Random.Range(1, 6); // go anywhere

            } else {    // STOP GENERATION
                stopGeneration = true;
            }
        }
    }

    // DEBUG; for room detection sphere
    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 1);
    }

}
