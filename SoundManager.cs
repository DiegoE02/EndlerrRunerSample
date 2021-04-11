using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
	public  AudioClip monedas;
	public  AudioClip powerUp1;
	public  AudioClip powerUp2;
	static AudioSource audioSrc;
    // Start is called before the first frame update
    void Start()
    {
        audioSrc=GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Monedas(){
        //Se reproduce el sonido de las monedas
    	audioSrc.PlayOneShot(monedas);
    }
    public void PowerUp1(){
        //Se reproduce el sonido del powerup 1
        audioSrc.PlayOneShot(powerUp1);
    }
    public void PowerUp2(){
        //Se reproduce el sonido del powerup 2
        audioSrc.PlayOneShot(powerUp2);
    }
}
