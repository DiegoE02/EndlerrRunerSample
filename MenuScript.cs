using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void Jugar() {
        //Va a la pantalla de juego (se llama a este void desde un botón)
        SceneManager.LoadScene("Juego");
    }

    public void Salir() {
        //Chau
        Application.Quit();
    }
}
