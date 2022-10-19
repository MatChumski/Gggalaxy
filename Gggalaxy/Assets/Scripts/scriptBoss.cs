using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptBoss : MonoBehaviour
{
    private Vector3 direction = Vector3.right;

    [SerializeField] private GameObject bulletImp;

    [SerializeField] private float speed;
    public float minSpeed;
    public float maxSpeed;

    [SerializeField] private float fireRate;
    [SerializeField] private float health;
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
        //minSpeed = 3f;
        //maxSpeed = 6f;

        speed = 4f;
        handler = GameObject.Find("GameHandler").GetComponent<scriptGameHandler>();

        health = 15f;
        alive = true;

        fireRate = 0.5f;
        fireCooldown = 0;
        //fireAmount = 4;

        direction = Vector3.right;

        rightLimit = 5f;
        leftLimit = -5f;
    }



    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            alive = false;
            Destroy(gameObject);
            
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
