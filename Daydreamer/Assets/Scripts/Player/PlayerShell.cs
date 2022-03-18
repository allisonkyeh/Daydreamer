using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShell : MonoBehaviour
{
    /***** CORRUPTION *****/
    [SerializeField][Range(0.02f, 1.0f)]
    public float    corruptionValue; // good <- 0.02 to 1 -> bad
    public float    corruptionRate;
    public bool     corrupting = true;

    /***** PLAYER *****/
    [SerializeField]
    Material    shellMat;
    Collider    playerCol;
    // var         playerCollision;

    private void Awake() {
        shellMat.SetFloat("_CorruptionValue", 0.02f);

        // playerCollision = GetComponent(PlayerCollision);
    }

    void Start() {
        InvokeRepeating("Corrupt", 1.0f, 1.0f);
    }

    void Corrupt() {
        if (corrupting && (corruptionValue < 1.0f)) {
            corruptionValue += corruptionRate; // increase displacement -> 1
        } else if (!corrupting && (corruptionValue > 0.02f)){
            corruptionValue = corruptionValue - (2.0f * corruptionRate); // decrease displacement -> 0.02
        }
        // maybe switch the minmax conditions with Mathf.Clamp
        shellMat.SetFloat("_CorruptionValue", corruptionValue);
    }
}

    /***** PARTICLE SYSTEMS *****/
    // [SerializeField]
    // GameObject handTrail;
    // [SerializeField]
    // GameObject handPrints;
    // ParticleSystem handTrailSys;
    // ParticleSystem handPrintsSys;

    // void Start()
    // {
    //     handTrailSys = handTrail.GetComponent<ParticleSystem>();
    //     handPrintsSys = handPrints.GetComponent<ParticleSystem>();
    // }

    // void Update()
    // {
    //     var trailEmi = handTrailSys.emission;
    //     var printEmi = handPrintsSys.emission;

    //     if (M > -0.5f) {
    //         trailEmi.enabled = true;
    //         printEmi.enabled = true;
    //     } else if (M <= -0.50f) {
    //         trailEmi.enabled = false;
    //         printEmi.enabled = false;
    //     }
    // }
