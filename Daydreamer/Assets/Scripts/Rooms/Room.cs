using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    /*** ROOM DATA ***/
    [SerializeField] public int roomNumber;
    // List of 'doors', to other rooms; for getting position/rotation to line up
    public GameObject[] doors;
    // List of neighboring rooms; for cleanup
    public List<GameObject> doorsRooms;
    // Max number of possible doors
    public int maxDoors => doors.Length;
    // The door the player is entering from; constantly changes
    public GameObject entrance;

    /*** DISSOLVE ***/
    public bool isDissolvingIn  = false;
    public bool isDissolvingOut = false;
    public bool isDissolvedIn   = false;
    // How long dissolve takes; maybe separate out for in out?
    private float dissolveDuration;

    private void Awake()
    {
        doorsRooms = new List<GameObject>();
    }

    // Player OnTriggerEnter -> Setup
    public void DissolveIn()
    {
        // check states
        isDissolvingIn = true;

        GameObject visuals = this.gameObject.transform.GetChild(0).gameObject;
        visuals.SetActive(true);
        // coroutine to dissolve over time

        isDissolvedIn = true;
        isDissolvingIn = false;
    }

    // Player OnTriggerExit -> Cleanup
    public void DissolveOut()
    {
        isDissolvingOut = true;

        Destroy(gameObject);

        isDissolvedIn = false;
        isDissolvingOut = false;
    }
}
