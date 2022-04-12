using UnityEngine;

public class VignetteManager : MonoBehaviour
{
    /*** PLAYER ***/
    public GameObject player;

    /*** VIGNETTE ***/
    [SerializeField] public GameObject[] vignettes;
    [SerializeField] public float   spawnMinDist;
    [SerializeField] public float   spawnMaxDist;
    [SerializeField] public float   spawnRate;
    [SerializeField] public int     activeMax;
    [HideInInspector]
    public int activeVigs = 0;
    private int vignetteLayer => LayerMask.NameToLayer("Vignette");

    void Start()
    {
        Spawn();
        InvokeRepeating("Spawn", 10.0f, spawnRate);
    }

    void Spawn()
    {
        if (activeVigs < activeMax) {
            // spawn in a donut shaped area around the player
            Vector3 newPos  = player.transform.position;
            Vector3 d       = new Vector3 (Random.Range(spawnMinDist, spawnMaxDist), 0, 0);
            Quaternion a    = Quaternion.Euler(0, Random.Range(0, 360), 0);

            newPos += d;
            newPos = a * (newPos - player.transform.position) + player.transform.position;

            int i = Random.Range(0, vignettes.Length);
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
                activeVigs++;
            }
        }
    }
}
