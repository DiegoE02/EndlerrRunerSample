using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public GameObject centro;
    public GameObject izq;
    public GameObject der;
    private float CarrilMovement;
    bool Controlable;

    public Vector2 touchPos;
    public float swipeTH = 200;
    
    public float touchPosition = -1;
    public float touchJump = -1;
    private bool tocando = false;
    

    public float speed;
    public float speedAvance;
    private float ogSpeedAdvance;
    public float speedIncreaseAmmount;
    public float speedIncreaseTime;
    public float jumpHeight;
    public float downForce;

    private float timer;

    private Rigidbody rb;
    public bool onFloor;

    public float puntosMoneda;
    public GameObject manager;
    public GameObject soundManager;
    public float puntosModifier;

    public GameObject pantallaFinal;
    public GameObject textoPuntos;

    public float powerUpMaxTime;
    private float powerUpTimer;

    public bool modoIman;
    public bool modoInvulnerable;

    private Color ogColor;

    public GameObject meshRender;

    // Start is called before the first frame update
    void Start()
    {
        //Se guarda la velocidad original en la variable ogSpeedAdvance, para realizar operaciones
        //con ella más adelante
        ogSpeedAdvance = speedAvance;

        //Se busca el componente rigidbody y se guarda en una variable
        rb = GetComponent<Rigidbody>();

        //Se setea el carril a 1 (carril central)
        CarrilMovement = 1;

        Controlable = true;

        //Se guarda el color original en una variable para poder volver a él después de cambiarlo
        ogColor = meshRender.gameObject.GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        //Se llama al void touch manager para registrar el input del celular
        TouchManager();

        //Si el personaje es controlable, se chequean los inputs (del teclado y los swipes del celular), 
        //y se mueve al personaje acorde (Derecha, Izquierda, Salto y hacia abajo si se está en medio de un salto)
        if (Controlable)
        {
            transform.position += Vector3.forward * speedAvance * Time.deltaTime;

            if(touchPosition == 0 || Input.GetKeyDown("left"))
            {
                MoveLeft();
            }

            if (touchPosition == 1 || Input.GetKeyDown("right"))
            {
                MoveRight();
            }

            if (onFloor)
            {
                if (touchJump == 0 || Input.GetKeyDown("up"))
                {
                    Jump();
                }
            }
            else {
                if (touchJump == 1 || Input.GetKeyDown("down")) {
                    DownForce();
                }
            }
        }else{
            puntosModifier = 0;   
        }

        //Se chequea el carril del personaje (0 = izq, 1 = centro, 2 = der) y se le mueve acorde
        switch (CarrilMovement)
        {
            case 0:
                MoverLateral(izq);
                break;
            case 1:
                MoverLateral(centro);
                break;
            case 2:
                MoverLateral(der);
                break;
        }

        powerUpCD();
        Invulnerable();

        SpeedIncrease();
}

    void MoverLateral(GameObject destino)
        {
            if(!Controlable) return;

            //Se mueve al player a la posicion de destino, la cual se toma de uno de los tres objetos "izquierda", "centro" o "derecha"
            Vector3 Posicion = new Vector3(destino.transform.position.x, transform.position.y, transform.position.z);
            Vector3 Funcion = Vector3.MoveTowards(transform.position, Posicion, speed * Time.deltaTime) - transform.position;
            transform.position += Funcion;
        }

    void MoveRight()
    {
        //Se suma 1 al carril
        CarrilMovement++;
        //Se utiliza "clamp" para limitar el numero de carril entre 0 y 2
        CarrilMovement = Mathf.Clamp(CarrilMovement, 0, 2);
    }

    void MoveLeft()
    {
        //Se resta 1 al carril
        CarrilMovement--;
        //Se utiliza "clamp" para limitar el numero de carril entre 0 y 2
        CarrilMovement = Mathf.Clamp(CarrilMovement, 0, 2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "obstaculo")
        {
            //Si se colisiona con un obstaculo, el player deja de ser controlable, y se llama al void gameOver
            if (!modoInvulnerable)
            {
                Controlable = false;
                gameOver();
            }
        }

        
        if (collision.gameObject.tag == "Suelo" || collision.gameObject.tag == "PisoFlotante")
        {
            //Si se choca con el suelo, onFloor se setea a true, para poder volver a saltar
            onFloor = true;
        }

        if (collision.gameObject.tag == "Moneda") {
            //Si se choca con una moneda, se suma su valor a los puntos (en el objeto manager), se
            //reproduce el sonido correspondiente, y se destruye la moneda
            manager.GetComponent<PuntajeController>().points += puntosMoneda;
            soundManager.GetComponent<SoundManager>().Monedas();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Iman") {
            //Si se choca con el iman: arranca a contar el timer del power up, se activa el modo iman,
            //se desactiva el modo invulnerable (para no tener dos powerups a la vez), se destruye el iman,
            //suena el sonido correspondiente y se cambia el color del personaje a Rojo
            powerUpTimer = 0;
            modoIman = true;
            modoInvulnerable = false;
            Destroy(collision.gameObject);
            soundManager.GetComponent<SoundManager>().PowerUp1();
            meshRender.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }

        if (collision.gameObject.tag == "Invulnerable")
        {
            //Si se choca con la estrella: arranca a contar el timer del power up, se activa el modo invulnerable,
            //se desactiva el modo iman (para no tener dos powerups a la vez), se destruye la estrella,
            //suena el sonido correspondiente y se cambia el color del personaje a Amarillo
            powerUpTimer = 0;
            modoInvulnerable = true;
            modoIman = false;
            Destroy(collision.gameObject);
            soundManager.GetComponent<SoundManager>().PowerUp2();
            meshRender.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Suelo" || collision.gameObject.tag == "PisoFlotante")
        {
            //Se setea onFloor a true para poder volver a saltar
            onFloor = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Suelo" || collision.gameObject.tag == "PisoFlotante")
        {
            //Si el personaje deja de tocar el suelo se setea onFloor a false, para que no pueda saltar en el aire
            onFloor = false;
        }
    }

    void Jump() 
    {
        //Cuando el personaje salta se le agrega una fuerza vertical multiplicada por la variable jumpHeight
        rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        //Se setea onFloor a false para que no pueda volver a saltar en el aire
        onFloor = false;
    }

    void DownForce() {
        //Si el personaje toca o swipea hacia abajo en medio de un salto, se le aplica una fuerza hacia abajo, para
        //poder caer más rapido (y controlar mejor el salto a velocidades más altas)
        rb.AddForce(Vector3.down * jumpHeight, ForceMode.Impulse);
    }

    void gameOver() {
        //Se activa el UI de la pantalla de gameover, y se desactiva el UI de los puntos (porque aparecen ya en la pantalla de game over)
        pantallaFinal.SetActive(true);
        textoPuntos.SetActive(false);
    }

    void powerUpCD() {
        //Si el personaje tiene un powerup, se le suma a un timer; cuando este llega al máximo (10 segundos), el personaje pierde el
        //powerup, y vuelve a su color original
        if (modoIman || modoInvulnerable)
        {
            if (powerUpTimer < powerUpMaxTime)
            {
                powerUpTimer += Time.deltaTime;
            }
            else
            {
                modoIman = false;
                modoInvulnerable = false;
                meshRender.gameObject.GetComponent<Renderer>().material.color = ogColor;
            }
        }
    }

    void Invulnerable() {
        if (modoInvulnerable)
        {
            //Si el personaje está en modo invulnerable, se ignoran las colisiones con la capa de obstáculos
            //y se activan las colisiones con la capa de puentes magicos
            Physics.IgnoreLayerCollision(9, 8, true);
            Physics.IgnoreLayerCollision(9, 10, false);
        }
        else {
            //Si el personaje no está en modo invulnerable, se activan las colisiones con la capa de obstáculos
            //y se ignoran las colisiones con la capa de puentes magicos
            Physics.IgnoreLayerCollision(9, 8, false);
            Physics.IgnoreLayerCollision(9, 10, true);
        }
    }

    void SpeedIncrease() {
        //Se utiliza un timer interno, que cada 5 segundos incrementa la velocidad del personaje
        timer += Time.deltaTime;

        if (timer > speedIncreaseTime) {
            timer = 0;
            speedAvance += speedIncreaseAmmount;
            speed += speedIncreaseAmmount;

            //Se le suma un valor a "puntosModifier" acorde a la velocidad del jugador, de forma que 
            //el aumento de puntos por segundo sea proporcional al aumento de velocidad
            puntosModifier += 1 / ogSpeedAdvance;
        }
    }

    void TouchManager() {
        
        touchPosition = -1;
        touchJump = -1;

        if (Input.touchCount > 0)
        {
            //Si el touchCount (integrado) es mayor a 0 (por lo que se estaría tocando la pantalla)
            //se guarda el toque en una variable
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) {
                //Si la phase del toque es "began" (o sea, cuando se toca por primera vez)
                //se guarda la posicion en pantalla del toque, y se setea la variable "tocando" a true

                touchPos = touch.position;
                tocando = true;

            }

            if (touch.phase == TouchPhase.Moved) {
                if (tocando) {
                    //Si la phase del toque es "moved" (o sea que la posicion del dedo se movió):

                    //Si la posicion del toque en X es mayor a la posicion original, significa que se swipeó a la derecha
                    if (touch.position.x > touchPos.x + swipeTH) {
                        touchPosition = 1;
                        touchPos = touch.position;
                        tocando = false;
                    }

                    //Si la posicion del toque en X es menor a la posicion original, significa que se swipeó a la izquierda
                    if (touch.position.x < touchPos.x - swipeTH)
                    {
                        touchPosition = 0;
                        touchPos = touch.position;
                        tocando = false;
                    }

                    //Si la posicion del toque en Y es mayor a la posicion original, significa que se swipeó hacia arriba
                    if (touch.position.y > touchPos.y + swipeTH)
                    {
                        touchJump = 0;
                        touchPos = touch.position;
                        tocando = false;
                    }

                    //Si la posicion del toque en Y es menor a la posicion original, significa que se swipeó hacia abajo
                    if (touch.position.y < touchPos.y - swipeTH)
                    {
                        touchJump = 1;
                        touchPos = touch.position;
                        tocando = false;
                    }
                }
            }  
        }
    }
}
