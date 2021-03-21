using UnityEngine;
/// <summary>A physical projectile that can be spawned by weapons</summary>
public class Projectile : MonoBehaviour
{
    public float minDamage = 7;
    [HideInInspector] public float dmg;
    public float maxDamage = 14;
    public bool destroyOnCollision = true;
    /// <summary>The lifetime of the projectile, it will be automatically destroyed when the time since it has first spawned reaches this number, it will also spawn the `destroyEffect` if there's one</summary>
    public float lifetime = 10;
    public float speed = 10;
    public float knockback = 1;
    float timer;
    public GameObject destroyEffect;
    Rigidbody r;
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        r = rb;
        if (rb) rb.velocity = transform.forward * speed;
        dmg = BootlegRandom.NextRange(minDamage, maxDamage);
        timer = lifetime;
    }
    void Update() {
        if (timer > 0) timer -= Time.deltaTime;
        if (timer <= 0) DestroyProjectile();
    }
    void OnCollisionEnter(Collision col) {
        Health h = col.collider.GetComponent<Health>();
        Rigidbody rb = col.collider.GetComponent<Rigidbody>();
        if (rb) rb.AddForceAtPosition(r.velocity * knockback, r.position, ForceMode.Impulse);
        if (h) h.TakeDamage(dmg);
        if (destroyOnCollision) DestroyProjectile();
    }
    void DestroyProjectile() {
        if (destroyEffect) Instantiate(destroyEffect, transform.position, Quaternion.Inverse(transform.rotation));
        Destroy(gameObject);
    }
}
