using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpSpawner : MonoBehaviour
{
    public GameObject[] powerUps;
    public int frecuencia;
    // Start is called before the first frame update
    void Start()
    {
        //Se spawnea un power up random
        //Se usa la variable frecuencia para darle un margen de posibilidad en el que no spawnea ningun power up
        int numeroRandom = Random.Range(-frecuencia, powerUps.Length);

        if (numeroRandom > -1)
        {
            Instantiate(powerUps[numeroRandom], transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
