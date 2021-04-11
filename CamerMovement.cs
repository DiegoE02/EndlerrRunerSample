using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CamerMovement : MonoBehaviour
{
    public GameObject player;
    private float desX;
    private float desY;
    private float desZ;
    public float minPosY;
    // Start is called before the first frame update
    void Start()
    {
        //Setea el desfazaje inicial con respecto al jugador, para mantenerse a esa distancai
        //durante el resto del juego
        //(Se decidió usar este método en vez de hacer a la camara hija del player, para tener
        //más control sobre su movimiento, por ejemplo limitar la altura de la misma)

        desX = transform.position.x - player.transform.position.x;
        desY = transform.position.y - player.transform.position.y;
        desZ = transform.position.z - player.transform.position.z;

        //Se setea la posición minima en Y que tendrá la camara, siendo esta la posicion inicial. Se usa
        //esta variable para que la camara no baje cuando el personaje se cae por un agujero
        minPosY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

        //Se setea la camara a la posicion del player + el desfasaje inicial
        //Se utiliza un "Clamp" para limitar la posicion en Y utilizando la variable minPosY
        float posY = Mathf.Clamp(player.transform.position.y + desY, minPosY, 99999);
        Vector3 posicion = new Vector3(player.transform.position.x + desX, posY , player.transform.position.z + desZ);
        transform.position = posicion;
    }
}
