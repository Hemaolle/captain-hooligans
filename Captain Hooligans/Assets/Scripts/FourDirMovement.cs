using UnityEngine;
using System.Collections;

/// <summary>
/// Moves a gameobject depending on it's facing direction.
/// </summary>
public class FourDirMovement : MonoBehaviour {

    public float movementSpeed = 0.01f;
    public delegate void MovementCallback();
	public enum FourDirections { Forward, Right, Back, Left };	// The possible movement directions

	bool moving = false;
    Vector3 movingDirection;
    Vector3 startPosition, endPosition;
    protected MovementCallback moveComplete;

    /// <summary>
    /// Moves to the specified movingDirection and calls moveComplete method when the gameObject arrives
    /// at the destination.
    /// </summary>
    /// <param name="movingDirection">Moving direction (relative to the gameobject's rotation).</param>
    /// <param name="moveComplete">Method to be called when moving is finished.</param>
    public void Move(FourDirections movingDirection, MovementCallback moveComplete) {
        if (!moving)
        {
            this.moveComplete = moveComplete;
            startPosition = transform.position;            

            Vector3 globalMovingDirection = GetGlobalMovingDirection(movingDirection);

            // Check for obstacles
            int obstacles = LayerMask.GetMask(new string[]{"Obstacle"});
            if (Physics.Raycast(startPosition, globalMovingDirection, GridMovement.tileSize, obstacles))
            {
                // If there's an obstacle in front of the player bump forward and then back a little bit.
                endPosition = startPosition + globalMovingDirection * GridMovement.tileSize * 0.2f;
                BlockedMoveForward ();              
                SoundEffectManager.playSoundEffectOnce ("WalkAgainstObstacle");
            }
            else {
                // Otherwis just move to the next tile.
                endPosition = startPosition + globalMovingDirection * GridMovement.tileSize;
                MoveToNextTile();
            }
            moving = true;
        }
    }

	/// <summary>
	/// The character's movement direction is calculated based on localDirection and
	/// it's rotation: for example if the character is facing right (or mostly right
    /// from the 4 main directions), and it's localDirection is right, it's global
    /// movement direction should be backward.
	/// </summary>
	/// <returns>The global moving direction.</returns>
	/// <param name="localDirection">The local moving direction enum.</param>
	Vector3 GetGlobalMovingDirection (FourDirections localDirection)
	{
		Vector3 localMovementDirectionVector = Vector3.zero;
		switch (localDirection) {
			case FourDirections.Back:
				localMovementDirectionVector = Vector3.back;
				break;
            
            case FourDirections.Forward:
                localMovementDirectionVector = Vector3.forward;
                break;
            
            case FourDirections.Left:
                localMovementDirectionVector = Vector3.left;
                break;
            
            case FourDirections.Right:
                localMovementDirectionVector = Vector3.right;
                break;
		}
		float yRotation = transform.rotation.eulerAngles.y;
		Vector3 facingDirection = Vector3.zero;
		if (yRotation < 45 || 360 - 45 < yRotation)
			facingDirection = Vector3.forward;
		else
			if (45 < yRotation && yRotation < 135)
				facingDirection = Vector3.right;
		else
			if (135 < yRotation && yRotation < 225)
				facingDirection = Vector3.back;
		else
			if (225 < yRotation && yRotation < 360 - 45)
				facingDirection = Vector3.left;
		Vector3 globalDirection = Quaternion.FromToRotation(Vector3.forward, localMovementDirectionVector) * facingDirection;

		return globalDirection;
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
