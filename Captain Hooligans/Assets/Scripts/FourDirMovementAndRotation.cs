using UnityEngine;
using System.Collections;

public class FourDirMovementAndRotation : FourDirMovement {

    public float rotationSpeed = 10;

    Vector3 newRotation;

    public void TurnLeft(MovementCallback moveComplete) {
        base.moveComplete = moveComplete;
        Turn(-90);
    }

    public void TurnRight(MovementCallback moveComplete) {
        base.moveComplete = moveComplete;
        Turn(90);
    }

    void Turn(float degrees) {
        newRotation = transform.rotation.eulerAngles;
        newRotation.y += degrees;
        iTween.RotateTo(gameObject, iTween.Hash("x", newRotation.x,
                                                 "y", newRotation.y,
                                                 "z", newRotation.z,
                                                 "speed", rotationSpeed,
                                                 "easetype", iTween.EaseType.linear,
                                                 "oncomplete", "NotMovingAnymore"));
    }
}
