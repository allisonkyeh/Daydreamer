using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vignette : MonoBehaviour
{
    /*** VIGNETTE DATA ***/
    [SerializeField][Range(0, 3)]
    public int      temperature;
    public bool  inhabited;
    public int      visits = 0;
    public Collider vignetteCol;

    Material            vignetteMaterial;
    // List<Material>      roomMats; // for if vignettes have multiple materials, but prob not

    /***** DISSOLVE TIMING *****/
    int             prevFrame;
    int             currFrame;
    float           timeElapsed;
    public float    lerpDuration;
    private int     layerIndex => LayerMask.NameToLayer("Room");

    private void Awake() {
        vignetteCol = this.gameObject.transform.GetChild(0).GetComponent<Collider>();
        vignetteCol.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("OnTriggerEnter: Vignette");

        Transform meshObj = this.gameObject.transform.GetChild(1);
        Debug.Log("this = " + this.name);
        Debug.Log("meshObj = " + meshObj.name);

        vignetteMaterial = meshObj.GetChild(0).GetComponent<Renderer>().material;

        if ((other.gameObject.layer == layerIndex) && (vignetteMaterial.GetFloat("_WholeMask") != 1))
        {
            // Debug.Log("Starting coroutine");
            StartCoroutine(MakeVisible(vignetteMaterial));
        }

    }

    IEnumerator MakeVisible(Material vignetteMaterial)
    {
        // TODO: change ref name in shader

        timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            vignetteMaterial.SetFloat("_WholeMask", Mathf.Lerp(-1, 1, timeElapsed / lerpDuration));
            timeElapsed += Time.deltaTime;
            yield return null; // wait till next frame before continuing
        }
        vignetteMaterial.SetFloat("_WholeMask", 1);
    }


    IEnumerator MakeHidden(Material vignetteMaterial)
    {
        // TODO: change ref name in shader

        timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            vignetteMaterial.SetFloat("_WholeMask", Mathf.Lerp(1, -1, timeElapsed / lerpDuration));
            timeElapsed += Time.deltaTime;
            yield return null; // wait till next frame before continuing
        }
        vignetteMaterial.SetFloat("_WholeMask", 1);
    }
}
