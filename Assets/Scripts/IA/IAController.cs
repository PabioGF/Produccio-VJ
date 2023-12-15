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
    protected Transform jugador;
    public float tiempoCambioDireccion = 3.0f;
    protected float tiempoActual;
    protected float distanciaDisparo = 10.0f;

    protected int direccion = 1;

    public Rigidbody2D myRB;
    protected bool hasDetected = false;
  
    protected Vector3 myVelocity;

    public int radioDeteccionSuelo;

    public Transform leftFoot, rightFoot;

    protected virtual void Start()
    {
        //Posición inicial del enemigo.
       // transform.position = new Vector3(puntoInicialX, transform.position.y, transform.position.z);
        jugador = player.transform;
        ReiniciarTemporizador();
        hasDetected = false;

        myRB = GetComponent<Rigidbody2D>();
        
    }

    protected virtual void Update()
    {
        myVelocity = myRB.velocity;

        if (VerificarLineaDeVision() || hasDetected)
        {
            Debug.Log("Entro");
            hasDetected = true;
        }

        if (!hasDetected)
        {
            Debug.Log(VerificarLineaDeVision());
            myVelocity = BasicMovement(myVelocity);
        }

        myRB.velocity = myVelocity;

    }



    private bool VerificarLineaDeVision()
    {
        Debug.Log("hola");
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

    private Vector3 BasicMovement(Vector3 myVelocity)
    {

        bool leftFootColliding = CheckFeetColliding(leftFoot);
        bool rightFootColliding = CheckFeetColliding(rightFoot);

        if (!leftFootColliding || !rightFootColliding)
        {
            CambiarDireccion();
            ReiniciarTemporizador();
        }

        myVelocity.x = velocidadMovimiento * direccion;

        tiempoActual -= Time.deltaTime;

        if (tiempoActual <= 0f)
        {
            CambiarDireccion();
            ReiniciarTemporizador();
        }

        return myVelocity;
    }

    private bool CheckFeetColliding(Transform feet)
    {
        Collider2D collider = Physics2D.OverlapCircle(feet.position, 0.2f, LayerMask.GetMask("Ground"));
        return collider != null;
        
    }

    private void ReiniciarTemporizador()
    {
        tiempoActual = tiempoCambioDireccion;
    }

    private void CambiarDireccion()
    {
        direccion *= -1;
    }

}
