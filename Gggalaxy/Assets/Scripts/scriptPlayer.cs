using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UI;

public class scriptPlayer : MonoBehaviour
{

    [SerializeField] private GameObject bulletImp;

    [SerializeField] private float speed;   // Velocidad de movimiento

    [SerializeField] private float fireRate;            // Espacio entre los disparos
    [SerializeField] private float shotCooldown = 5;    // Enfriamiento del disparo
    [SerializeField] private float fireAmount;          // Cantidad de balas por disparo

    [SerializeField] private bool dashing = false;
    [SerializeField] private float dashDuration;    // Tiempo que dura el dash
    [SerializeField] private float timeDashing;     // Por cuánto tiempo ha hecho dash
    [SerializeField] private float dashCoolingTime; // Tiempo de enfriamiento
    [SerializeField] private float dashCooldown;    // Tiempo EN enfriamiento
    [SerializeField] private float dashForce;       // Multiplicador de impulso

    public float health;                // Salud
    public scriptLivesUI livesManager;  // Interfaz de las vidas
    
    public scriptGameHandler handler;   // Controlador del juego

    public GameObject shieldImp;    // Importa el GameObject del escudo
    private GameObject shield;      // El escudo

    private bool shieldActive;
    private bool speedActive;
    private bool tripleActive;

    // Start is called before the first frame update
    void Start()
    {
        health = 3;
        fireAmount = 1;

        livesManager = GameObject.Find("Lives").GetComponent<scriptLivesUI>();
        handler = GameObject.Find("GameHandler").GetComponent<scriptGameHandler>();

        shieldActive = false;
        speedActive = false;
        tripleActive = false;
    }

    Vector3 direction = Vector3.up;

    // Update is called once per frame
    void Update()
    {
        /*
         * MORIR
         */
        if (health <= 0)
        {
            Die();
        }

        /*
         * MOVIMIENTO
         */
        if (!dashing)
        {
            // UP
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                direction = Vector3.up;
                transform.position += direction * speed * Time.deltaTime;
            }
            // DOWN
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                direction = Vector3.down;
                transform.position += direction * speed * Time.deltaTime;
            }
            // LEFT
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                direction = Vector3.left;
                transform.position += direction * speed * Time.deltaTime;
            }
            // RIGHT
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                direction = Vector3.right;
                transform.position += direction * speed * Time.deltaTime;
            }
        }

        /*
         * DISPARO
         */
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && shotCooldown >= fireRate)
        {
            Shoot();
        }
        else if (shotCooldown <= fireRate)
        {
            shotCooldown += Time.deltaTime;
        }

        /*
         * DASH
         * El Dash se activa al presionar la tecla Shift Izquierda,
         * siempre y cuando no se esté dasheando, y el enfriamiento
         * se haya completado
         */
        if (Input.GetKeyDown(KeyCode.LeftShift) && !dashing && (dashCooldown >= dashCoolingTime))
        {
            Dash();
        }
        else if (dashCooldown <= dashCoolingTime)
        {
            dashCooldown += Time.deltaTime;
        }

        if (dashing && (timeDashing >= dashDuration))
        {
            StopDash();
        }
        else
        {
            if (dashing && timeDashing <= dashDuration)
            {
                timeDashing += Time.deltaTime;
            }
        }

        /*
         * ESCUDO
         * Si el escudo está activo, debe seguir al jugador
         */
        if (shieldActive)
        {
            shield.transform.position = transform.position;
        }

        /*
         * VELOCIDAD
         * Si la velocidad está activa, se debe apagar
         * cuando se llegue al tiempo límite
         */
        if (speedActive)
        {
            if (speedTimeActive >= speedDuration)
            {
                StopSpeed();
            } else
            {
                speedTimeActive += Time.deltaTime;
            }
        }

        /*
         * TRIPLE DISPARO
         * Si el triple disparo está activo, se debe 
         * apagar cuando se llegue al tiempo límite
         */
        if (tripleActive)
        {
            if (tripleTimeActive >= tripleDuration)
            {
                StopTriple();
            } else
            {
                tripleTimeActive += Time.deltaTime;
            }
        }
    }

    /*
     * DISPARO
     */
    private void Shoot()
    {
        void CreateBullet(float xBullet, float yBullet)
        {
            GameObject bullet = Instantiate(bulletImp);
            scriptBullet bulletScript = bullet.GetComponent<scriptBullet>();
            bulletScript.source = "player";
            bulletScript.speed = 10f;
            bullet.transform.position = new Vector3(xBullet, yBullet, 0);
        }

        switch (fireAmount)
        {
            case 1:
                CreateBullet(transform.position.x, transform.position.y);
                break;
            case 3:
                float xBullet = transform.position.x - 0.25f;
                float yBullet = transform.position.y;
                for (int i = 0; i < fireAmount; i++)
                {
                    CreateBullet(xBullet, yBullet);
                    xBullet += 0.25f;
                }
                break;
        }

        shotCooldown = 0;
    }

    /*
     * DASH
     */

    // Iniciar Dash
    private void Dash()
    {
        Debug.Log("dash");
        Vector3 dash = direction * dashForce;

        GetComponent<Rigidbody2D>().AddForce(dash, ForceMode2D.Impulse);
        dashing = true;
        dashCooldown = 0;
    }

    
    // Detener Dash
    private void StopDash()
    {
        Debug.Log("stop dash");
        dashing = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        rb.velocity = new Vector2(0, 0);

        timeDashing = 0;
    }

    /*
     * POWER UPS
     */

    // ESCUDO

    private void StartShield()
    {
        if (!shieldActive)
        {
            shieldActive = true;
            shield = Instantiate(shieldImp);
        }
    }
    private void StopShield()
    {
        shieldActive = false;
        Destroy(shield);
    }

    // VELOCIDAD

    private float speedDuration = 5f;
    private float speedTimeActive;
    [SerializeField][Range(1, 2)] private float speedMultiplier;
    private void StartSpeed()
    {
        if (!speedActive)
        {
            speedActive = true;
            speed *= speedMultiplier;
        }
        speedTimeActive = 0f;
    }
    private void StopSpeed()
    {
        speedActive = false;
        speed /= speedMultiplier;
    }

    // DISPARO TRIPLE

    private float tripleDuration = 5f;
    private float tripleTimeActive;
    private void StartTriple()
    {
        if (!tripleActive)
        {
            tripleActive = true;
            fireAmount = 3;
        }
        tripleTimeActive = 0;
    }
    private void StopTriple()
    {
        tripleActive = false;
        fireAmount = 1;
    }

    /*
     * MORIR
     */
    private void Die()
    {
        Destroy(gameObject);
        handler.status = "dead";
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!dashing)
        {
            if (collision.CompareTag("Bullet") && collision.GetComponent<scriptBullet>().source == "enemy")
            {
                if (shieldActive)
                {
                    StopShield();
                } else
                {
                    health--;
                    livesManager.ChangeSprite(health);
                }
                Destroy(collision.gameObject);

            }
        }

        if (collision.CompareTag("PowerUpShield"))
        {
            StartShield();
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("PowerUpSpeed"))
        {
            StartSpeed();
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("PowerUpTriple"))
        {
            StartTriple();
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (shieldActive)
            {
                StopShield();
            }
            else
            {
                health--;
                livesManager.ChangeSprite(health);
            }
            Destroy(collision.gameObject);
        }
    }
}


