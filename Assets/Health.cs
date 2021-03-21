using UnityEngine;
/// <summary>Adds health system to the attached object, if it reaches zero the object will be destroyed and will spawn the `destroyEffect` if there's one, additionally the `destroyEffect` will inherit the velocity to make explosions cooler (lol)</summary>
public class Health : MonoBehaviour {
    public float maxHealth = 100;
    public GameObject destroyEffect;
    protected bool dead = false;
    protected float health;
    protected virtual void Start() {
        health = maxHealth;
    }
    public virtual void TakeDamage(float a) {
        health -= a;
        if (health <= 0) {
            Kill();
        }
    }
    public virtual void Kill() {
        if (dead) return;
        dead = true;
        if (destroyEffect) {
            Vector3 vel = GetComponent<Rigidbody>().velocity;
            GameObject o = Instantiate(destroyEffect, transform.position, transform.rotation);
            o.transform.localScale = transform.localScale;
            // Make the destroy effect inherit the current velocity for cooler explosions lol
            Rigidbody[] rbs = o.GetComponentsInChildren<Rigidbody>(false);
            foreach (Rigidbody rb in rbs) {
                rb.velocity = vel;
            }
        }
        Destroy(gameObject);
    }
}