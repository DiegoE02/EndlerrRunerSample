using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscenarioManager : MonoBehaviour
{
    public GameObject[] escenarioPrefabs;
    private float zSpawn = 0;
    public float largoTiles;
    public int cantidadTiles;
    public Transform playerTransform;
    private List<GameObject> tilesActivos = new List<GameObject>();
    public float playerSafeArea;

    // Start is called before the first frame update
    void Start()
    {
        //Al iniciar el juego se generan los tiles iniciales
        for (int i = 0; i < cantidadTiles; i++)
        {
            //Si i == 0, se genera el tile(0), esto se hace para que el primer tile en generarse
            //sea siempre el mismo (y por ejemplo no empiece el personaje justo en frente a un obstáculo)
            if (i == 0)
            {
                CreateTiles(0);
            }
            else
            {
                //Si i != 0 se genera un tile aleatorio
                CreateTiles(Random.Range(0, escenarioPrefabs.Length));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Si el player sobrepasa una distancia determinada, se crea un nuevo tile adelante del jugador, 
        //y se elimina el primer tile en la lista (que el jugador ya pasó)
        if (playerTransform.position.z - playerSafeArea > zSpawn - (cantidadTiles * largoTiles)) {
            CreateTiles(Random.Range(0, escenarioPrefabs.Length));
            DeleteTiles();
        }
    }

    public void CreateTiles(int tileIndex) {
        //Se genera un prefab de tiles y este se agrega a una lista, para poder eliminarlo más adelante
        GameObject tile = Instantiate(escenarioPrefabs[tileIndex], transform.forward * zSpawn, transform.rotation);
        tilesActivos.Add(tile);
        zSpawn += largoTiles;
    }

    public void DeleteTiles() {
        //Se elimina el primer tile en la lista (el que está más atras) y se borra de la lista, para
        //que el siguiente quede primero (y se elimine la proxima vez)
        Destroy(tilesActivos[0]);
        tilesActivos.RemoveAt(0);
    }
}
