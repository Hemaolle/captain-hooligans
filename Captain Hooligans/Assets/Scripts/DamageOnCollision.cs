using UnityEngine;
using System.Collections;

public class DamageOnCollision : MonoBehaviour {
    public CharacterHealth.Alignment alignment = CharacterHealth.Alignment.Enemy;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnCollisionEnter(Collision other){
        GameObject collider = other.gameObject;
        CharacterHealth targetHealth = collider.GetComponent<CharacterHealth>();

        if (targetHealth != null && targetHealth.alignment != alignment)
        {
            targetHealth.TakeDamage(1);

        }

    }
}
