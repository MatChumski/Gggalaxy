using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptBullet : MonoBehaviour
{

    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * speed, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= 15)
        {
            Destroy(this.gameObject);
        }
    }
}
