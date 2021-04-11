using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class PuntajeController : MonoBehaviour
{
    public float points;
    public Text textoPuntos;
    public GameObject player;
    private float modifier;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Se suma delta Time al puntaje para sumarle 1 por segundo, y se muestra en pantalla 
        //(usando Floor para que solo se muestren numeros enteros)
        modifier = player.GetComponent<PlayerMove>().puntosModifier;
        points += Time.deltaTime * modifier;
        textoPuntos.text = "Puntos: " + Mathf.Floor(points);
    }
}
