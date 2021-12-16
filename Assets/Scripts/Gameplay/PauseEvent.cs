using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class PauseEvent : MonoBehaviour
{
    public InputActionReference invoker;
    public UnityEvent pauseEvent;

    private void Start()
    {
        invoker.action.performed += (e) => { Invoke(); };
    }

    public void Invoke()
    {
        pauseEvent.Invoke();
    }
}
