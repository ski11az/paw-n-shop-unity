using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvent : MonoBehaviour
{
    [SerializeField] private string description;

    [SerializeField] UnityEvent OnPlay;
    public bool IsFinished = false;

    public virtual void PlayEvent()
    {
        Debug.Log("Playing event");

        OnPlay.Invoke();
        IsFinished = true;
    }

}