using UnityEngine;

public class NormalIA : IAController
{
    public float distanciaParada = 1.0f;
    public float tiempoEntreAtaques = 2.0f;
    public GameObject lowTrigger;
    public GameObject highTrigger;
    private float tiempoActivos = 0.5f;

    private Animator myAnimator;
    private LifeComponent lifeComponent;
    private float tiempoUltimoAtaque;
    private int contPunch;

    protected override void Start()
    {
        base.Start();
        myAnimator = GetComponent<Animator>();
        lifeComponent = GetComponent<LifeComponent>();
        tiempoUltimoAtaque = -tiempoEntreAtaques;
        contPunch = 0;
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
                //Debug.Log("Dist:" + distanciaAlJugadorX);
                myVelocity.x = 0f;
                myAnimator.SetBool("stopMovement", true);
                Pegar();
            }
            else
            {
                contPunch = 0;
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
            int ataqueAleatorio = 3;//Random.Range(1, 4);
            switch (ataqueAleatorio)
            {
                case 1:
                    Combo1();
                    break;
                case 2:
                    Combo2();
                    break;
                case 3:
                    Combo3();
                    break;
            }
            
           // tiempoUltimoAtaque = Time.time;

        }


            
    }


    private void Combo1()
    {
        //puñetazo, puñetazo, puñetazo
        Debug.Log(contPunch);
        if (contPunch == 0)
        {
            myAnimator.SetTrigger("doPunch");
        }
        
        if(contPunch == 3)
        {
            myAnimator.SetTrigger("stopPunch");
        }
        
    }

    private void Combo2()
    {
        //puñetazo, puñetazo, patada
        if (contPunch == 0)
        {
            myAnimator.SetTrigger("doPunch");
        }
        
        if (contPunch == 2)
        {
            myAnimator.SetTrigger("doKick");

        }
        if(contPunch == 3)
        {
            myAnimator.SetTrigger("stopPunch");
        }
    }

    private void Combo3()
    {
        //patada, patada, puñetazo
        if (contPunch == 0)
        {
            myAnimator.SetTrigger("doKick");
        }

        if (contPunch == 2)
        {
            myAnimator.SetTrigger("doPunch");

        }
        if (contPunch == 3)
        {
            myAnimator.SetTrigger("stopPunch");
        }
    }


    private void CountPunch()
    {
        contPunch++;
        
    }

    private void setLowTrigger()
    {
        lowTrigger.SetActive(true);

        // Desactivar el trigger después de un tiempo
        Invoke("DesactivarLowTrigger", tiempoEntreAtaques);
    }

    private void DesactivarLowTrigger()
    {
        lowTrigger.SetActive(false);
    }

    private void setHighTrigger()
    {
        highTrigger.SetActive(true);

     
        Invoke("DesactivarHighTrigger", tiempoEntreAtaques);
    }

    private void DesactivarHighTrigger()
    {
        highTrigger.SetActive(false);
    }
}