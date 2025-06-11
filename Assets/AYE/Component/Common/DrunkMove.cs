using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrunkMove : MonoBehaviour
{
    public float horizontalRange = 0.2f;
    public float verticalRange = 0.1f;
    public float speed = 0.5f;
    float t = 0f;
    private void Update()
    {
        t += Time.deltaTime * speed;
        transform.localPosition = new Vector3((-0.5f + Mathf.PerlinNoise(100f, t)) * horizontalRange, (-0.5f + Mathf.PerlinNoise(200f, t)) * verticalRange, 0f);
    }
}
