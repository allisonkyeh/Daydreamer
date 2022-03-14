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
        playerCol = this.gameObject.transform.GetChild(0).GetComponent<Collider>();
        playerCol.isTrigger = true;
        // playerCollision = GetComponent(PlayerCollision);
    }

    private void Update() {
        if (corrupting) {
            corruptionValue += corruptionRate;
        } else {
            corruptionValue -= corruptionRate;
        }
        shellMat.SetFloat("_CorruptionValue", corruptionValue);
    }

    // private void OnTriggerEnter(Collider other) {
    //     Debug.Log("OnTriggerEnter: PlayerShell");
    // }

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
