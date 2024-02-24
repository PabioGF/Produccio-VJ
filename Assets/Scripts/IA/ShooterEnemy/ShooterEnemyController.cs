using UnityEngine;

public class ShooterEnemyController : IAController
{
    [Header("Shooter Enemy Params")]
    [SerializeField] private GameObject _highBullet;
    [SerializeField] private GameObject _lowBullet;
    [SerializeField] private float _upperBulletProbability;
    [SerializeField] private GameObject _pointer;
    [SerializeField] private float _bulletSpeed;

    private Rigidbody2D _rigidbody;
    private bool _upperBullet;
    private Vector2 _aimDirection;

    protected override void Start()
    {
        base.Start();
        myAnimator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void EnemyBasicMovement()
    {
        base.EnemyBasicMovement();

        if (!hasDetected) return;

        Debug.Log(hasDetected);

        // If the player is too far, gets closer
        if (DistanceToPlayer() >= _attackRadius)
        {
            Vector3 direction = (_player.position - transform.position).normalized;
            myVelocity.x = velocidadMovimiento * direction.x;
            myRB.velocity = myVelocity;
        }
        // If it is within the range, shoots
        else
        {
            myVelocity.x = 0;
            myAnimator.SetTrigger("Shoot");
            myRB.velocity = myVelocity;
        } 
    }

    private void CalculateDirection()
    {
        _aimDirection = _player.transform.position - transform.position;
        float angle = Mathf.Atan2(_aimDirection.y, _aimDirection.x) * Mathf.Rad2Deg;
    }

    private void SpawnBullet()
    {
        GameObject bullet = Random.value > _upperBulletProbability ? _highBullet : _lowBullet;
        GameObject newBullet = Instantiate(bullet, _pointer.transform.position, _pointer.transform.rotation);
        newBullet.GetComponent<BulletScript>().SetDirection(_aimDirection);
    }

    private void Shoot()
    {
        CalculateDirection();
        SpawnBullet();
    }
}