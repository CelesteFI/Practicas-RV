using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Movimiento : MonoBehaviour
{
    public float velocidad = 3f;
    public float multiplicadorCorrer = 2f;
    public float gravedad = -9.81f;
    public float alturaSalto = 1.5f;
    public float velocidadRotacion = 90f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isRunning = false;

    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private float rotationInput;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputActions.Player.Run.performed += ctx => isRunning = true;
        inputActions.Player.Run.canceled += ctx => isRunning = false;

        inputActions.Player.Jump.performed += ctx => Salto();

        inputActions.Player.Rotate.performed += ctx => rotationInput = ctx.ReadValue<float>();
        inputActions.Player.Rotate.canceled += ctx => rotationInput = 0f;
    }

    void OnEnable() { inputActions.Enable(); }
    void OnDisable() { inputActions.Disable(); }

    void Salto()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(alturaSalto * -2f * gravedad);
        }
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        float velocidadActual = isRunning ? velocidad * multiplicadorCorrer : velocidad;
        controller.Move(move * velocidadActual * Time.deltaTime);

        transform.Rotate(Vector3.up * rotationInput * velocidadRotacion * Time.deltaTime);

        velocity.y += gravedad * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}