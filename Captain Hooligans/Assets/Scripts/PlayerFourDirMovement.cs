using UnityEngine;
using System.Collections;

/// <summary>
/// Moves a gameobject depending on it's facing direction.
/// </summary>
public class PlayerFourDirMovement : MonoBehaviour {

	public GameObject toinenPeliObjekti;
    public float movementSpeed = 0.01f;

	MovementStatus movementStatus = MovementStatus.Still;
    Vector3 movingDirection;
    float movementProgress = 0;
    Vector3 startPosition, endPosition;

	enum MovementStatus { Still, Moving, BlockedGoingForward, BlockedGoingBackward };
	
	// Update is called once per frame
	void Update () {
        bool playerGaveMoveOrder = Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0;
        if (movementStatus == MovementStatus.Still && playerGaveMoveOrder)
        {
            startPosition = transform.position;
            float yRotation = transform.rotation.eulerAngles.y;

			// Player's movement direction is determined by his facing direction: he moves to the direction that is 
			// closest to his facing direction
            if (yRotation < 45 || 360 - 45 < yRotation)
                movingDirection = Vector3.forward;
            else if (45 < yRotation && yRotation < 135)
                movingDirection = Vector3.right;
            else if (135 < yRotation && yRotation < 225)
                movingDirection = Vector3.back;
            else if (225 < yRotation && yRotation < 360 - 45)
                movingDirection = Vector3.left;

			// If the palayer moves to some other direction than forward, rotate the movingDirection accordinlgy
            if(Input.GetAxisRaw("Horizontal") == 1)
                movingDirection = Quaternion.Euler(new Vector3(0, 90, 0)) * movingDirection;            
            else if(Input.GetAxisRaw("Horizontal") == -1)
                movingDirection = Quaternion.Euler(new Vector3(0, 270, 0)) * movingDirection;
            else if(Input.GetAxisRaw("Vertical") == -1)
                movingDirection = Quaternion.Euler(new Vector3(0, 180, 0)) * movingDirection;
        
            endPosition = startPosition + movingDirection * GridMovement.tileSize;

			// Check for obstacles
			int obstacles = LayerMask.GetMask(new string[]{"Obstacle"});
			if (Physics.Raycast(startPosition, movingDirection, GridMovement.tileSize, obstacles))
			{
				movementStatus = MovementStatus.BlockedGoingForward;
				SoundEffectManager.playSoundEffectOnce("WalkAgainstObstacle");
			}
			else {
				movementStatus = MovementStatus.Moving;
			}
        }

	}

    void FixedUpdate() {
		bool stop = false;

		switch (movementStatus) {
		
			case MovementStatus.Moving:
				movementProgress += movementSpeed;
				if (movementProgress > 1) {
					movementProgress = 1;
				}
				
				if (movementProgress == 1) {
					movementProgress = 1;
					stop = true;
					//movementStatus = MovementStatus.Still;
	            }
				break;

			case MovementStatus.BlockedGoingForward: 
				if(movementProgress < 0.2) {
					movementProgress += movementSpeed;
                }
				else {
					movementStatus = MovementStatus.BlockedGoingBackward;
				}
				break;

			case MovementStatus.BlockedGoingBackward:
				movementProgress -= movementSpeed;
				if(movementProgress < 0) {
					movementProgress = 0;
					//movementStatus = MovementStatus.Still;
					stop = true;
                }
				break;

            default:
                break;
        }

		if (movementStatus != MovementStatus.Still) {
			transform.position = Vector3.Lerp(startPosition, endPosition, movementProgress);
			if(stop) {
				movementStatus = MovementStatus.Still;
				movementProgress = 0;
			}
		}        
    }
}
