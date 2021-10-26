using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelType : MonoBehaviour
{
    public int type;

    public void LevelDestruction()
    {
        Destroy(gameObject);
    }
}
