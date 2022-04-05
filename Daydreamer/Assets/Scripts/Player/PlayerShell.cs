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
    Material        shellMat;
    private Color   addColor;
    private Color   targetColor;
    Collider        playerCol;
    // var         playerCollision;

    /***** DISSOLVE TIMING *****/
    [SerializeField] public float deathDuration;
    private bool    dying = false;
    private int     prevFrame;
    private int     currFrame;
    private float   timeElapsed;

    private float pingpongCorruption = 0;
    [SerializeField] private float pingpongSpeed;
    [SerializeField] private float pingpongThreshold = 0.1f;


    private void Awake() {
        targetColor = new Color(1, 0, 0, 1); //red
        addColor = new Color(1, 1, 1, 1);
        shellMat.SetFloat("_CorruptionValue", 0.02f);
        shellMat.SetColor("_Color", addColor);
        shellMat.SetFloat("_WholeMask", 1);

        // playerCollision = GetComponent(PlayerCollision);
    }

    void Start() {
        InvokeRepeating("Corrupt", 1.0f, 1.0f);
    }

    void Update() {
        if (corruptionValue < pingpongThreshold || !corrupting) {
            shellMat.SetFloat("_CorruptionValue", corruptionValue);
        } else {
            pingpongCorruption = Mathf.PingPong(Time.time / 2f, corruptionValue) + 0.02f;
            shellMat.SetFloat("_CorruptionValue", pingpongCorruption);
        }
    }

    void Corrupt() {
        Color currentColor = shellMat.GetColor("_Color");
        Color baseColor = new Color(1, 1, 1, 1);

        if (corrupting && (corruptionValue < 1.0f)) {

            corruptionValue += corruptionRate; // increase displacement -> 1

            if (currentColor != targetColor) {
                addColor.g -= 0.02f;
                addColor.b -= 0.02f;
            } else if (currentColor == targetColor && !dying) {
                StartCoroutine(Death());
            }

        } else if (!corrupting && (corruptionValue > 0.02f)){
            corruptionValue = corruptionValue - (2.0f * corruptionRate); // decrease displacement -> 0.02

            if (currentColor != baseColor) {
                addColor.g += 0.02f;
                addColor.b += 0.02f;
            }
        }
        // maybe switch the minmax conditions with Mathf.Clamp
        // pingpongCorruption = Mathf.PingPong(Time.time, corruptionValue) + 0.02f;
        // shellMat.SetFloat("_CorruptionValue", pingpongCorruption);

        addColor.g = Mathf.Clamp(addColor.g, 0, 1);
        addColor.b = Mathf.Clamp(addColor.b, 0, 1);
        shellMat.SetColor("_Color", addColor);

    }

    IEnumerator Death()
    {
        dying = true;
        timeElapsed = 0;
        while (timeElapsed < deathDuration)
        {
            shellMat.SetFloat("_WholeMask", Mathf.Lerp(1, -1, timeElapsed / deathDuration));
            timeElapsed += Time.deltaTime;
            yield return null; // wait till next frame before continuing
        }
        shellMat.SetFloat("_WholeMask", -1);
    }

    // TODO:
    IEnumerator Revive()
    {
        yield return null;
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
