using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShell : MonoBehaviour
{
    [SerializeField]
    [Range(-1, 1)]
    public float wholeMask;

    [SerializeField]
    Material playerShell;

    [SerializeField]
    GameObject handTrail;
    [SerializeField]
    GameObject handPrints;

    ParticleSystem handTrailSys;
    ParticleSystem handPrintsSys;

    void Start()
    {
        handTrailSys = handTrail.GetComponent<ParticleSystem>();
        handPrintsSys = handPrints.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        playerShell.SetFloat("_WholeMask", wholeMask);

        var trailEmi = handTrailSys.emission;
        var printEmi = handPrintsSys.emission;

        if (wholeMask > -0.5f) {
            trailEmi.enabled = true;
            printEmi.enabled = true;
        } else if (wholeMask <= -0.50f) {
            trailEmi.enabled = false;
            printEmi.enabled = false;
        }
    }
}
