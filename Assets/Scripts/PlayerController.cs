using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] public float forwardSpeed = 10f;
    [SerializeField] float laneDistance = 3f; // Distance between lanes
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float sideSpeed = 5f;

    // starts in the middle
    int currentLane = 1; // 0 = Left, 1 = Middle, 2 = Right
    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Lane input
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (currentLane > 0) currentLane--;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (currentLane < 2) currentLane++;
        }
        // jump logic, ensures players can't jump while in mid air
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Mathf.Abs(rb.linearVelocity.y) < 0.01f) rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }
    }

    void FixedUpdate()
    {
        // forward movement
        Vector3 velocity = rb.linearVelocity;
        velocity.z = forwardSpeed;
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, forwardSpeed);

        // Move position position based on current lane
        float targetX = (currentLane - 1) * laneDistance;
        float deltaX = targetX - rb.position.x;
        velocity.x = deltaX * sideSpeed;

        rb.linearVelocity = velocity;
    }

    public void SetForwardSpeed(float newSpeed)
    {
        forwardSpeed = newSpeed;
    }
}
