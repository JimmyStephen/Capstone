using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] Teleporter target;
    public GameObject LandingLocation;
    [SerializeField] float cooldown = 3;
    private float currentCD;

    public void OnTeleport()
    {
        currentCD = cooldown;
    }

    void Update()
    {
        currentCD -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (currentCD > 0) return;
        if (!other.TryGetComponent<CharacterTemplate>(out _)) return;
        target.OnTeleport();
        other.gameObject.transform.position = target.LandingLocation.transform.position;
    }
}
