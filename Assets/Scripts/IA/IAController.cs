using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : MonoBehaviour
{
    public float velocidadMovimiento = 2.0f;
    public float puntoInicialX;
    public float puntoFinalX;
    public float distanciaDeteccion = 5.0f;
    public GameObject player;
    private Transform jugador;
    public float tiempoCambioDireccion = 3.0f;
    public string type;
    public enum EnemyType { Shooter, Normal}
    public EnemyType typeenum;
        
    private float tiempoActual;
    private int direccion = 1;
    public float distanciaDisparo = 10.0f;
    Rigidbody2D myRB;
    bool hasDetected = false;
    public GameObject balaPrefab;


    private void Start()
    {
        //Posición inicial del enemigo.
        transform.position = new Vector3(puntoInicialX, transform.position.y, transform.position.z);
        jugador = player.transform;
        ReiniciarTemporizador();
        hasDetected = false;

        myRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector3 myVelocity = myRB.velocity;

        if(typeenum == EnemyType.Normal)
        {
            myVelocity = NormalIA(myVelocity);
        }
        else if(type == "shooter"){
            myVelocity = ShooterIA(myVelocity);
        }

        myRB.velocity = myVelocity;

    }


    private Vector3 ShooterIA(Vector3 myVelocity)
    {
        //Debug.Log(VerificarLineaDeVision());
        if (VerificarLineaDeVision() || hasDetected)
        {
            
            hasDetected = true;
            Disparar();
            myVelocity.x = 0;
        }
        else
        {
            if (!hasDetected)
            {

                myVelocity = BasicMovement(myVelocity);

            }

        }

        return myVelocity;
    }

    private void Disparar()
    {

        if (Time.time > tiempoActual)
        {
            GameObject bala = Instantiate(balaPrefab, transform.position, Quaternion.identity);
            bala.GetComponent<Rigidbody2D>().velocity = new Vector2(direccion * 5.0f, 0f);

            //Frecuencia de disparo.
            tiempoActual = Time.time + 1.0f; 
        }
    }

    private bool VerificarLineaDeVision()
    {
        // Lanzamos un rayo desde el enemigo hacia el jugador.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, jugador.position - transform.position, distanciaDisparo);
        Debug.Log("Rayo de visión lanzado.");
        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, (jugador.position - transform.position).normalized * hit.distance, Color.green);
        }
        else
        {
            Debug.DrawRay(transform.position, (jugador.position - transform.position).normalized * distanciaDisparo, Color.red);
        }
        

        // Si el rayo no choca con ningún objeto, devuelve true.
        return hit.collider != null && hit.collider.CompareTag("Player");
    }
    private Vector3 NormalIA(Vector3 myVelocity)
    {
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

        if (distanciaAlJugador <= distanciaDeteccion || hasDetected)
        {
            hasDetected = true;
            Vector3 direccionJugador = (jugador.position - transform.position).normalized;
            myVelocity.x = velocidadMovimiento * direccionJugador.x;

        }
        else
        {
            if (!hasDetected)
            {
                
               myVelocity = BasicMovement(myVelocity);

            }



        }

        return myVelocity;
    }

    private Vector3 BasicMovement(Vector3 myVelocity)
    {
        myVelocity.x = velocidadMovimiento * direccion;

        tiempoActual -= Time.deltaTime;

        if (tiempoActual <= 0f)
        {
            CambiarDireccion();
            ReiniciarTemporizador();
        }

        return myVelocity;
    }

    private void ReiniciarTemporizador()
    {
        tiempoActual = tiempoCambioDireccion;
    }

    private void CambiarDireccion()
    {
        direccion *= -1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerLifeComponent life = collision.gameObject.GetComponent<PlayerLifeComponent>();
        if (life != null)
        {
            life.ReceiveHit(1f, true);
        }
    }
}
