using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    #region Variables
    [SerializeField] GameObject _groundCheck;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private int _maxJumps;

    private Rigidbody2D _rigidbody2D;
    private int _availableJumps;
    #endregion

    #region Unity methods
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        if (_groundCheck == null)
        {
            Debug.LogError("[PlayerController] La referència a groundCheck és null");
        }
    }

    void Start()
    {
        
    }
    

    void Update()
    {
        MovePlayer();
    }
    #endregion

    private void MovePlayer()
    {
        Vector2 myVelocity = _rigidbody2D.velocity;

        myVelocity.x = Input.GetAxis("Horizontal") * _speed;

        myVelocity = this.Jump(myVelocity);

        _rigidbody2D.velocity = myVelocity;

    }

    private Vector2 Jump(Vector2 myVelocity)
    {
        bool isGrounded = Physics2D.OverlapCircle(_groundCheck.transform.position, 0.1f, LayerMask.GetMask("Ground"));

        if (isGrounded && !Input.GetKey(KeyCode.Space))
        {
            _availableJumps = _maxJumps;
        }

        if (Input.GetKeyDown(KeyCode.Space) && _availableJumps > 0) 
        {
            myVelocity.y += _jumpForce;
            _availableJumps -= 1;
        }

        return myVelocity;
    }


}
