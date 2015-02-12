using UnityEngine;
using System.Collections;

[RequireComponent (typeof (FourDirMovement))]
public class PlayerInput : MonoBehaviour {

    private FourDirMovement movement;
	
    void Start() {
        movement = GetComponent<FourDirMovement>(); 
    }

	// Update is called once per frame
	void Update () {
        bool playerGaveMoveOrder = Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0;
        if (playerGaveMoveOrder)
        {
            float yRotation = transform.rotation.eulerAngles.y;
            
            Vector3 movingDirection = GetMovingDirectionFromOrientation (yRotation);
            movement.Move(movingDirection, null);
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
}
