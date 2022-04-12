using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp : MonoBehaviour
{
    /***** WISP *****/
    [SerializeField]
    Material        wispMat;

    /***** DISSOLVE TIMING *****/
    [SerializeField] public float duration;
    private int     prevFrame;
    private int     currFrame;
    private float   timeElapsed;

    private void Start() {
        wispMat.SetFloat("_Mask", 0);
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        timeElapsed = 0;
        while (timeElapsed < duration)
        {
            wispMat.SetFloat("_Mask", Mathf.Lerp(0, 1, timeElapsed / duration));
            timeElapsed += Time.deltaTime;
            yield return null; // wait till next frame before continuing
        }
        wispMat.SetFloat("_Mask", 1);
    }
}
