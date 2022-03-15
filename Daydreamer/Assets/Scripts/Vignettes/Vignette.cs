using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vignette : MonoBehaviour
{
    /*** VIGNETTE DATA ***/
    public enum Temperature {
        Warm,
        Neutral,
        Cold
    }
    public Temperature  temp;
    public bool         inhabited;
    public int          visits = 0;
    public Collider     vignetteCol;

    [SerializeField]
    Material            vignetteMaterial;
    // List<Material>      roomMats; // for if vignettes have multiple materials, but prob not

    /***** DISSOLVE TIMING *****/
    int             prevFrame;
    int             currFrame;
    float           timeElapsed;
    public float    lerpDuration;

    private void Awake() {
        vignetteCol = this.gameObject.transform.GetChild(0).GetComponent<SphereCollider>();
        vignetteCol.isTrigger = true;
    }

    // TODO: for now just keep characters and env the same material- maybe later char can have more
    private void OnTriggerEnter(Collider other) {
        // Debug.Log("OnTriggerEnter: Vignette");
        // Transform meshObj = this.gameObject.transform.GetChild(1);
        // vignetteMaterial = meshObj.GetChild(0).GetComponent<Renderer>().material;

        if (vignetteMaterial.GetFloat("_WholeMask") < 1)
        {
            Debug.Log("Starting coroutine: MakeVisible");
            StartCoroutine(MakeVisible(vignetteMaterial));
        }
    }
    private void OnTriggerExit(Collider other) {
        // Debug.Log("OnTriggerExit: Vignette");
        // Transform meshObj = this.gameObject.transform.GetChild(1);
        // vignetteMaterial = meshObj.GetChild(0).GetComponent<Renderer>().material;

        if (vignetteMaterial.GetFloat("_WholeMask") > -1)
        {
            Debug.Log("Starting coroutine: MakeHidden");
            StartCoroutine(MakeHidden(vignetteMaterial));
        }
    }

    IEnumerator MakeVisible(Material vignetteMaterial)
    {
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
        timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            vignetteMaterial.SetFloat("_WholeMask", Mathf.Lerp(1, -1, timeElapsed / lerpDuration));
            timeElapsed += Time.deltaTime;
            yield return null; // wait till next frame before continuing
        }
        vignetteMaterial.SetFloat("_WholeMask", -1);

        visits++; // track how many times visited to change dialogue
        gameObject.SetActive(false); // clear out room, but still need to track visits?
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 5);
    }
}
