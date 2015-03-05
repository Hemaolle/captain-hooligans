using UnityEngine;
using System.Collections;

public class CharacterHealth : MonoBehaviour {

    public float maxHP = 10;
    protected float currentHP;

    public Alignment alignment = Alignment.Enemy;
    
    public enum Alignment
    {
        Friendly, Enemy
    };

	// Use this for initialization
	void Start () {
        currentHP = maxHP;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void TakeDamage(int damage){

        currentHP = currentHP - damage;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public virtual void Die(){
        Destroy(gameObject);
    }
}
