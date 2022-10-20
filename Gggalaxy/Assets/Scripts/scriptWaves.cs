using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * OLEADAS
 * Las oleadas contienen múltiples listas, para generar
 * individualmente cada enemigo de la ola con las 
 * características necesarias
 * 
 * enemies: Enemigos
 * coordsXY: Coordenadas en X y en Y para el enemigo
 * limitsLR: Límites izquierdo y derecho 
 * speeds: Velocdiades
 * fireAmounts: Cantidad de balas
 */

public class Wave
{
    public List<GameObject> enemies;
    public List<float[]> coordsXY;
    public List<float[]> limitsLR;
    public List<float> speeds;
    public List<float> fireAmounts;
    public string type;

    public Wave(List<float[]> coordsXY, List<float[]> limitsLR, List<float> speeds, List<float> fireAmounts, string type)
    {
        this.coordsXY = coordsXY;
        this.limitsLR = limitsLR;
        this.speeds = speeds;
        this.fireAmounts = fireAmounts;
        this.type = type;
    }

    /*
     * INICIAR OLEADA
     * Se toman los parámetros establecidos en el constructor
     * Recibe una lista con los enemigos que se van a
     * instanciar
     */
    public void StartWave(List<GameObject> enemies)
    {
        this.enemies = enemies;

        if (type != "boss")
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].transform.position = new Vector3(coordsXY[i][0], 10, 0);

                scriptEnemy script = enemies[i].GetComponent<scriptEnemy>();

                script.posY = coordsXY[i][1];
                script.leftLimit = limitsLR[i][0];
                script.rightLimit = limitsLR[i][1];
                script.minSpeed = speeds[i] - 2f;
                script.maxSpeed = speeds[i];
                script.fireAmount = fireAmounts[i];
            }
        } 
        else
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].transform.position = new Vector3(coordsXY[i][0], coordsXY[i][1], 0);

                scriptBoss script = enemies[i].GetComponent<scriptBoss>();

                script.leftLimit = limitsLR[i][0];
                script.rightLimit = limitsLR[i][1];
                script.speed = speeds[i];
            }
        }

        
    }
}
public class scriptWaves : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
