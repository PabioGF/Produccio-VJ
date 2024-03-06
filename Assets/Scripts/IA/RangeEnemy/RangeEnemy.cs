using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
private bool _isHit;
private float _lastTimeHit;

protected override void Update()
{
    base.Update();
    if (Time.time - _lastTimeHit > 0.4)
    {
        ResetMovement();
    }
}

public override void GetHit()
{
    _isHit = true;
    myRB.gravityScale = 0;
    myRB.velocity = new(0,0);
    _lastTimeHit = Time.time;
}


private void ResetMovement()
{
    myRB.gravityScale = 3;
}
*/
public class RangeEnemyController : IAController
{
    [Header("Range Enemy Params")]
    [SerializeField] private float minPlayerDistance;
    private void Start()
    {
        base.Start();
        myAnimator = GetComponent<Animator>();
    }

    public override void EnemyBasicMovement()
    {
        base.EnemyBasicMovement();

        if (!hasDetected) return;
        if (DistanceToPlayer() <= minPlayerDistance) return;

        Vector3 direction = (_player.position - transform.position).normalized;
        myVelocity.x = velocidadMovimiento * direction.x;
        myRB.velocity = myVelocity;
    }
}
