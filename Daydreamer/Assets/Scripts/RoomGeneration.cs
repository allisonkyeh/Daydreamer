using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{

    private int direction;
    public float moveAmount;
    
    private float timeBtwRoom;
    public float startTimeBtwRoom = 0.25f;

    public float minX; // based on transform value; units
    public float maxX;
    public float minZ;
    private bool stopGeneration;

    public Transform[] startingPositions;
    public GameObject[] rooms;
    // index 0 -> LR, 1 -> LRB, 2, -> LRT, 3 -> LRTB
    // make sure room types^ are in the right order in the input array in Inspector,
    // and set correct int in RoomType script component on each room prefab

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
                Vector3 newPos = new Vector3(transform.position.x - moveAmount, transform.position.y, transform.position.z);
                transform.position = newPos;

                int rand = Random.Range(0, rooms.Length); // use any room type
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(3, 6); // won't move RIGHT

            } else {
                direction = 5;
            }

        } else if (direction == 5) { // move DOWN
            if (transform.position.z > minZ) {
                Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - moveAmount);
                transform.position = newPos;

                // need room with top opening
                int rand = Random.Range(2, 4);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);
                // make sure prev room has bottom opening

                direction = Random.Range(1, 6); // go anywhere

            } else {    // STOP GENERATION
                stopGeneration = true;
            }

        }

    }
}
