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
            
            FourDirMovement.FourDirections movingDirection = GetMovingDirection();			

            movement.Move(movingDirection, null);
        }
	}    

	/// <summary>
	/// If the palayer moves to some other direction than forward, rotate the movingDirection accordinlgy
	/// </summary>
	/// <returns>The rotated moving direction.</returns>
	/// <param name="movingDirection">The original moving direction.</param>
	FourDirMovement.FourDirections GetMovingDirection ()
	{
		FourDirMovement.FourDirections newMovingDirection = FourDirMovement.FourDirections.Forward;
		if (Input.GetAxisRaw ("Horizontal") == 1)
			newMovingDirection = FourDirMovement.FourDirections.Right;
		else
			if (Input.GetAxisRaw ("Horizontal") == -1)
				newMovingDirection = FourDirMovement.FourDirections.Left;
		else
			if (Input.GetAxisRaw ("Vertical") == -1)
				newMovingDirection = FourDirMovement.FourDirections.Back;		
		return newMovingDirection;
	}

}
