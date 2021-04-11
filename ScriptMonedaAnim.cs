using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptMonedaAnim : MonoBehaviour
{
    public float rotSpeed;

    public float arriba;
    public float abajo;
    bool subiendo;
    public float flotanteSpeed;

    public GameObject player;
    public float playerDistance;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        subiendo = false;
        arriba = transform.position.y + arriba;
        abajo = transform.position.y - abajo;
    }

    // Update is called once per frame
    void Update()
    {
        //Se gira a la moneda sobre su propio eje
        transform.Rotate(transform.up * rotSpeed * Time.deltaTime);

        Flotar();
        Iman();
    }

    void Flotar() {
        if (transform.position.y < arriba)
        {
            //Si la moneda está subiendo se suma a su posición Y
            if (subiendo)
            {
                transform.position += flotanteSpeed * transform.up * Time.deltaTime;
            }
        }
        else
        {
            //Si alcanzó la altura maxima empieza a bajar
            subiendo = false;
        }

        if (transform.position.y > abajo)
        {
            //Si la moneda está bajando se resta a su posición Y
            if (!subiendo)
            {
                transform.position += flotanteSpeed * -transform.up * Time.deltaTime;
            }
        }
        else
        {
            //Si alcanzó la altura minima empieza a subir
            subiendo = true;
        }
    }

    void Iman() {
        bool iman = player.GetComponent<PlayerMove>().modoIman;
        float distancia = Vector3.Distance(transform.position, player.transform.position);
        
        //Si el personaje está en modo iman, las monedas se mueven hacia la posicion del personaje
        if (iman) {
            if (distancia < playerDistance) {
                Vector3 Posicion = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
                Vector3 Funcion = Vector3.MoveTowards(transform.position, Posicion, speed * Time.deltaTime) - transform.position;
                transform.position += Funcion;
            }
        }
    }
}
