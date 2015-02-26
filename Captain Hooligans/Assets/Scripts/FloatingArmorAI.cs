using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FourDirMovementAndRotation))]
public class FloatingArmorAI : MonoBehaviour {

    public float idleBetweenActions;

    bool moving = false;
    float lastMovementEnded = 0;
    FourDirMovementAndRotation movement;

	// Use this for initialization
	void Start () {
        movement = GetComponent<FourDirMovementAndRotation>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!moving && Time.timeSinceLevelLoad - lastMovementEnded > idleBetweenActions)
        {
            RandomizeNextAction();
        }
	}

    void RandomizeNextAction()
    {
        float random = Random.Range(0f, 1f);
//        if (random < 0.2f)
//            movement.TurnLeft(MoveEnded);
//        else if (random < 0.4f)
//            movement.TurnRight(MoveEnded);
//        else
            movement.Move(FourDirMovement.FourDirections.Forward, MoveEnded);
        moving = true;
    }

    void MoveEnded() {
        lastMovementEnded = Time.timeSinceLevelLoad;
        moving = false;
    }
}
