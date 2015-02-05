using UnityEngine;
using System.Collections;

public class PlayerFourDirMovement : MonoBehaviour {

	public GameObject toinenPeliObjekti;
    public float movementSpeed = 0.01f;

    bool moving = false;
    Vector3 movingDirection;
    float movementProgress = 0;
    Vector3 startPosition, endPosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//        Debug.Log("moving " + moving + " vertical " + Input.GetAxisRaw("Vertical") + " horizontal " + Input.GetAxisRaw("Horizontal"));
        bool playerGaveMoveOrder = Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0;
        if (!moving && playerGaveMoveOrder)
        {
            startPosition = transform.position;
            moving = true;            
            float yRotation = transform.rotation.eulerAngles.y;

            if (yRotation < 45 || 360 - 45 < yRotation)
                movingDirection = Vector3.forward;
            else if (45 < yRotation && yRotation < 135)
                movingDirection = Vector3.right;
            else if (135 < yRotation && yRotation < 225)
                movingDirection = Vector3.back;
            else if (225 < yRotation && yRotation < 360 - 45)
                movingDirection = Vector3.left;

            if(Input.GetAxisRaw("Horizontal") == 1)
                movingDirection = Quaternion.Euler(new Vector3(0, 90, 0)) * movingDirection;            
            else if(Input.GetAxisRaw("Horizontal") == -1)
                movingDirection = Quaternion.Euler(new Vector3(0, 270, 0)) * movingDirection;
            else if(Input.GetAxisRaw("Vertical") == -1)
                movingDirection = Quaternion.Euler(new Vector3(0, 180, 0)) * movingDirection;
        
            endPosition = startPosition + movingDirection * GridMovement.tileSize;
        }

	}

    void FixedUpdate() {
        if (moving)
        {
            movementProgress += movementSpeed;
            if (movementProgress > 1) {
                movementProgress = 1;
            }
            transform.position = Vector3.Lerp(startPosition, endPosition, movementProgress);
            if (movementProgress == 1) {
                movementProgress = 0;
                moving = false;
            }
        }
    }
}
