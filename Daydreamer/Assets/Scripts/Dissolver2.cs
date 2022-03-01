using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolver2 : MonoBehaviour
{
    [SerializeField]
    [Range(-1, 1)]
    public float wholeMask;

    [SerializeField]
    Material            playerShell;
    List<Material>      roomMats;
    private Collider    rmCol;
    private Transform   rmRoot;
    Material            rmMaterial;

    int             prevFrame;
    int             currFrame;
    float           timeElapsed;
    public float    lerpDuration;
    private int     layerIndex => LayerMask.NameToLayer("Room");

    private void Awake() {
        rmCol = this.gameObject.transform.GetChild(0).GetComponent<Collider>();
        rmCol.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("OnTriggerEnter: Dissolver2");

        Transform meshObj = this.gameObject.transform.GetChild(1);
        Debug.Log("this = " + this.name);
        Debug.Log("meshObj = " + meshObj.name);

        rmMaterial = meshObj.GetChild(0).GetComponent<Renderer>().material;

        Debug.Log("rmMaterial = " + rmMaterial);

        // SHOULD iterate through room and add the materials of all the child objects
        // int numOfChildren = rmRoot.childCount;
        // for(int i = 0; i < numOfChildren; i++)
        // {
        //     GameObject child = rmRoot.GetChild(i).gameObject;
        //     Material childMat = child.GetComponent<Renderer>().material;
        //     if (!roomMats.Contains(childMat)) roomMats.Add(childMat);
        // }

        // if already visible, skip coroutine
        // foreach (Material rmMaterial in roomMats){}

        if ((other.gameObject.layer == layerIndex) && (rmMaterial.GetFloat("_WholeMask") != 1))
        {
            Debug.Log("Starting coroutine");
            StartCoroutine(MakeVisible(rmMaterial));
        }

    }

    IEnumerator MakeVisible(Material rmMaterial)
    {
        playerShell.SetFloat("_WholeMask", 1);
        timeElapsed = 0;
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
