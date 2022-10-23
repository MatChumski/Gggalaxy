using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptBoss : MonoBehaviour
{
    private Vector3 direction = Vector3.right;

    [SerializeField] private GameObject bulletImp;

    public float speed;

    [SerializeField] private float fireRate;
    [SerializeField] private float health;
    private float fireCooldown;
    public float fireAmount;

    private scriptGameHandler handler;
    private bool alive;

    public float rightLimit;
    public float leftLimit;

    [SerializeField] private AudioSource audioEnemy;
    [SerializeField] private AudioClip clipExplosion;

    [SerializeField] private Sprite spriteExplosion;

    // Start is called before the first frame update
    void Start()
    {
        //Propiedades base del enemigo

        speed = 4f;
        handler = GameObject.Find("GameHandler").GetComponent<scriptGameHandler>();

        health = 30f;
        alive = true;

        fireRate = 0.5f;
        fireCooldown = 0;

        direction = Vector3.right;

        audioEnemy = GetComponentInChildren(typeof(AudioSource)) as AudioSource;

        if (handler.wave >= 20)
        {
            health = 45f;
            speed = 6f;
            fireRate = 0.35f;
        }
        else if (handler.wave >= 15)
        {
            speed = 6f;
            fireRate = 0.4f;
        }
        else if (handler.wave >= 10)
        {
            speed = 5f;
        }
    }

    private float deathTimer = 0f;
    private float timeDeath = 1.5f;

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            if (health <= 0)
            {
                audioEnemy.clip = clipExplosion;
                audioEnemy.Play();

                GetComponent<SpriteRenderer>().sprite = spriteExplosion;

                alive = false;                
            }

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
        else
        {
            if (deathTimer >= timeDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                deathTimer += Time.deltaTime;
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

        int fireAmount = Random.Range(1, 4 + 1);

        float xBullet = transform.position.x - fireAmount;
        float yBullet = transform.position.y - 1.5f;
        for (int i = 0; i < fireAmount; i++)
        {
            CreateBullet(xBullet, yBullet);
            xBullet += fireAmount / 1.75f;

            Debug.Log("shoot: " + fireAmount);
        }

        fireCooldown = 0;
    }

    // Muere en contacto con una bala de un jugador
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Bullet") && collision.GetComponent<scriptBullet>().source == "player" && alive)
        {
            Debug.Log("tag: " + collision.tag + collision.GetComponent<scriptBullet>().source);
            Destroy(collision.gameObject);
            health--;
        }
    }
}
