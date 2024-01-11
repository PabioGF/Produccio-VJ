using UnityEngine;

public class NormalIA : IAController
{
    public float distanciaParada = 1.0f;
    public float tiempoEntreCombos = 5.0f;
    private float tiempoUltimoCombo;
    public GameObject lowTrigger;
    public GameObject highTrigger;
    private float tiempoActivos = 0.5f;

    private Animator myAnimator;
    private LifeComponent lifeComponent;
    private int contPunch;

    protected override void Start()
    {
        base.Start();
        myAnimator = GetComponent<Animator>();
        lifeComponent = GetComponent<LifeComponent>();
        tiempoUltimoCombo = -tiempoEntreCombos;
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
        // Calcular el tiempo transcurrido desde el último combo
        float tiempoDesdeUltimoCombo = Time.time - tiempoUltimoCombo;

        // Realizar combo si ha pasado el tiempo requerido
        if (tiempoDesdeUltimoCombo >= tiempoEntreCombos)
        {
            int ataqueAleatorio = 2;//Random.Range(1, 4);
           
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

            
        }
       

    }


    private void Combo1()
    {
        //puñetazo, puñetazo, puñetazo
        switch (contPunch)
        {
            case 1:
                myAnimator.SetTrigger("doPunch");
                break;
            case 2:
                myAnimator.SetTrigger("doPunch");
                break;
            case 3:
                myAnimator.SetTrigger("doPunch");
                tiempoUltimoCombo = Time.time;


                break;
        }

    }

    private void Combo2()
    {

        //puñetazo, puñetazo, patada

        switch (contPunch)
        {
            case 1:
                myAnimator.SetTrigger("doPunch");
                break;
            case 2:
                myAnimator.SetTrigger("doPunch");
                break;
            case 3:
                myAnimator.SetTrigger("doKick");
                tiempoUltimoCombo = Time.time;


                break;
        }
    }

    private void Combo3()
    {



        switch (contPunch)
        {
            case 1:
                myAnimator.SetTrigger("doKick");
                break;
            case 2:
                myAnimator.SetTrigger("doKick");
                
                break;
            case 3:
                myAnimator.SetTrigger("doPunch");
                tiempoUltimoCombo = Time.time;
                
                
                break;
        }
        //patada, patada, puñetazo, patada


    }


    private void CountPunch()
    {
       
        contPunch++;
        if(contPunch > 3)
        {
            contPunch = 0;
        }
        Debug.Log(contPunch);
    }

    private void setLowTrigger()
    {
        lowTrigger.SetActive(true);

        // Desactivar el trigger después de un tiempo
        //Invoke("DesactivarLowTrigger", tiempoEntreAtaques);
    }

    private void DesactivarLowTrigger()
    {
        lowTrigger.SetActive(false);
        myAnimator.SetTrigger("stopPunch");
    }

    private void setHighTrigger()
    {
        highTrigger.SetActive(true);
        myAnimator.SetTrigger("stopPunch");

        // Invoke("DesactivarHighTrigger", tiempoEntreAtaques);
    }

    private void DesactivarHighTrigger()
    {
        highTrigger.SetActive(false);
    }

}