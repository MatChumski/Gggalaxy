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
        speed = Random.Range(minSpeed, maxSpeed);
        handler = GameObject.Find("GameHandler").GetComponent<scriptGameHandler>();
        alive = true;

        fireRate = Random.Range(0.5f, 1.5f);
        fireCooldown = 0;

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
    if (transform.position.y >= posY)
    {
        transform.position += Vector3.down * 5 * Time.deltaTime;
    }
    else
    {
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
    void CreateBullet(float xBullet, float yBullet)
    {
        GameObject bullet = Instantiate(bulletImp);
        scriptBullet bulletScript = bullet.GetComponent<scriptBullet>();
        bulletScript.source = "enemy";
        bulletScript.speed = 5f;
        bullet.transform.position = new Vector3(xBullet, yBullet, 0);
    }

    switch (fireAmount)
    {
        case 1:
            CreateBullet(transform.position.x, transform.position.y);
            break;
        case 2:
            float xBullet = transform.position.x - 0.25f;
            float yBullet = transform.position.y;
            for (int i = 0; i < fireAmount; i++)
            {
                CreateBullet(xBullet, yBullet);
                xBullet += 0.5f;
            }
            break;
    }

    fireCooldown = 0;
}

private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Bullet") && collision.GetComponent<scriptBullet>().source == "player" && alive)
    {
        alive = false;
        handler.KillEnemy(gameObject);
        Destroy(collision.gameObject);
    }
}
}
