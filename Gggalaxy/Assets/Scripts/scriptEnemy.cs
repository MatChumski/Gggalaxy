using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptEnemy : MonoBehaviour
{
    private Vector3 direction = Vector3.right;

    [SerializeField] private GameObject bulletImp;

    [SerializeField] private float speed;
    private float minSpeed = 4f;
    private float maxSpeed = 6f;

    private float fireRate;
    private float fireCooldown;

    private scriptGameHandler handler;
    private bool alive;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        handler = GameObject.Find("GameHandler").GetComponent<scriptGameHandler>();
        alive = true;

        fireRate = Random.Range(0.5f, 1.5f);
        fireCooldown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (direction == Vector3.right)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            if (transform.position.x >= 8.5)
            {
                direction = Vector3.left;
            }
        }
        if (direction == Vector3.left)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            if (transform.position.x <= -8.5)
            {
                direction = Vector3.right;
            }
        }

        if (fireCooldown >= fireRate)
        {
            Shoot();            
        } else
        {
            fireCooldown += Time.deltaTime;
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletImp);
        bullet.GetComponent<scriptBullet>().source = "enemy";
        bullet.transform.position = transform.position;
        fireCooldown = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet" && collision.GetComponent<scriptBullet>().source == "player" && alive)
        {
            alive = false;
            handler.KillEnemy(gameObject);
        }
    }
}
