using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    // [SerializeField] private DialogueObject testDialogue;

    public bool IsOpen { get; private set; }

    private ResponseHandler responseHandler;
    private TypewriterEffect typewriterEffect;

    [SerializeField] AudioSource npcVoice;

    private void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
        responseHandler = GetComponent<ResponseHandler>();

        CloseDialogueBox();
        // ShowDialogue(testDialogue);
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        IsOpen = true;
        dialogueBox.SetActive(true);
        npcVoice.Play();
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        // yield return new WaitForSeconds(0.5f);

        // if there are responses to show, don't wait for spacebar
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++) {
            string dialogue = dialogueObject.Dialogue[i];
            yield return typewriterEffect.Run(dialogue, textLabel);

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

            // Advance text with button
             yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            // yield return new WaitUntil(() => Input.GetMouseButton(0));

        }

        if (dialogueObject.HasResponses){
            responseHandler.ShowResponses(dialogueObject.Responses);
        } else {
            CloseDialogueBox();
        }
    }

    private void CloseDialogueBox()
    {
        IsOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
        npcVoice.Stop();
    }

}
