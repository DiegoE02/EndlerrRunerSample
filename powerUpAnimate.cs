using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpAnimate : MonoBehaviour
{
    public float arriba;
    public float abajo;
    bool subiendo;
    public float flotanteSpeed;
    // Start is called before the first frame update
    void Start()
    {
        subiendo = false;
        arriba = transform.position.y + arriba;
        abajo = transform.position.y - abajo;
    }

    // Update is called once per frame
    void Update()
    {
        Flotar();
    }

    void Flotar()
    {
        if (transform.position.y < arriba)
        {
            //Si el powerup está subiendo se suma a su posición Y
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
            //Si el powerup está bajando se resta a su posición Y
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
}
