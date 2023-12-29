using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopController : MonoBehaviour
{
    private bool _timeStopped;
    public void StopTime(float timeChange, float duration)
    {
        if (_timeStopped) return;

       // Time.timeScale = timeChange;
       // _timeStopped = true;
       // StartCoroutine(RestoreTime(duration));
    }

    IEnumerator RestoreTime(float offset)
    {
        yield return new WaitForSecondsRealtime(offset);
        Time.timeScale = 1.0f;
        _timeStopped = false;
    }

}
