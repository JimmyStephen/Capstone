using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Damage : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float HealthDamage;
    [SerializeField] float EnergyDamage;
    [SerializeField] bool destroyAfterDuration = false;
    [SerializeField] float destroyAfter;
    [SerializeField] bool destroyOnCollision;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Vector3 forceSpeed = Vector3.right * speed;
        if (transform.rotation.z == 1)
        {
            forceSpeed *= -1;
        }
        rb.AddForce(forceSpeed, ForceMode.Force);

        if (destroyAfterDuration) Destroy(this.gameObject, destroyAfter);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CharacterTemplate>(out CharacterTemplate player))
        {
            float damageDealt = HealthDamage -= player.resistanceFlat;
            damageDealt -= damageDealt * player.resistancePercent;
            if (damageDealt < 0) damageDealt = 0;
            player.health.Damage(damageDealt);
            player.energy.Damage(EnergyDamage);
        }

        if (destroyOnCollision) Destroy(this.gameObject, destroyAfter);
    }
}
