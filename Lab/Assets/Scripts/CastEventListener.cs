using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CastEventListener : MonoBehaviour
{
    public CastEvent Event;
    public UnityEvent<KeyCode> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }
    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }
    public void OnEventRaised(KeyCode K)
    {
        Response.Invoke(K);
    }
}
