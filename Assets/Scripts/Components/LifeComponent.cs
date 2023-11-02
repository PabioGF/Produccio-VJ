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

    /*public Image lifeBar;
    public Color FullLifeColor = Color.green;
    public Color DeadColor = Color.red;*/
    #endregion

    #region Unity methods
    void Start()
    {
        CurrentLife = MaxLife;

        //UpdateLifeBar();

    }
    #endregion
    public void ApplyDamage(float amount)
    {
        CurrentLife -= amount;

       // UpdateLifeBar();

        if (CurrentLife <= 0)
        {
            isDead = true;

            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    /*private void UpdateLifeBar()
    {
        if (lifeBar != null)
        {
            lifeBar.fillAmount = CurrentLife / MaxLife;

            lifeBar.color = Color.Lerp(DeadColor, FullLifeColor, CurrentLife / MaxLife);
        }
    }*/
}
