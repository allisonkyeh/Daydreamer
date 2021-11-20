using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] public int roomNumber;
    // max number of possible doors
    [SerializeField] public int maxDoors;
    // List of 'doors', to other rooms
    public GameObject[] doors;
    /*
        this could be list of empty gameobjs, and just generate rooms at those points,
        replacing the gameobj in the list with the generated room
    */

    public bool isDissolvingIn = false;
    public bool isVisible = false;


    // OnPlayerEnter
    public void DissolveIn()
    {
        // check states
        isDissolvingIn = true;

        GameObject visuals = this.gameObject.transform.GetChild(0).gameObject;
        visuals.SetActive(true);
        // coroutine to dissolve over time

        isVisible = true;
        isDissolvingIn = false;
    }

    // OnPlayerExit
    public void DissolveOut()
    {
        Destroy(gameObject);
    }
}
