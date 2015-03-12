using UnityEngine;
using System.Collections;

public class SwordDeleter : MonoBehaviour {
    public float deleteIn=2f;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, deleteIn);
	}
    /*
    public void OnCollisionEnter(){
        Destroy(gameObject);
    }
*/
}
