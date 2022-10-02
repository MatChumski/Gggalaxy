using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptGameHandler : MonoBehaviour
{
    [SerializeField] private float startWave;

    public GameObject enemyImp;
    public float wave;

    public float enemySpeed;
    public List<GameObject> enemies;

    public List<GameObject> powerUps = new List<GameObject>();

    public float powerUpSpawnTime;
    public float powerUpSpawnCooldown;

    // Start is called before the first frame update
    void Start()
    {
        powerUpSpawnCooldown = 0;

        wave = startWave;
        StartWave();
    }

    // Update is called once per frame
    void Update()
    {
        // Cambia de oleada cuando no quedan enemigos en pantalla
        if (enemies.Count == 0)
        {
            wave += 1;
            StartWave();
        }

        // Controla la aparición de Power Ups
        if (powerUpSpawnCooldown >= powerUpSpawnTime)
        {
            CreatePowerUp();
            powerUpSpawnCooldown = 0;
        } else if (powerUpSpawnCooldown < powerUpSpawnTime)
        {
            powerUpSpawnCooldown += Time.deltaTime;
        }
    }

    /*
     * CREAR POWER UPS
     */
    public void CreatePowerUp()
    {
        int option = Random.Range(0, powerUps.Count);

        Instantiate(powerUps[option]);
    }

    /*
     * CREAR ENEMIGOS
     */
    public void CreateEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            enemies.Add(Instantiate(enemyImp));
        }
    }

    /*
     * INICIAR OLA
     * TODO: Crear una clase para las oleadas que permita generarlas
     * de una manera estandarizada
     */
    public void StartWave()
    {
        switch (wave)
        {
            case 1:
                float posY = 0;
                float posX = 0;
                Debug.Log("Wave 1");
                enemies.Clear();

                CreateEnemies(5);
                posY = 0.5f;
                posX = -8.4f;

                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].transform.position = new Vector3(posX, posY, 0);

                    enemies[i].GetComponent<scriptEnemy>().leftLimit = -8.5f;
                    enemies[i].GetComponent<scriptEnemy>().rightLimit = 8.5f;

                    enemies[i].GetComponent<scriptEnemy>().minSpeed = 4f;
                    enemies[i].GetComponent<scriptEnemy>().maxSpeed = 6f;

                    enemies[i].GetComponent<scriptEnemy>().fireAmount = 1;

                    posY++;
                    posX += 4;
                }
                break;
            default:
                break;
        }
    }

    /*
     * ELIMINAR ENEMIGO
     * Elimina al enemigo tanto del juego como de la lista de 
     * enemigos
     */
    public void KillEnemy(GameObject enemy)
    {
        Destroy(enemy);
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == enemy)
            {
                enemies.RemoveAt(i);
            }
        }
        Debug.Log("enemies left: " + enemies.Count);
    }
}
