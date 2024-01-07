using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : InteractableObject
{
    #region Global Variables
    [SerializeField] private GameObject _linkedObject;
    [SerializeField] private LinkedObjectType _objectType;
    private enum LinkedObjectType
    {
        turret = 0,
        ground = 1
    } 

    #endregion

    protected override void Interact()
    {
        switch (_objectType)
        { 
            case LinkedObjectType.turret:
                _linkedObject.GetComponent<TurretScript>().DisarmTurret();
                break;
            case LinkedObjectType.ground:
                _linkedObject.GetComponent<Animator>().SetTrigger("Move");
                break;
        }

    }
}
