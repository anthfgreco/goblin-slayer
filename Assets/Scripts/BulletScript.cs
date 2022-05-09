using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public AudioClip clip1;
    public AudioClip clip2;

    // Start is called before the first frame update
    void Start() {
        // Destroy bullet 10 seconds after it's made
        Destroy(gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     private void OnCollisionEnter(Collision collision) {
        GameObject other = collision.gameObject;
    
        // If bullet hits goblin, decrease health of goblin, and destroy bullet
        if (other.gameObject.CompareTag("Goblin")) {
            other.GetComponent<EnemyScript>().TakeDamage(27);

            if (other.GetComponent<EnemyScript>().currentHealth > 0) {
                AudioSource.PlayClipAtPoint(clip1, this.transform.position, 1.0f);
            }
            else {
                AudioSource.PlayClipAtPoint(clip2, this.transform.position, 1.0f);
            }
            Debug.Log("Hit Goblin!");
            // destroy bullet
            Destroy(gameObject);
        }
        // If bullet hits bullseye and the player has killed 10 goblins, enable the stealth bomber to attack
        else if (other.gameObject.CompareTag("Bullseye") && 
            GameObject.Find("Controller (right)").GetComponent<RightController>().goblinsKilled >= 10) {
            Debug.Log("Bullseye hit!");
            GameObject.Find("Stealth_Bomber").GetComponent<PlaneScript>().attackState = true;
            this.GetComponent<Rigidbody>().useGravity = true;
        }
        // If bullet does not hit goblin, apply gravity to bullet
        else {
            // Turn on gravity when object is hit
            this.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
