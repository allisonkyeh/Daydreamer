using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] public int roomNumber;
    // max number of possible doors
    [SerializeField] private int maxDoors;
    // List of 'doors', to other rooms
    private GameObject[] doors;

    /*
        this could be list of empty gameobjs, and just generate rooms at those points,
        replacing the gameobj in the list with the generated room
    */

    // OnPlayerEnter
    public void Setup()
    {
        
    }

    // OnPlayerExit
    public void Cleanup()
    {
        Destroy(gameObject);
    }
}
