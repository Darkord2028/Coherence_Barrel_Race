using UnityEngine;

public class Interactable : MonoBehaviour
{
    public InteractableTypes interactableType;
}
public enum InteractableTypes
{
    None,
    Nitro,
    Mud,
    Wall,
    Ink,
    Checkpoint
}
