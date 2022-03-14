using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shellMat : MonoBehaviour
{
    /***** CORRUPTION *****/
    [SerializeField]
    [Range(-1, 1)]
    public float    corruptionValue;
    public float    corruptionRate;
    public boolean  corrupting = true;

    /***** PLAYER *****/
    [SerializeField]
    Material    shellMat;
    Collider    playerCol;
    var         playerCollision;

    /***** VIGNETTES *****/
    List<Material>      roomMats;
    private Transform   roomRoot;
    // TODO: need to phase this out so it's handled in vignette class

    /***** DISSOLVE TIMING *****/
    int prevFrame;
    int currFrame;
    public float lerpDuration;
    float timeElapsed;

    private void Awake() {
        playerCol = this.gameObject.transform.GetChild(0).GetComponent<Collider>();
        playerCol.isTrigger = true;
        playerCollision = GetComponent(PlayerCollision);
    }

    private void Update() {
        if (corrupting) {
            corruptionValue += corruptionRate;
        } else {
            corruptionValue -= corruptionRate;
        }
        // TODO: change _WholeMask in shader, also make it from 0 to 1?
        shellMat.SetFloat("_WholeMask", corruptionValue);
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("OnTriggerEnter: PlayerShell");

        roomRoot = other.gameObject.transform.root;

        // checks layer
        // if (other.gameObject.layer == layerIndex) {
        //     rmMaterial = roomRoot.GetComponent<Renderer>().mat;
        // }

        // SHOULD iterate through room and add the materials of all the child objects
        int numOfChildren = roomRoot.childCount;
        for(int i = 0; i < numOfChildren; i++)
        {
            GameObject child = roomRoot.GetChild(i).gameObject;
            Material childMat = child.GetComponent<Renderer>().material;
            if (!roomMats.Contains(childMat)) roomMats.Add(childMat);
        }

        // if already visible, skip coroutine
        foreach (Material rmMaterial in roomMats){
            if (rmMaterial.GetFloat("_M") != 1)
            {
                StartCoroutine(MakeVisible(rmMaterial));
            }
        }

    }

    IEnumerator MakeVisible(Material rmMaterial)
    {
        timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            rmMaterial.SetFloat("_WholeMask", Mathf.Lerp(-1, 1, timeElapsed / lerpDuration));
            timeElapsed += Time.deltaTime;
            yield return null; // wait till next frame before continuing
        }
        rmMaterial.SetFloat("_WholeMask", 1);
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
