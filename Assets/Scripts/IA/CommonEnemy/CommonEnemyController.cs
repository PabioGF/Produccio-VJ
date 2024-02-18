using UnityEngine;

public class CommonEnemyController : IAController
{
    [Header("Common Enemy Params")]
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackDamage;
    [SerializeField] private float _minPlayerDistance;
    public float tiempoEntreCombos = 5.0f;
    
    private float tiempoUltimoCombo;

    protected override void Start()
    {
        base.Start();
        myAnimator = GetComponent<Animator>();
        tiempoUltimoCombo = -tiempoEntreCombos;
    }

    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null) return;

        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }

    public override void EnemyBasicMovement()
    {
        base.EnemyBasicMovement();

        if (!hasDetected) return;
        if (DistanceToPlayer() <= _minPlayerDistance) return;

        Vector3 direction = (_player.position - transform.position).normalized;
        myVelocity.x = velocidadMovimiento * direction.x;
        myRB.velocity = myVelocity; 
    }

    #region Attack

    public void Attack()
    {
        StandStill();
        myAnimator.SetInteger("Combo", Random.Range(0, 3));
        myAnimator.SetTrigger("Attack");
    }

    private void PerformAttack(int type)
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(_attackPoint.position, _attackRange, LayerMask.GetMask("Player"));

        if (playerCollider != null)
        {
            PlayerLifeComponent.AttackTypes attackType = (PlayerLifeComponent.AttackTypes)type;
            playerCollider.GetComponent<PlayerLifeComponent>().ReceiveHit(_attackDamage, attackType);
            Debug.Log(playerCollider.name + " has been hit");
        }
    }
    #endregion

    /*
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
    */

}