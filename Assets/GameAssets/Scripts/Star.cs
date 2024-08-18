using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour, IInteractable
{
    private readonly EventBrokerComponent eventBroker = new EventBrokerComponent();

    public bool Interact(GameObject source)
    {
        eventBroker.Publish(this, new StarEvents.AddStar());
        Destroy(gameObject, .1f);
        return false;
    }

    public bool StopInteract()
    {
        return true;
    }
}
