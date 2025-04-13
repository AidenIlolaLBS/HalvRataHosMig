using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeignt;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    public float timeStepInterval = 0.75f;
    float time = 0;

    Rigidbody rb;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }


    // Update is called once per frame
    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeignt * 0.5f + 0.2f, whatIsGround);

        MyInput();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (verticalInput != 0 || horizontalInput != 0)
        {
            time = Time.deltaTime;
            if (time > timeStepInterval)
            {
                time = 0;
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<AudioManager>().StartSFX(SoundType.WalkingSound);
            }
        }

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }
}
