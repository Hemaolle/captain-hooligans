using UnityEngine;
using System.Collections;

/// <summary>
/// Moves a gameobject depending on it's facing direction.
/// </summary>
public class FourDirMovement : MonoBehaviour {

    public float movementSpeed = 0.01f;
    public delegate void MovementCallback();

	bool moving = false;
    Vector3 movingDirection;
    Vector3 startPosition, endPosition;
    protected MovementCallback moveComplete;

    public void Move(Vector3 movingDirection, MovementCallback moveComplete) {
        if (!moving)
        {
            this.moveComplete = moveComplete;
            startPosition = transform.position;            
            
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
	/// Not moving anymore, so it's possible to move again.
	/// </summary>
	protected void NotMovingAnymore() {
		moving = false;
        if (moveComplete != null)
            moveComplete();
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
