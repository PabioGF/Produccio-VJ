using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : InteractableObject
{
    #region Global Variables
    [SerializeField] private GameObject _linkedTurret;

    private bool _hasInteracted;
    #endregion

    protected override void Interact()
    {
       _linkedTurret.SetActive(false);
    }
}
