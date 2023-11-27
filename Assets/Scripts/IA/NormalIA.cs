using UnityEngine;

public class NormalIA : IAController
{
    public float distanciaDisparo = 10.0f;


    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        Vector3 myVelocity = base.myVelocity;
        myRB.velocity = myVelocity;
        if (hasDetected)
        {
            Vector3 direccionJugador = (jugador.position - transform.position).normalized;
            myVelocity.x = velocidadMovimiento * direccionJugador.x;
            Pegar();
        }
    }


    private void Pegar()
    {
       
    }
}