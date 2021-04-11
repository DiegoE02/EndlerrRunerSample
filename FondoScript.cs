using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FondoScript : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Setea la posición del fondo (cubo que está abajo del mapa, chequeando colisiones para cuando el player se caiga),
        //siguiendo la posicion Z del personaje
        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z);
    }
}
