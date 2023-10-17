using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    #region Variables
    [SerializeField] private GameObject _attackArea;
    [SerializeField] private float _attackDuration;
    [SerializeField] private float _attackCd;

    private Animator _animator;
    private bool _isAttacking;
    private float _attackDurationTimer;
    private float _attackCdTimer = 1;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

        _attackArea.SetActive(false);
        if (_attackArea == null)
        {
            Debug.LogError("[PlayerAttack] La referència a attackArea és null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        CheckBools();
        
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && _attackCdTimer > _attackCd) {
            _isAttacking = true;
            _attackArea.SetActive(_isAttacking);
            _animator.SetTrigger("Attack");
        }
    }

    private void CheckBools()
    {
        if (_isAttacking)
        {
            _attackDurationTimer += Time.deltaTime;
            if (_attackDurationTimer >= _attackDuration)
            {
                _isAttacking = false;
                _attackArea.SetActive(_isAttacking);
                _attackDurationTimer = 0;
                _attackCdTimer = 0;

            }
        } else
        {
            _attackCdTimer += Time.deltaTime;
        }
    }
}
