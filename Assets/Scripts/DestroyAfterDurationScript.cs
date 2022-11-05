using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDurationScript : MonoBehaviour
{
    [SerializeField] float duration = 0;
    void Start()
    {
        Destroy(gameObject, duration);    
    }
}
