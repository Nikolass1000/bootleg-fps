using UnityEngine;
/// <summary>Destroys the GameObject after `time` seconds</summary>
public class Despawn : MonoBehaviour {
    public float time = 30;
    void Start() {
        Destroy(gameObject, time);
    }
}