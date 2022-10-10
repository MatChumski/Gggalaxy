using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptGameHandler : MonoBehaviour
{
    [SerializeField] private float startWave;

    public scriptWaves waveHandler;
    public List<Wave> waveTemplates;

    public GameObject enemyImp;
    public float wave;
    public Wave curWave;

    public float enemySpeed;
    public List<GameObject> enemies;

    public List<GameObject> powerUps = new List<GameObject>();

    public float powerUpSpawnTime;
    public float powerUpSpawnCooldown;

    // Start is called before the first frame update
    void Start()
    {
        powerUpSpawnCooldown = 0;

        waveTemplates = new List<Wave>()
        {
            // Wave 1
            new Wave(
                // CoordsXY
                new List<float[]>()
                {
                    new float[] {-8.4f, 0},
                    new float[] {-4.4f, 1f},
                    new float[] {0f, 2f},
                    new float[] {4.4f, 3f},
                    new float[] {8.4f, 4f},
                },
                // LimitsLR
                new List<float[]>()
                {
                    new float[] {-8.5f, 8.5f},
                    new float[] {-8.5f, 8.5f},
                    new float[] {-8.5f, 8.5f},
                    new float[] {-8.5f, 8.5f},
                    new float[] {-8.5f, 8.5f},
                },
                // Speeds
                new List<float>()
                {
                    5f,
                    5f,
                    5f,
                    5f,
                    5f
                },
                // FireAmounts
                new List<float>()
                {
                    1,
                    1,
                    1,
                    1,
                    1
                }
                ),
            // Wave 2
            new Wave(
                // CoordsXY
                new List<float[]>()
                {
                    new float[] {-8.4f, 4f},
                    new float[] {-4.4f, 2f},
                    new float[] {-2.4f, 0f},
                    new float[] {2.4f, 0f},
                    new float[] {4.4f, 2f},
                    new float[] {8.4f, 4f}
                },
                // LimitsLR
                new List<float[]>()
                {
                    new float[] {-8.5f, 0f},
                    new float[] {-8.5f, 0f},
                    new float[] {-8.5f, 0f},
                    new float[] {0f, 8.5f},
                    new float[] {0f, 8.5f},
                    new float[] {0f, 8.5f}
                },
                // Speeds
                new List<float>()
                {
                    5f,
                    5f,
                    5f,
                    5f,
                    5f,
                    5f
                },
                // FireAmounts
                new List<float>()
                {
                    1,
                    1,
                    1,
                    1,
                    1,
                    1
                }
                ),
            // Wave 3
            new Wave(
                // CoordsXY
                new List<float[]>()
                {
                    new float[] {-8.4f, 0f},
                    new float[] {-2.4f, 1f},
                    new float[] {2.4f, 2f},
                    new float[] {8.4f, 3f},
                    new float[] {-8.4f, 4f},
                    new float[] {8.4f, 4f}
                },
                // LimitsLR
                new List<float[]>()
                {
                    new float[] {-8.5f, 8.5f},
                    new float[] {-8.5f, 8.5f},
                    new float[] {-8.5f, 8.5f},
                    new float[] {-8.5f, 8.5f},
                    new float[] {-8.5f, 0f},
                    new float[] {0f, 8.5f}
                },
                // Speeds
                new List<float>()
                {
                    6f,
                    6f,
                    6f,
                    6f,
                    7f,
                    7f
                },
                // FireAmounts
                new List<float>()
                {
                    1,
                    1,
                    1,
                    1,
                    2,
                    2
                }
                ),
            // Wave 4
            new Wave(
                // CoordsXY
                new List<float[]>()
                {
                    new float[] {-8.5f, 0},
                    new float[] {-6.5f, 0.6f},
                    new float[] {-4.5f, 1.2f},
                    new float[] {-2.5f, 1.8f},
                    new float[] {-0.5f, 2.4f},
                    new float[] {2.5f, 3f},
                    new float[] {4.5f, 3.6f},
                    new float[] {6.5f, 4.2f}
                },
                // LimitsLR
                new List<float[]>()
                {
                    new float[] {-8.5f, 8.5f},
                    new float[] {-8.5f, 8.5f},
                    new float[] {-8.5f, 8.5f},
                    new float[] {-8.5f, 8.5f},
                    new float[] {-8.5f, 8.5f},
                    new float[] {-8.5f, 8.5f},
                    new float[] {-8.5f, 8.5f},
                    new float[] {-8.5f, 8.5f}
                },
                // Speeds
                new List<float>()
                {
                    6f,
                    6f,
                    6f,
                    6f,
                    6f,
                    6f,
                    6f,
                    6f
                },
                // FireAmounts
                new List<float>()
                {
                    1,
                    2,
                    1,
                    2,
                    1,
                    2,
                    1,
                    2,
                }
                )

        };

        wave = startWave;
        curWave = waveTemplates[(int)wave - 1];
        StartWave();
    }

    // Update is called once per frame
    void Update()
    {
        // Cambia de oleada cuando no quedan enemigos en pantalla
        if (curWave.enemies.Count == 0)
        {
            enemies.Clear();
            wave += 1;
            StartWave();
        }

        // Controla la aparición de Power Ups
        if (powerUpSpawnCooldown >= powerUpSpawnTime)
        {
            CreatePowerUp();
            powerUpSpawnCooldown = 0;
        }
        else if (powerUpSpawnCooldown < powerUpSpawnTime)
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
        int thisWave;
        if (wave > waveTemplates.Count)
        {
            thisWave = Random.Range(1, waveTemplates.Count + 1) - 1;
        } else
        {
            thisWave = (int)wave - 1;
        }

        curWave = waveTemplates[thisWave];
        int ene = waveTemplates[thisWave].coordsXY.Count;

        CreateEnemies(ene);
        curWave.StartWave(enemies);        
    }

    /*
     * ELIMINAR ENEMIGO
     * Elimina al enemigo tanto del juego como de la lista de 
     * enemigos
     */
    public void KillEnemy(GameObject enemy)
    {
        Destroy(enemy);

        for (int i = 0; i < curWave.enemies.Count; i++)
        {
            if (curWave.enemies[i] == enemy)
            {
                curWave.enemies.RemoveAt(i);
            }
        }
        Debug.Log("enemies left: " + curWave.enemies.Count);
    }
}
