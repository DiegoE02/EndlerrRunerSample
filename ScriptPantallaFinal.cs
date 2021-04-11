using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptPantallaFinal : MonoBehaviour
{
    public GameObject puntosManager;
    private float points;
    public Text puntajeFinal;

    // Update is called once per frame
    void Update()
    {
        //Se muestra el puntaje final
        points = puntosManager.GetComponent<PuntajeController>().points;
        puntajeFinal.text = "" + Mathf.Floor(points);
    }

    public void Reiniciar() {
        //Se vuelve a cargar el juego (se llama a este void con un botón)
        SceneManager.LoadScene("Juego");
    }

    public void Salir() {
        //Se vuelve al menu (se llama a este void con un botón)
        SceneManager.LoadScene("Intro");
    }
}
