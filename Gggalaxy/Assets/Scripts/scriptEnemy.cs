using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class scriptEnemy : MonoBehaviour
{
    private Vector3 direction = Vector3.right;

    [SerializeField] private GameObject bulletImp;

    [SerializeField] private float speed;
    public float minSpeed;
    public float maxSpeed;

    private float fireRate;
    private float fireCooldown;
    public float fireAmount;

    private scriptGameHandler handler;
    private bool alive;

    public float rightLimit;
    public float leftLimit;

    public float posY;

    // Start is called before the first frame update
    void Start()
    {
        //Propiedades base del enemigo
        speed = Random.Range(minSpeed, maxSpeed);
        handler = GameObject.Find("GameHandler").GetComponent<scriptGameHandler>();
        alive = true;

        fireRate = Random.Range(0.5f, 1.5f);
        fireCooldown = 0;

        // Dependiendo de la oleada, es más o menos rápido
        if (handler.wave > 20)
        {
            speed *= 2f;
            fireRate /= 1.75f;
        }
        else if (handler.wave > 10)
        {
            speed *= 1.5f;
            fireRate /= 1.25f;
        }
        else if (handler.wave > 5)
        {
            speed *= 1.25f;
        }
    }



    // Update is called once per frame
    void Update()
    {
        /*
         * Si el enemigo todavía no ha llegado a su posición en Y correspondiente, 
         * se desplaza constantemente hacia abajo
         */
        if (transform.position.y >= posY)
        {
            transform.position += Vector3.down * 5 * Time.deltaTime;
        }
        else
        {
            // Según su dirección actual, se mueve a la derecha o izquierda
            if (direction == Vector3.right)
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
                if (transform.position.x >= rightLimit)
                {
                    direction = Vector3.left;
                }
            }
            if (direction == Vector3.left)
            {
                transform.position += Vector3.left * speed * Time.deltaTime;
                if (transform.position.x <= leftLimit)
                {
                    direction = Vector3.right;
                }
            }

            // Según el fireRate, dispara
            if (fireCooldown >= fireRate)
            {
                Shoot();
            }
            else
            {
                fireCooldown += Time.deltaTime;
            }
        }


    }

    private void Shoot()
    {
        // Genera una bala en una posición indicada
        void CreateBullet(float xBullet, float yBullet)
        {
            GameObject bullet = Instantiate(bulletImp);
            scriptBullet bulletScript = bullet.GetComponent<scriptBullet>();
            bulletScript.source = "enemy";
            bulletScript.speed = 5f;
            bullet.transform.position = new Vector3(xBullet, yBullet, 0);
        }

        // Hay diferentes modos de disparo dependiendo de la cantidad de balas
        switch (fireAmount)
        {
            case 1:
                CreateBullet(transform.position.x, transform.position.y - 0.8f);
                break;
            case 2:
                float xBullet = transform.position.x - 0.25f;
                float yBullet = transform.position.y - 1f;
                for (int i = 0; i < fireAmount; i++)
                {
                    CreateBullet(xBullet, yBullet);
                    xBullet += 0.5f;
                }
                break;
        }

        fireCooldown = 0;
    }

    // Muere en contacto con una bala de un jugador
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && collision.GetComponent<scriptBullet>().source == "player" && alive)
        {
            if (transform.position.y <= posY)
            {
                alive = false;
                handler.KillEnemy(gameObject);
                Destroy(collision.gameObject);
            }
        }
    }
}
