using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossStart : MonoBehaviour
{
    [SerializeField] private GameObject _door;
    [SerializeField] private GameObject _boss;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _door.SetActive(true);
        _boss.GetComponent<Animator>().enabled = true;
    }
}
