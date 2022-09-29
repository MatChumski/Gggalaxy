using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptPlayer : MonoBehaviour
{

    [SerializeField] private GameObject bullet;

    [SerializeField] private int speed;

    [SerializeField] private float fireRate;
    [SerializeField] private float shotCooldown = 5;

    [SerializeField] private bool dashing = false;
    [SerializeField] private float dashDuration;    // Tiempo que dura el dash
    [SerializeField] private float timeDashing;     // Por cuánto tiempo ha hecho dash
    [SerializeField] private float dashCoolingTime; // Tiempo de enfriamiento
    [SerializeField] private float dashCooldown;    // Tiempo EN enfriamiento
    [SerializeField] private float dashForce;    // Multiplicador de impulso

    // Start is called before the first frame update
    void Start()
    {

    }


    private Vector3 position;
    Vector3 direction = Vector3.up;
    Vector3 dash;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) && !dashing)
        {
            direction = Vector3.up;
            transform.position += direction * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow) && !dashing)
        {
            direction = Vector3.down;
            transform.position += direction * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow) && !dashing)
        {
            direction = Vector3.left;
            transform.position += direction * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow) && !dashing)
        {
            direction = Vector3.right;
            transform.position += direction * speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && shotCooldown >= fireRate)
        {
            Shoot();
        }
        else if (shotCooldown <= fireRate)
        {
            shotCooldown += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashing == false && (dashCooldown >= dashCoolingTime))
        {
            Dash();
        } else if (dashCooldown <= dashCoolingTime)
        {
            dashCooldown += Time.deltaTime;
        }

        if (dashing && (timeDashing >= dashDuration))
        {
            StopDash();
        } else 
        {
            if (dashing && timeDashing <= dashDuration)
            {
                timeDashing += Time.deltaTime;
            }
        }
    }

    private void Shoot()
    {
        GameObject newBullet = Instantiate(bullet);
        newBullet.GetComponent<scriptBullet>().source = "player";
        newBullet.transform.position = transform.position;
        shotCooldown = 0;
    }

    private void Dash()
    {
        Debug.Log("dash");
        position = transform.position;
        dash = direction * dashForce;

        GetComponent<Rigidbody2D>().AddForce(dash, ForceMode2D.Impulse);
        dashing = true;
        dashCooldown = 0;
    }

    private void StopDash()
    {
        Debug.Log("stop dash");
        dashing = false;
        GetComponent<Rigidbody2D>().AddForce(-dash, ForceMode2D.Impulse);
        timeDashing = 0;
    }
}


