using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] private GameObject warningLightPrefab;
    private GameObject currentWarningLight;
    public AudioClip clip;
    public float missileDamage;

    [Header("Warning Light")]
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float explosionRadius;

    // Start is called before the first frame update
    void Start()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit; 
        // If ray hits the ground
        if (Physics.Raycast(ray, out hit, 10000)){
            currentWarningLight = Instantiate(warningLightPrefab, hit.point + new Vector3(0.0f, 10.0f, 0.0f), Quaternion.Euler(90, 0, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Explode missile if it goes below the map
        if (this.transform.position.y < -100.0f) {
            MissileExplosion();
        }

    }
    void OnCollisionEnter(Collision collision) {
        // Explode missile when they come in contact with something
        MissileExplosion();
    }

    void MissileExplosion() {
        //audioData.Play(0);
        AudioSource.PlayClipAtPoint(clip, this.transform.position);

        // Create explosion effect
        GameObject currentExplosion = Instantiate(explosionEffect, this.transform.position, Quaternion.identity);

        MissileDoDamage(this.transform.position, explosionRadius, missileDamage);

        // Remove missile and warning light
        Destroy(currentWarningLight);
        Destroy(this.gameObject);
    }

    void MissileDoDamage(Vector3 location, float radius, float damage) {
        Collider[] objectsInRange = Physics.OverlapSphere(location, radius);
        foreach (Collider col in objectsInRange) {
            if (col.tag == "Goblin") {
                GameObject enemy = col.gameObject;
                float proximity = (location - enemy.transform.position).magnitude;
                float effect = 1 - (proximity / radius);
                int finalDamage = Math.Abs((int)(damage * effect));
                enemy.GetComponent<EnemyScript>().TakeDamage(finalDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

}
