using UnityEngine;
/// <summary>An explosion that will damage all objects within range with `Health` attached to them and will also add force to them if they have a `Rigidbody` attached</summary>
public class Explosion : MonoBehaviour {
    public float range = 5;
    public float damage = 25;
    public float force = 25;
    void Start() {
        Collider[] cols = Physics.OverlapSphere(transform.position, range);
        foreach (Collider col in cols) {
            Debug.Log(col);
            Rigidbody rb = col.GetComponent<Rigidbody>();
            Health h = col.GetComponent<Health>();
            if (rb) {rb.AddExplosionForce(force, transform.position, range, 1, ForceMode.Impulse);Debug.Log(rb);}
            if (h) {h.TakeDamage(damage * (1 - (Vector3.Distance(col.transform.position, transform.position) / range)));Debug.Log(h);}
        }
    }
    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}