using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolver : MonoBehaviour
{
    [SerializeField][Range(-1, 1)]
    public float visibilityMask;  // for manual testing

    [SerializeField]
    Material     playerShell;

    List<Material>      roomMats;
    private Collider    roomCol;
    private Transform   roomRoot;
    Material            roomMaterial;

    int             prevFrame;
    int             currFrame;
    float           timeElapsed;
    public float    lerpDuration;
    private int     layerIndex => LayerMask.NameToLayer("Room");

    private void Awake() {
        roomCol = this.gameObject.transform.GetChild(0).GetComponent<Collider>();
        roomCol.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("OnTriggerEnter: Dissolver2");

        Transform meshObj = this.gameObject.transform.GetChild(1);
        Debug.Log("this = " + this.name);
        Debug.Log("meshObj = " + meshObj.name);

        roomMaterial = meshObj.GetChild(0).GetComponent<Renderer>().material;

        // Debug.Log("roomMaterial = " + roomMaterial);

        // SHOULD iterate through room and add the materials of all the child objects
        // int numOfChildren = roomRoot.childCount;
        // for(int i = 0; i < numOfChildren; i++)
        // {
        //     GameObject child = roomRoot.GetChild(i).gameObject;
        //     Material childMat = child.GetComponent<Renderer>().material;
        //     if (!roomMats.Contains(childMat)) roomMats.Add(childMat);
        // }

        // if already visible, skip coroutine
        // foreach (Material roomMaterial in roomMats){}

        if ((other.gameObject.layer == layerIndex) && (roomMaterial.GetFloat("_visibilityMask") != 1))
        {
            // Debug.Log("Starting coroutine");
            StartCoroutine(MakeVisible(roomMaterial));
        }

    }

    IEnumerator MakeVisible(Material roomMaterial)
    {
        // TODO: change _WholeMask in shader, also make it from 0 to 1?

        playerShell.SetFloat("_WholeMask", 1);
        timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            roomMaterial.SetFloat("_WholeMask", Mathf.Lerp(-1, 1, timeElapsed / lerpDuration));
            timeElapsed += Time.deltaTime;
            yield return null; // wait till next frame before continuing
        }
        roomMaterial.SetFloat("_WholeMask", 1);
        playerShell.SetFloat("_WholeMask", -1);
    }
}
