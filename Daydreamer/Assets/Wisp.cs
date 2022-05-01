using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp : MonoBehaviour
{
    /***** WISP *****/
    [SerializeField]
    Material        wispMat;

    /***** DISSOLVE TIMING *****/
    [SerializeField] public float spawnDuration;
    [SerializeField] public float dieDuration;

    private int     prevFrame;
    private int     currFrame;
    private float   timeElapsed;

    private void OnEnable() {
        wispMat.SetFloat("_Mask", 0);
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        timeElapsed = 0;
        while (timeElapsed < spawnDuration)
        {
            wispMat.SetFloat("_Mask", Mathf.Lerp(0, 1, timeElapsed / spawnDuration));
            timeElapsed += Time.deltaTime;
            yield return null; // wait till next frame before continuing
        }
        wispMat.SetFloat("_Mask", 1);
    }

    public IEnumerator Die(ParticleSystem particles, Animator anim)
    {
        // this coroutine starts when interaction starts, so wait a bit first
        yield return new WaitForSeconds(3.0f);

        timeElapsed = 0;
        while (timeElapsed < dieDuration)
        {
            wispMat.SetFloat("_Mask", Mathf.Lerp(1, 0, timeElapsed / dieDuration));
            timeElapsed += Time.deltaTime;
            yield return null; // wait till next frame before continuing
        }
        wispMat.SetFloat("_Mask", 0);
        particles.Stop();
        anim.SetBool("Absorbing", false);
    }

}
