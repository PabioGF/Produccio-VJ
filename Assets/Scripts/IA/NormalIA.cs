using UnityEngine;

public class NormalIA : IAController
{
    


    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        Vector3 myVelocity = base.myVelocity;
        
        if (hasDetected)
        {
            
            Vector3 direccionJugador = (jugador.position - transform.position).normalized;
            myVelocity.x = velocidadMovimiento * direccionJugador.x;
            Pegar();
        }

        myRB.velocity = myVelocity;
    }


    private void Pegar()
    {
       
    }
}