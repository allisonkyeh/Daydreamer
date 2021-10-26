using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomType : MonoBehaviour
{
    public int roomNumber;

    public GameObject entranceMarker;
    public GameObject[] exitMarker;

    public void RoomDestruction()
    {
        Destroy(gameObject);
    }
}
