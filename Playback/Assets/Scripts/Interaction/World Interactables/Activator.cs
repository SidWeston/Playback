using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    [SerializeField] protected List<ActivatableObject> activatableObjects;
    public bool powered = false;
}