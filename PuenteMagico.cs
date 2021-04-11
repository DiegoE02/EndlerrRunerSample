using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuenteMagico : MonoBehaviour
{
    public GameObject player;
    private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //Si el personaje está en modo invulnerable, se activa el render del puente. Si no, se mantiene invisible
        bool invulnerable = player.GetComponent<PlayerMove>().modoInvulnerable;
        if (invulnerable)
        {
            rend.enabled = true;
        }
        else {
            rend.enabled = false;
        }
    }
}
