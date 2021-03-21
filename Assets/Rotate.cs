using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>Do i really have to add a description to this one?</summary>
public class Rotate : MonoBehaviour
{
    public float x = 0;
    public float y = 0;
    public float z = 0;
    void FixedUpdate() {
        transform.Rotate(x * Time.deltaTime, y * Time.deltaTime, z * Time.deltaTime);
    }
}
