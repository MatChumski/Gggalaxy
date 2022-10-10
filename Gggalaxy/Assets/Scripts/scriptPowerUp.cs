using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

public class scriptPowerUp : MonoBehaviour
{
    public float speed;
    public float limit;

    public float spawnX = -8.4f;
    public float spawnY = 6f;

    // Start is called before the first frame update

    /*
     * Al generar un PowerUp, este aparece en una posición aleatoria
     * desde la parte superior de la pantalla, y similar a una bala,
     * se le aplica un impulso hacia abajo
     */
    void Start()
    {
        transform.position = new Vector3(spawnX + Random.Range(0, 16), spawnY, 0);
        speed = Random.Range(5, 7);
        GetComponent<Rigidbody2D>().AddForce(Vector2.down * speed, ForceMode2D.Impulse);
        limit = -10f;
    }

    // Update is called once per frame
    void Update()
    {
        // Si supera el límite inferior, se destruye
        if (transform.position.y <= limit)
        {
            Destroy(this.gameObject);
        }
    }
}
