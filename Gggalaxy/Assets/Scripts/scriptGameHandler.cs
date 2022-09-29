using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptGameHandler : MonoBehaviour
{

    public GameObject enemyImp;
    public float wave;
    public float enemySpeed;
    public List<GameObject> enemies;

    // Start is called before the first frame update
    void Start()
    {
        wave = 1;
        StartWave();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateEnemies(int amount)
    {
        for (int i = 1; i <= amount; i++)
        {
            enemies.Add(Instantiate(enemyImp));
        }
    }

    public void StartWave()
    {
        switch (wave)
        {
            case 1:
                Debug.Log("Wave 1");
                enemies.Clear();

                CreateEnemies(3);
                float posY = 0.5f;
                float posX = -8.4f;

                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].transform.position = new Vector3(posX, posY, 0);
                    posY = posY + 2;
                    posX = posX + 8;
                }
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
    }

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
