using System;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;



public class Fireball : MonoBehaviour
{
    private void DestroyParent()
    {
        Destroy(transform.parent.gameObject);
    }
}
