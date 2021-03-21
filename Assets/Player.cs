using UnityEngine;
using UnityEngine.UI;
/// <summary>Do i also have to explain this one?</summary>
public class Player : MonoBehaviour
{
    public float decceleration = 5;
    public LayerMask groundMask;
    public Transform groundCheck;
    public GunHolder holder;
    public float groundDistance = 0.2f;
    public int stockAmmo = 750;
    public float speed = 13;
    public float runSpeed = 25;
    [HideInInspector] public float curSpeed;
    public float jumpStrength = 5;
    public Transform cam;
    [HideInInspector] public bool isGrounded = true;
    [HideInInspector] public bool isRunning = false;
    public Text ammoText;
    public Text maxAmmoText;
    public Text weaponNameText;
    public Text stockAmmoText;
    [HideInInspector] public bool isMoving = false;
    Vector3 velocity;
    [HideInInspector] public Rigidbody rb;
    bool justLanded = false;
    void Start() {
        stockAmmoText.text = stockAmmo + "";
        curSpeed = speed;
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (isRunning) {
            curSpeed = runSpeed;
            if (holder.guns[holder.selected].isReady) {
               holder.guns[holder.selected].Down(null); 
            }
        } else curSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift) && isMoving && !isRunning && holder.guns[holder.selected].isReady) {
            isRunning = true;
            holder.guns[holder.selected].Down(null);
        }
        if ((!Input.GetKey(KeyCode.LeftShift) || !isMoving) && isRunning) {
            isRunning = false;
            holder.guns[holder.selected].Up();
        }
    }
    void FixedUpdate() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Mathf.Sqrt(jumpStrength * -2 * Physics.gravity.y) * Vector3.up, ForceMode.Impulse);
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        if (move.magnitude >= .05f) {
            isMoving = true;
        } else isMoving = false;
        rb.AddForce(move * curSpeed);
        Vector3 v = rb.velocity;
        v.Scale(Vector3.right + Vector3.forward);
        rb.velocity = (v * (1 - (decceleration * Time.deltaTime))) + Vector3.up * rb.velocity.y;
    }
}
