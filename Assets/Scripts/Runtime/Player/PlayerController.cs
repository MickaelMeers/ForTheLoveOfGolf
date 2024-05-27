using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed;
    public float maxSpeed;
    public float rotationSpeed;
    public float gravityForce;

    [SerializeField, Range(0, 90)] float maxGroundAngle = 25f; //angle max du ce qu'est un sol

    [Header("References")]
    private InputManager inputs;
    private Rigidbody rb;

    public bool isGrounded;

    public Vector3 direction;
    public Vector3 gravity;
    public Vector3 normal;
    public Vector3 friction;
    public Vector3 acceleration;

    public float minGroundDotProduct;

    // Transparence de la balle en mode Aim
    public Material materialOpaque;
    public Material materialTransparent;

    private void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputs = GetComponent<InputManager>();

        GetComponent<MeshRenderer>().material = materialOpaque;

        OnValidate();
    }

    private void Update()
    {
        HandleInput();

        HandleDirection();
        HandleGravity();

        CheckGround();

        HandleNormal();
        HandleFriction();
        HandleAcceleration();

        LimitSpeed();
    }

    private void FixedUpdate()
    {
        HandleForces();
    }

    private void HandleInput()
    {
        Vector2 playerInput;

        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");

        playerInput = Vector2.ClampMagnitude(playerInput, 1);
    }

    private void HandleDirection()
    {
        float inputMouse = Input.GetAxisRaw("Mouse X");
        if (inputMouse != 0f)
        {
            float angle = Input.GetAxisRaw("Mouse X") * rotationSpeed;
            rb.velocity = Quaternion.AngleAxis(angle, Vector3.up) * rb.velocity; // La direction tourne avec le mouvement de la souris
        }

        if (rb.velocity.magnitude > 0.01)
        {
            direction = rb.velocity.normalized; // La direction de la balle est celle de la v�locit�
        }

        Debug.Assert(direction.magnitude > 0f);
    }

    private void HandleGravity()
    {
        gravity = new Vector3(0, -gravityForce, 0);
    }

    private void HandleNormal()
    {
        if (isGrounded)
        {
            normal *= gravity.magnitude;
        }
        else
        {
            normal = Vector3.zero;
        }
    }

    private void HandleFriction()
    {
        friction = Vector3.zero;
    }

    private void HandleAcceleration()
    {
        acceleration = direction * Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime; // Avance

        acceleration += Quaternion.AngleAxis(90, Vector3.up) * direction * moveSpeed * 100 * Input.GetAxisRaw("Horizontal") * Time.deltaTime; // Strafe

        acceleration = Vector3.ClampMagnitude(acceleration, 5f);
    }

    private void HandleForces()
    {
        Vector3 forces = (gravity + normal + acceleration + friction);

        rb.AddForce(forces, ForceMode.Acceleration);
    }

    float groundDetectionLength = 0.03f;
    private void CheckGround()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, transform.localScale.x * 0.5f + groundDetectionLength))
        {
            normal = hit.normal;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void LimitSpeed()
    {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }

    public void Boost(Vector3 direction,float power)
    {
        rb.AddForce(direction * power * 5f, ForceMode.Impulse);
    }

    /// <summary>
    /// Renvoie la balle en fonction du vecteur normal � la surface sur laquelle la balle est entr�e en collision.
    /// </summary>
    /// <param name="normal"></param>
    public void BumpFlipper(Vector3 normal)
    {
        float angle = Vector3.Angle(rb.velocity, normal);

        rb.velocity = Quaternion.AngleAxis(angle * 2, normal) * rb.velocity;
    }

    /// <summary>
    /// Projette la balle dans la direction donn�e en param�tre, en gardant la force de la balle
    /// </summary>
    /// <param name="direction"></param>
    public void BumpTrampoline(Vector3 direction)
    {
        rb.velocity = direction.normalized * rb.velocity.magnitude;
    }

    private void MakePlayerOpaque()
    {
        GetComponent<MeshRenderer>().material.DOFade(1, 0.5f).OnComplete(() => { GetComponent<MeshRenderer>().material = materialOpaque; });
    }

    private void MakePlayerTransparent()
    {
        GetComponent<MeshRenderer>().material = materialTransparent;
        GetComponent<MeshRenderer>().material.DOFade(0.2f, 0.5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + normal);
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.position + gravity);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + acceleration);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + friction);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + direction);
        //Gizmos.DrawLine(transform.position + direction, transform.position + direction + debugDirection);
    }
}