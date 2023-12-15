using UnityEngine;

public class NormalIA : IAController
{
    public float distanciaParada = 1.0f;
    public float tiempoEntreAtaques = 2.0f;

    private Animator myAnimator;
    private LifeComponent lifeComponent;
    private float tiempoUltimoAtaque;

    protected override void Start()
    {
        base.Start();
        myAnimator = GetComponent<Animator>();
        lifeComponent = GetComponent<LifeComponent>();
        tiempoUltimoAtaque = -tiempoEntreAtaques;
    }

    protected override void Update()
    {
        base.Update();
        Vector3 myVelocity = base.myVelocity;
        
        if (hasDetected)
        {

            float distanciaAlJugadorX = Mathf.Abs(transform.position.x - jugador.position.x);

            
            if (distanciaAlJugadorX <= distanciaParada)
            {
                Debug.Log("Dist:" + distanciaAlJugadorX);
                myVelocity.x = 0f;
                myAnimator.SetBool("stopMovement", true);
                Pegar();
            }
            else
            {
                
                Vector3 direccionJugador = (jugador.position - transform.position).normalized;
                myVelocity.x = velocidadMovimiento * direccionJugador.x;
                myAnimator.SetBool("stopMovement", false);
            }

        }

        myRB.velocity = myVelocity;
    }


    private void Pegar()
    {
        if (Time.time - tiempoUltimoAtaque >= tiempoEntreAtaques)
        {
            int ataqueAleatorio = Random.Range(0, 2);

            if (ataqueAleatorio == 0)
            {
                RealizarAtaquePorArriba();
                Debug.Log("Pega arriba");

            }
            else if (ataqueAleatorio == 1)
            {
                RealizarAtaquePorAbajo();
                Debug.Log("Pega abajo");

            }

            tiempoUltimoAtaque = Time.time;

        }
            
    }


    private void RealizarAtaquePorArriba()
    {
        // Lógica para el ataque por arriba y aplicar daño al jugador
        if (lifeComponent != null)
        {
            float damage = 10f; // Ajusta según tus necesidades
            lifeComponent.ReceiveHit(damage, LifeComponent.AttackTypes.HighAttack);
        }
    }

    private void RealizarAtaquePorAbajo()
    {
        // Lógica para el ataque por abajo y aplicar daño al jugador
        if (lifeComponent != null)
        {
            float damage = 10f; // Ajusta según tus necesidades
            lifeComponent.ReceiveHit(damage, LifeComponent.AttackTypes.LowAttack);
        }
    }
}