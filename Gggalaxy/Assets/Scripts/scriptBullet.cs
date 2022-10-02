using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptBullet : MonoBehaviour
{

    public float speed;
    public string source;
    private float limit;

    // Start is called before the first frame update
    void Start()
    {
        if (source == "player")
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * speed, ForceMode2D.Impulse);
            limit = 10f;
        }
        else if (source == "enemy")
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.down * speed, ForceMode2D.Impulse);
            GetComponent<SpriteRenderer>().color = Color.white;
            limit = -10f;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if ((source == "player" && transform.position.y >= limit) || (source == "enemy" && transform.position.y <= limit))
        {
            Destroy(this.gameObject);
        }
    }
}
