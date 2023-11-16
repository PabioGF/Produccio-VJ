using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeComponent : MonoBehaviour
{
    #region Variables
    public float MaxLife = 1f;
    public float CurrentLife;
    public bool isDead = false;
    #endregion

    #region Unity methods
    void Start()
    {
        CurrentLife = MaxLife;

    }
    #endregion
    public void ApplyDamage(float amount)
    {
        CurrentLife -= amount;

        if (CurrentLife <= 0)
        {
            isDead = true;

            gameObject.SetActive(false);
        }
    }
}
