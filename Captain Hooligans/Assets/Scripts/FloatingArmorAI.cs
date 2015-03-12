using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FourDirMovementAndRotation))]
public class FloatingArmorAI : MonoBehaviour {

    public float idleBetweenActions;

    bool moving = false;
    float lastMovementEnded = 0;
    FourDirMovementAndRotation movement;
    Vector3 newMovingDirection = Vector3.zero;

	// Use this for initialization
	void Start () {
        movement = GetComponent<FourDirMovementAndRotation>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!moving && Time.timeSinceLevelLoad - lastMovementEnded > idleBetweenActions)
        {
            RandomizeNextAction();
            //transform.TransformDirection(ToPlayerTile());
//            movement.Move(ToPlayerTile(), MoveEnded);
//            moving = true;

        }
	}

    void RandomizeNextAction()
    {
        float random = Random.Range(0f, 1f);
        if (random < 0.2f)
            movement.TurnLeft(MoveEnded);
        else if (random < 0.4f)
            movement.TurnRight(MoveEnded);
        else
            movement.Move(FourDirMovement.FourDirections.Forward, MoveEnded);
        moving = true;
    }

   
    /// masan koodi


  
    FourDirMovement.FourDirections ToPlayerTile(){
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        newMovingDirection = player.transform.position - transform.position;
        Debug.Log(player.transform.position- transform.position);
        newMovingDirection.Normalize();
        newMovingDirection = transform.InverseTransformDirection(newMovingDirection);

        float forA, backA, leftA, rightA;

        forA = Mathf.Abs(Vector3.Angle(newMovingDirection, Vector3.forward));
        backA = Mathf.Abs(Vector3.Angle(newMovingDirection, Vector3.back));
        leftA = Mathf.Abs(Vector3.Angle(newMovingDirection, Vector3.left));
        rightA = Mathf.Abs(Vector3.Angle(newMovingDirection, Vector3.right));

        if (forA <= 45.1)
        {
            return FourDirMovement.FourDirections.Forward;
        } else if (backA <= 45.1)
        {
            return FourDirMovement.FourDirections.Back;
        } else if (leftA <= 45.1)
        {
            return FourDirMovement.FourDirections.Left;
        }
        return FourDirMovement.FourDirections.Right;

    }

    void MoveEnded() {
        lastMovementEnded = Time.timeSinceLevelLoad;
        moving = false;
    }
}
