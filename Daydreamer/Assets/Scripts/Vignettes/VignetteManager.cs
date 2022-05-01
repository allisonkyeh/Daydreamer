using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VignetteManager : MonoBehaviour
{
    /*** PLAYER ***/
    public GameObject player;

    /*** VIGNETTE ***/
    [SerializeField] public List<GameObject> vignettes = new List<GameObject>();
    [SerializeField] public float   spawnMinDist;
    [SerializeField] public float   spawnMaxDist;
    [SerializeField] public float   spawnRate;
    [SerializeField] public int     activeMax;
    [HideInInspector]
    public int activeNum = 0;
    [Space]
    [SerializeField]
    public List<GameObject> activeVignettes = new List<GameObject>();
    private int vignetteLayer => LayerMask.NameToLayer("Vignette");

    void Start()
    {
        Spawn();
        // Vector3 newPos = player.transform.forward + Vector3(0, 0, 10);
        // GameObject v = (GameObject) Instantiate(vignettes[0], newPos, Quaternion.identity);

        InvokeRepeating("Spawn", 20.0f, spawnRate);
    }

    void Spawn()
    {
        if (activeNum < activeMax) {
            // spawn in a donut shaped area around the player
            Vector3 newPos  = player.transform.position;
            Vector3 d       = new Vector3 (Random.Range(spawnMinDist, spawnMaxDist), 0, 0);
            Quaternion a    = Quaternion.Euler(0, Random.Range(0, 360), 0);

            newPos += d;
            newPos = a * (newPos - player.transform.position) + player.transform.position;

            int i = Random.Range(0, vignettes.Count);
            GameObject v = (GameObject) Instantiate(vignettes[i], newPos, Quaternion.identity);

            Collider[] intersections = Physics.OverlapSphere(newPos, 7);
            int validCols = 0;
            foreach (Collider col in intersections) {
                if (col.gameObject.layer == vignetteLayer) validCols++;
            }
            if (validCols > 1) {
                Debug.Log("Destroyed an instantiated vignette.");
                Destroy(v);
            } else {
                activeNum++;
                activeVignettes.Add(vignettes[i]);
                vignettes.RemoveAt(i);
            }
        }
    }
}
