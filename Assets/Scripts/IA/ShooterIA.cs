using UnityEngine;

public class ShooterIA : IAController
{
    public GameObject balaPrefab;
    [SerializeField] private GameObject _highBullet;
    [SerializeField] private GameObject _lowBullet;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _upperBulletProbability;
    [SerializeField] private Rigidbody2D _referencePoint;
    [SerializeField] private GameObject _pointer;
    [SerializeField] private float _bulletSpeed;

    private Animator myAnimator;
    private Rigidbody2D _rigidbody;
    private GameObject _player;
    private bool _upperBullet;
    private Vector2 _aimDirection;
    private bool _playerDetected;
    private bool _isDisarmed;


    protected override void Start()
    {
        base.Start();
        myAnimator = GetComponent<Animator>();
        _player = GameObject.Find("Player");
    }

    protected override void Update()
    {
        base.Update();
        Vector3 myVelocity = base.myVelocity;
        myRB.velocity = myVelocity;
        if (hasDetected)
        {
           

            // Shoot();
            myVelocity.x = 0;
            myAnimator.SetBool("stopMovement", true);
            myAnimator.SetTrigger("shoot");
        }
        else
        {
            CancelInvoke(nameof(SpawnBullet));
            myAnimator.SetBool("stopMovement", false);
        }

        myRB.velocity = myVelocity;
    }

    private void CalculateDirection()
    {
        _aimDirection = _player.transform.position - transform.position;
        float angle = Mathf.Atan2(_aimDirection.y, _aimDirection.x) * Mathf.Rad2Deg;
        _referencePoint.rotation = angle;
    }


  /* private void Shoot()
    {
        if (!_startShooting) return;

        _startShooting = false;
        InvokeRepeating(nameof(SpawnBullet), 0, _fireRate);
    }*/

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