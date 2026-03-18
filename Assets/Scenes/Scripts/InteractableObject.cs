using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    void Awake() 
    {
        
    }
    public virtual void TryInteract()
    {
        Interact();
    }
    protected abstract void Interact();
}
