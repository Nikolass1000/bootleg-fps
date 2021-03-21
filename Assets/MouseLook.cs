using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 300f;
    public float normalSensitivity = 300f;
    public Transform playerBody;
    Player p;
    Vector3 motion = Vector3.zero;
    Vector3 oldPos;
    Vector3 originalPos;
    public GameObject weaponHolder;
    public float weaponBobbingYIntensity = 0.1f;
    public float weaponBobbingXIntensity = 0.1f;
    public float weaponBobbingYSpeed = 0.1f;
    public float weaponBobbingXSpeed = 0.1f;
    public float bobbingIntensity = 0.4f;
    public float bobbingSpeed = 0.1f;
    public float weaponYOffsetByVelocity = 0.01f;
    public float weaponYOffsetMax = 0.1f;
    public float cameraYOffsetByVelocity = 0.008f;
    public float cameraOffsetMax = 0.21f;
    float Xrotation = 0f;
    Vector3 weaponBasePos;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        originalPos = transform.localPosition;
        weaponBasePos = weaponHolder.transform.localPosition;
        p = playerBody.GetComponent<Player>();
    }
    void Update() {
        motion = playerBody.position - oldPos;
    }
    void LateUpdate() {
        oldPos = playerBody.position;
    }
    void FixedUpdate()
    {
        if (p.isMoving && p.isGrounded) {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos + (Vector3.up * ((Mathf.Sin(Time.time * bobbingSpeed * (p.curSpeed / p.speed)) * bobbingIntensity) + Mathf.Clamp(cameraYOffsetByVelocity * p.rb.velocity.y, -cameraOffsetMax, cameraOffsetMax))), 10 * Time.deltaTime);
            weaponHolder.transform.localPosition = Vector3.Lerp(weaponHolder.transform.localPosition, weaponBasePos + (Vector3.up * (Mathf.Sin(Time.time * weaponBobbingYSpeed * (p.curSpeed / p.speed)) + Mathf.Clamp(weaponYOffsetByVelocity * p.rb.velocity.y, -weaponYOffsetMax, weaponYOffsetMax)) * weaponBobbingYIntensity) + (Vector3.right * Mathf.Sin(Time.time * weaponBobbingXSpeed * (p.curSpeed / p.speed)) * weaponBobbingXIntensity), 10 * Time.deltaTime);
        } else {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos + (Vector3.up * Mathf.Clamp(cameraYOffsetByVelocity * p.rb.velocity.y, -cameraOffsetMax, cameraOffsetMax)), 10 * Time.deltaTime);
            weaponHolder.transform.localPosition = Vector3.Lerp(weaponHolder.transform.localPosition, weaponBasePos + (Vector3.up * Mathf.Clamp(weaponYOffsetByVelocity * p.rb.velocity.y, -weaponYOffsetMax, weaponYOffsetMax)), 10 * Time.deltaTime);
        }
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;

        Xrotation -= mouseY;
        Xrotation = Mathf.Clamp(Xrotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(Xrotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
