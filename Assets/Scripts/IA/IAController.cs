using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : MonoBehaviour
{
    public float velocidadMovimiento = 2.0f;
    public float puntoInicialX;
    public float puntoFinalX;
    public float distanciaDeteccion = 5.0f; // Distancia de detección del jugador.
    public GameObject player;
    private Transform jugador; // Referencia al jugador.
    private int direccion = 1;
    Rigidbody2D myRB;

    private void Start()
    {
        //Posición inicial del enemigo.
        transform.position = new Vector3(puntoInicialX, transform.position.y, transform.position.z);
        jugador = player.transform;

        myRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector3 myVelocity = myRB.velocity;

        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

        if (distanciaAlJugador <= distanciaDeteccion)
        {
            // Si el jugador está dentro de la zona de detección, mueve al enemigo hacia el jugador.
            Vector3 direccionJugador = (jugador.position - transform.position).normalized;
            myVelocity.x = velocidadMovimiento * direccionJugador.x;
            //transform.Translate(direccionJugador * velocidadMovimiento * Time.deltaTime);
        }
        else
        {
            // Mover al enemigo en la dirección actual.
            myVelocity.x = velocidadMovimiento * direccion;

            // Si el enemigo llega al punto final, cambia de dirección.
            if ((direccion == 1 && transform.position.x >= puntoFinalX) ||
                (direccion == -1 && transform.position.x <= puntoInicialX))
            {
                CambiarDireccion();
            }

        }
        myRB.velocity = myVelocity;
    }

    private void CambiarDireccion()
    {
        direccion *= -1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        LifeComponent life = collision.gameObject.GetComponent<LifeComponent>();
        if (life != null)
        {
            life.ApplyDamage(1f);
        }
    }
}
