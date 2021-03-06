using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference movementControl;
    [SerializeField] private InputActionReference jumpControl;

    private Animator anim;

    [SerializeField] private float playerSpeed   = 2.0f;
    [SerializeField] private float jumpHeight    = 1.0f;
    [SerializeField] private float gravityValue  = -9.81f;
    [SerializeField] private float rotationSpeed = 4f;

    private CharacterController controller;
    private Vector3             playerVelocity;
    private bool                groundedPlayer;
    private Transform           cameraMainTransform;

    /***** DIALOGUE *****/
    [SerializeField] private DialogueUI dialogueUI; // canvas goes in this
    public DialogueUI DialogueUI => dialogueUI;
    public IInteractable Interactable { get; set; }

    void Awake()
    {
        anim = this.gameObject.transform.GetChild(1).GetComponent<Animator>();
    }

    private void OnEnable() {
        movementControl.action.Enable();
        jumpControl.action.Enable();
    }

    private void OnDisable() {
        movementControl.action.Disable();
        jumpControl.action.Disable();
    }

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        cameraMainTransform = Camera.main.transform;
    }

    void Update()
    {
        if (dialogueUI.IsOpen) return; // prevent movement while dialogue

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movement = movementControl.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(movement.x, 0, movement.y);

        // mecanim
        anim.SetFloat ("Speed", move.magnitude);

        // to factor in camera direction
        move    = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
        move.y  = 0f;

        controller.Move(move * Time.deltaTime * playerSpeed);

        // changes the height position of the player
        if (jumpControl.action.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // rotation
        if(movement != Vector2.zero) {
            float targetAngle   = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg
                                  + cameraMainTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation  = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            anim.SetBool("Absorbing", true);
            Interactable?.Interact(this, anim);  // if Interactable != null

        }

    }
}
