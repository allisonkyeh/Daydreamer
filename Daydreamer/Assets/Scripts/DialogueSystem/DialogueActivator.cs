using UnityEngine;

// handles call to dialogueUI interaction, by implementing IInteractable interface
public class DialogueActivator : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private ParticleSystem particles;
    private Wisp w;

    private void Awake() {
        w = this.GetComponent<Wisp>();
    }

    private void OnTriggerEnter(Collider other) {
        // checks for player tag and player component
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerController player)) {
            player.Interactable = this;
        }

    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerController player)) {
            if (player.Interactable is DialogueActivator dialogueActivator && dialogueActivator == this) {
                player.Interactable = null;
            }
        }
    }

    public void Interact (PlayerController player, Animator anim) {
        particles.Play();
        StartCoroutine(w.Die(particles, anim));
        player.DialogueUI.ShowDialogue(dialogueObject);
    }
}
