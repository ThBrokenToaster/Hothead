using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class InteractableAbstract : MonoBehaviour {

    abstract public bool CanInteract();
    abstract public void GainFocus();
    abstract public void LoseFocus();
    abstract public void Interact();

}
