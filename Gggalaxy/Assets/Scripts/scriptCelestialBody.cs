using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class scriptCelestialBody : MonoBehaviour
{

    [SerializeField][Range(0, 50)] float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
    }
}
