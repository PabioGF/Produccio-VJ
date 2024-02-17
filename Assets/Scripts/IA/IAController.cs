using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IAController : MonoBehaviour
{
    [SerializeField] protected Transform _player;
    [SerializeField] private Transform leftFoot, rightFoot;

    [Header("Shared enemy params")]
    [SerializeField] protected float velocidadMovimiento;
    [SerializeField] private float distanciaDeteccion;
    [SerializeField] float tiempoCambioDireccion;
    [SerializeField] private float _detectionExtraTime;
    [SerializeField] private float _velocityMultiplier;

    protected float tiempoActual;
    protected float distanciaDisparo = 10.0f;
    protected int direccion = 1;
    protected bool hasDetected = false;
    protected Vector3 myVelocity;

    protected Rigidbody2D myRB;
    private float _detectionTimer;
    private bool _isHit;
    private float _lastTimeHit;
    private bool _isFlipped;

    protected virtual void Start()
    {
        ReiniciarTemporizador();
        hasDetected = false;
        myRB = GetComponent<Rigidbody2D>();
    }
    public virtual void EnemyBasicMovement()
    {
        myVelocity = myRB.velocity;
        LookAtPlayer();
        DetectPlayer();
        IdleMove();

        if (Time.time - _lastTimeHit > 0.4)
        {
            ResetMovement();
        }
    }

    private void DetectPlayer()
    {
        // Lanzamos un rayo desde el enemigo hacia el jugador.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _player.position - transform.position, distanciaDisparo);
        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, (_player.position - transform.position).normalized * hit.distance, Color.green);
        }
        else
        {
            Debug.DrawRay(transform.position, (_player.position - transform.position).normalized * distanciaDisparo, Color.red);
        }

        // Si el rayo no choca con ning�n objeto, devuelve true.
        if (hit.collider != null && hit.collider.CompareTag("Player") && !hasDetected)
        {
            hasDetected = true;
            velocidadMovimiento *= _velocityMultiplier;
        }

        // Stops chasing the player if it has not been detectend during the established time
        if ((hit.collider == null || !hit.collider.CompareTag("Player")) && hasDetected)
        {
            _detectionTimer += Time.deltaTime;
            if (_detectionTimer > _detectionExtraTime)
            {
                hasDetected = false;
                velocidadMovimiento /= _velocityMultiplier;
                _detectionTimer = 0;
            }
        }
    }

    private void IdleMove()
    {
        if (hasDetected) return;

        bool leftFootColliding = CheckFeetColliding(leftFoot);
        bool rightFootColliding = CheckFeetColliding(rightFoot);

        if (!leftFootColliding || !rightFootColliding || tiempoActual <= 0f)
        {
            CambiarDireccion();
            ReiniciarTemporizador();
        }

        myVelocity.x = velocidadMovimiento * direccion;
        tiempoActual -= Time.deltaTime;
        myRB.velocity = myVelocity;
    }

    public void LookAtPlayer()
    {
        if (transform.position.x < _player.transform.position.x && !_isFlipped)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            _isFlipped = true;
        }
        else if (transform.position.x > _player.transform.position.x && _isFlipped)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            _isFlipped = false;
        }
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

    public void GetHit()
    {
        _isHit = true;
        myRB.gravityScale = 0;
        myRB.velocity = new(0, 0);
        _lastTimeHit = Time.time;
    }

    private void ResetMovement()
    {
        myRB.gravityScale = 3;
    }

    public float DistanceToPlayer()
    {
        return Mathf.Abs(transform.position.x - _player.position.x);
    }

}
