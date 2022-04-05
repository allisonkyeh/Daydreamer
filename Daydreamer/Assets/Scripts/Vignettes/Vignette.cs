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
    [SerializeField] public Temperature  temp;
    [SerializeField] public bool         inhabited;
    [SerializeField] public int          visits = 0;
    [SerializeField] public Collider     vignetteCol;
    [SerializeField] Material            vignetteMaterial;
    // List<Material>      roomMats; // for if vignettes have multiple materials, but prob not
    private string      shellTag = "ShellTrigger";
    // using collision matrix to ignore shell x npc collision

    /***** DISSOLVE TIMING *****/
    [SerializeField] public float    lerpDuration;
    private int     prevFrame;
    private int     currFrame;
    private float   timeElapsed;

    /***** AUDIO *****/
    [SerializeField] AudioSource vignetteMusic;
    [SerializeField] GameObject ambience;
    [SerializeField] AudioSource vignetteExit;
    Component[] amb;

    private void Awake() {
        // vignetteCol = this.gameObject.GetComponent<SphereCollider>();
        // vignetteCol.isTrigger = true;
        vignetteMaterial.SetFloat("_WholeMask", -1);

        amb = ambience.GetComponents<AudioSource>();
    }

    // TODO: for now just keep characters and env the same material- maybe later char can have more
    private void OnTriggerEnter(Collider other) {
        // Debug.Log("OnTriggerEnter: Vignette " + other.gameObject.name);
        // Transform meshObj = this.gameObject.transform.GetChild(1);
        // vignetteMaterial = meshObj.GetChild(0).GetComponent<Renderer>().material;

        // Debug.Log("OnTriggerEnter Collider: " + other.gameObject.tag);

        if (other.gameObject.tag == shellTag && vignetteMaterial.GetFloat("_WholeMask") < 1)
        {
            // Debug.Log("Starting coroutine: MakeVisible" + other.gameObject.name);
            StartCoroutine(MakeVisible(vignetteMaterial));
        }
    }
    private void OnTriggerExit(Collider other) {
        // Debug.Log("OnTriggerExit: Vignette " + other.gameObject.name);
        // Transform meshObj = this.gameObject.transform.GetChild(1);
        // vignetteMaterial = meshObj.GetChild(0).GetComponent<Renderer>().material;

        if (other.gameObject.tag == shellTag && vignetteMaterial.GetFloat("_WholeMask") > -1)
        {
            // Debug.Log("Starting coroutine: MakeHidden" + other.gameObject.name);
            StartCoroutine(MakeHidden(vignetteMaterial));
        }
    }

    IEnumerator MakeVisible(Material vignetteMaterial)
    {
        vignetteMusic.Play();
        foreach (AudioSource a in amb) a.Pause();

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
        vignetteExit.Play();
        while (timeElapsed < lerpDuration)
        {
            vignetteMaterial.SetFloat("_WholeMask", Mathf.Lerp(1, -1, timeElapsed / lerpDuration));
            timeElapsed += Time.deltaTime;
            yield return null; // wait till next frame before continuing
        }
        vignetteMaterial.SetFloat("_WholeMask", -1);

        yield return new WaitForSeconds(2.0f);
        vignetteMusic.Stop();
        foreach (AudioSource a in amb) a.Play();

        visits++; // track how many times visited to change dialogue
        gameObject.SetActive(false); // clear out room, but still need to track visits?
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 7);
    }
}
