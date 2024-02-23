using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;
    private bool isGrounded = false;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Verificar si el personaje está en el suelo
        if (controller.isGrounded)
        {
            isGrounded = true;
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }
        else
        {
            isGrounded = false;
        }

        // Aplicar gravedad al personaje
        moveDirection.y -= gravity * Time.deltaTime; //Puede que aqui este el problema de saltar

        // Mover al personaje
        if (isGrounded)
        {
            controller.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            controller.Move(Vector3.down * gravity * Time.deltaTime);
        }

        // Rotar la cabeza del personaje con el ratón
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        transform.Rotate(Vector3.up, mouseX);
        transform.Rotate(Vector3.left, mouseY);
    }
}
