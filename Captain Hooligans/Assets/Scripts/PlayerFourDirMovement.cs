﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Moves a gameobject depending on it's facing direction.
/// </summary>
public class PlayerFourDirMovement : MonoBehaviour {

    public float movementSpeed = 0.01f;

	bool moving = false;
    Vector3 movingDirection;
    Vector3 startPosition, endPosition;

	// Update is called once per frame
	void Update () {
        bool playerGaveMoveOrder = Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0;
        if (!moving && playerGaveMoveOrder)
        {
            startPosition = transform.position;
            float yRotation = transform.rotation.eulerAngles.y;

			movingDirection = GetMovingDirectionFromOrientation (yRotation);
			movingDirection = RotateMovingDirection (movingDirection); 

			// Check for obstacles
			int obstacles = LayerMask.GetMask(new string[]{"Obstacle"});
			if (Physics.Raycast(startPosition, movingDirection, GridMovement.tileSize, obstacles))
			{
				// If there's an obstacle in front of the player bump forward and then back a little bit.
				endPosition = startPosition + movingDirection * GridMovement.tileSize * 0.2f;
				BlockedMoveForward ();				
				SoundEffectManager.playSoundEffectOnce ("WalkAgainstObstacle");
			}
			else {
				// Otherwis just move to the next tile.
				endPosition = startPosition + movingDirection * GridMovement.tileSize;
				MoveToNextTile();
			}
			moving = true;
        }

	}

	/// <summary>
	/// Player's movement direction is determined by his facing direction: he moves to the direction that is 
	/// closest to his facing direction.
	/// </summary>
	/// <returns>The moving direction.</returns>
	/// <param name="yRotation">Y rotation.</param>
	Vector3 GetMovingDirectionFromOrientation (float yRotation)
	{
		Vector3 newMovingDirection = Vector3.zero;
		if (yRotation < 45 || 360 - 45 < yRotation)
			newMovingDirection = Vector3.forward;
		else
			if (45 < yRotation && yRotation < 135)
				newMovingDirection = Vector3.right;
			else
				if (135 < yRotation && yRotation < 225)
					newMovingDirection = Vector3.back;
				else
					if (225 < yRotation && yRotation < 360 - 45)
						newMovingDirection = Vector3.left;
		return newMovingDirection;
	}

	/// <summary>
	/// If the palayer moves to some other direction than forward, rotate the movingDirection accordinlgy
	/// </summary>
	/// <returns>The rotated moving direction.</returns>
	/// <param name="movingDirection">The original moving direction.</param>
	Vector3 RotateMovingDirection (Vector3 movingDirection)
	{
		Vector3 newMovingDirection = Vector3.zero;
		if (Input.GetAxisRaw ("Horizontal") == 1)
			newMovingDirection = Quaternion.Euler (new Vector3 (0, 90, 0)) * movingDirection;
		else
			if (Input.GetAxisRaw ("Horizontal") == -1)
				newMovingDirection = Quaternion.Euler (new Vector3 (0, 270, 0)) * movingDirection;
			else
				if (Input.GetAxisRaw ("Vertical") == -1)
					newMovingDirection = Quaternion.Euler (new Vector3 (0, 180, 0)) * movingDirection;
				else
					return movingDirection;
		return newMovingDirection;
	}

	/// <summary>
	/// Not moving anymore, so it's possible to move again.
	/// </summary>
	void NotMovingAnymore() {
		moving = false;
	}

	/// <summary>
	/// Moves the player to the next tile.
	/// </summary>
	void MoveToNextTile() {
		iTween.MoveTo(gameObject, iTween.Hash("x", endPosition.x,
		                                      "y", endPosition.y,
		                                      "z", endPosition.z,
		                                      "speed", movementSpeed,
		                                      "easetype", iTween.EaseType.linear,
		                                      "oncomplete", "NotMovingAnymore"));
	}

	/// <summary>
	/// The player will bump forward, then backward.
	/// </summary>
	void BlockedMoveForward ()
	{
		iTween.MoveTo (gameObject, iTween.Hash ("x", endPosition.x,
		                                        "y", endPosition.y,
		                                        "z", endPosition.z, 
		                                        "speed", movementSpeed / 2,
		                                        "easetype", iTween.EaseType.linear,
		                                        "oncomplete", "BlockedMoveBack"));
	}

	/// <summary>
	/// Move backwards after moving forwards when being blocked.
	/// </summary>
	void BlockedMoveBack() {
		iTween.MoveTo(gameObject, iTween.Hash("x", startPosition.x,
		                                      "y", startPosition.y,
		                                      "z", startPosition.z,
		                                      "speed", movementSpeed/2,
		                                      "easetype", iTween.EaseType.linear,
		                                      "oncomplete", "NotMovingAnymore"));
	}
}
