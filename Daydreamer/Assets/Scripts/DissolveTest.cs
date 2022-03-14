using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manual control for dissolving
public class DissolveTest : MonoBehaviour
{
    [SerializeField]
    [Range(-3.30f, -2.2f)]
    private float ground = -3.3f;

    [SerializeField]
    Material[] groundMats;

    [SerializeField]
    [Range(0f, 1f)]
    private float dissolve = 0.0f;

    [SerializeField]
    Material[] dissolveMats;

    void Start() {
    }

    void Update() {
        foreach (Material mat in groundMats) {
            mat.SetFloat("_Dissolve", ground);
        }
        foreach (Material mat in dissolveMats) {
            mat.SetFloat("_Dissolve", dissolve);
        }
    }
}
