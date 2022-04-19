using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vignette : MonoBehaviour
{
    // if smtg isn't working then double check that all serialized fields are set...

    /*** VIGNETTE DATA ***/
    private VignetteManager vmanager;
    [HideInInspector] public int visits = 0;
    public enum Temperature {
        Warm,
        Neutral,
        Cold
    }
    [SerializeField] public Temperature  temp;
    [SerializeField] public bool         inhabited;

    [Space]
    [SerializeField] public Collider     vignetteCol;
    [SerializeField] Material            vignetteMaterial;
    // List<Material>      roomMats; // for if vignettes have multiple materials, but prob not

    /***** PLAYER *****/
    [Space]
    private GameObject player;
    private float distFromPlayer;
    [SerializeField]
    private float maxDistFromPlayer;

    /***** COLLISIONS *****/
    private string  shellTag = "ShellTrigger";
    // using collision matrix to ignore shell x npc collision

    /***** DISSOLVE TIMING *****/
    [Space]
    [SerializeField] public float lerpDuration;
    private int     prevFrame;
    private int     currFrame;
    private float   timeElapsed;
    [SerializeField] private float maxLifetime;
    private float   startTime;
    private float   lifeTime;

    /***** AUDIO *****/
    // [SerializeField] AudioSource vignetteMusic;
    // [SerializeField] GameObject ambience;
    // [SerializeField] AudioSource vignetteExit;
    // Component[] amb;

    private void Awake() {
        // vignetteCol = this.gameObject.GetComponent<SphereCollider>();
        // vignetteCol.isTrigger = true;
        vignetteMaterial.SetFloat("_WholeMask", -1);
        player = GameObject.Find("PLAYER");
        vmanager = GameObject.Find("VIGMANAGER").GetComponent<VignetteManager>();

        startTime = Time.time;

    //    amb = ambience.GetComponents<AudioSource>();
    }

    private void Update() {
        CleanIfOld();
        CleanIfFar();
    }

    private void CleanIfOld() {
        lifeTime = Time.time - startTime;
        if (lifeTime > maxLifetime) {
            // only if player hasn't gone in yet.. need to do smtg with MakeHidden otherwise
            if (vignetteMaterial.GetFloat("_WholeMask") == -1) CleanUp();
        }
    }

    private void CleanIfFar() {
        distFromPlayer = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if (distFromPlayer > maxDistFromPlayer) {
            Debug.Log("Too far. Cleaning vignette.");
            CleanUp();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == shellTag && vignetteMaterial.GetFloat("_WholeMask") < 1) {
            StartCoroutine(MakeVisible(vignetteMaterial));
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == shellTag && vignetteMaterial.GetFloat("_WholeMask") > -1) {
            StartCoroutine(MakeHidden(vignetteMaterial));
        }
    }

    IEnumerator MakeVisible(Material vignetteMaterial)
    {
        // vignetteMusic.Play();
        // foreach (AudioSource a in amb) a.Pause();

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
        // vignetteExit.Play();
        while (timeElapsed < lerpDuration)
        {
            vignetteMaterial.SetFloat("_WholeMask", Mathf.Lerp(1, -1, timeElapsed / lerpDuration));
            timeElapsed += Time.deltaTime;
            yield return null; // wait till next frame before continuing
        }
        vignetteMaterial.SetFloat("_WholeMask", -1);

        yield return new WaitForSeconds(2.0f);
        // vignetteMusic.Stop();
        // foreach (AudioSource a in amb) a.Play();

        CleanUp();
    }

    void CleanUp()
    {
        vmanager.activeNum--;
        Destroy(gameObject);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 7);
    }
}
