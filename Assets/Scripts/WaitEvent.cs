using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitEvent : GameEvent
{
    [SerializeField] float waitTime = 0;

    public override void PlayEvent()
    {
        StartCoroutine(Co_WaitTime(waitTime));
    }

    private IEnumerator Co_WaitTime(float time)
    {
        yield return new WaitForSeconds(time);
        IsFinished = true;
    }
}
