using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarComponent : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    public void UpdateHealthBar(float current, float max)
    {
        slider.value = current / max;

    }
    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
    }
}
