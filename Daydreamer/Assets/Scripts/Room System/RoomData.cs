using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    public int roomNumber;

    private GameObject[] rooms;

    public void Cleanup()
    {
        Destroy(gameObject);
    }
}
