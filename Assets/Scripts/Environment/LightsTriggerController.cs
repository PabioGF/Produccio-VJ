using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsTriggerController : MonoBehaviour
{
    [SerializeField] private float _lightTransitionDuration;
    [SerializeField] private SpriteRenderer _lightArea;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(OpenLights(_lightArea));
        }
    }
    private IEnumerator OpenLights(SpriteRenderer lightArea)
    {
        float timeElapsed = 0;
        Color color;

        while (timeElapsed < _lightTransitionDuration)
        {
            color = lightArea.color;
            color.a = Mathf.Lerp(1, 0, timeElapsed / _lightTransitionDuration);
            lightArea.color = color;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        color = lightArea.color;
        color.a = 0;
        lightArea.color = color;
    }
}
