using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    private Material material;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        float r = Mathf.PingPong(Time.time / 10, 0.72f + 0.7f);
        float g = Mathf.PingPong(Time.time / 10, 0.48f + 0.5f);
        float b = Mathf.PingPong(Time.time / 10, 0.21f + 0.2f);

		material.color = new Color(r, g, b);
    }
}
