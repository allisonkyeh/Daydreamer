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
    List<Material> roomMats;
    private Collider    playerCol;
    private Transform  rmRoot;

    int prevFrame;
    int currFrame;
    public float lerpDuration;
    float timeElapsed;

    private void Awake() {
        playerCol = this.gameObject.transform.GetChild(0).GetComponent<Collider>();
        // playerCol.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("OnTriggerEnter: PlayerShell");

        rmRoot = other.gameObject.transform.root;

        // checks layer
        // if (other.gameObject.layer == layerIndex) {
        //     rmMaterial = rmRoot.GetComponent<Renderer>().mat;
        // }

        // SHOULD iterate through room and add the materials of all the child objects
        int numOfChildren = rmRoot.childCount;
        for(int i = 0; i < numOfChildren; i++)
        {
            GameObject child = rmRoot.GetChild(i).gameObject;
            Material childMat = child.GetComponent<Renderer>().material;
            if (!roomMats.Contains(childMat)) roomMats.Add(childMat);
        }

        // if already visible, skip coroutine
        foreach (Material rmMaterial in roomMats){
            if (rmMaterial.GetFloat("_WholeMask") != 1)
            {
                StartCoroutine(MakeVisible(rmMaterial));
            }
        }

    }

    IEnumerator MakeVisible(Material rmMaterial)
    {
        timeElapsed = 0;

        playerShell.SetFloat("_WholeMask", 1);

        while (timeElapsed < lerpDuration)
        {
            rmMaterial.SetFloat("_WholeMask", Mathf.Lerp(-1, 1, timeElapsed / lerpDuration));
            timeElapsed += Time.deltaTime;
            yield return null; // wait till next frame before continuing
        }
        rmMaterial.SetFloat("_WholeMask", 1);

        playerShell.SetFloat("_WholeMask", -1);
    }
}

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

    //     if (wholeMask > -0.5f) {
    //         trailEmi.enabled = true;
    //         printEmi.enabled = true;
    //     } else if (wholeMask <= -0.50f) {
    //         trailEmi.enabled = false;
    //         printEmi.enabled = false;
    //     }
    // }
