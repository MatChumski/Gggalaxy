using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptParallax : MonoBehaviour
{
    [Range(-2, 2)] public float scrollSpeed;

    private float offset;
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        offset += (Time.deltaTime * scrollSpeed) / 10f;
        material.SetTextureOffset("_MainTex", new Vector2(0, offset));
    }
}
