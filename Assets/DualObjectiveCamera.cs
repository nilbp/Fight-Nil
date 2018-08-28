using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualObjectiveCamera : MonoBehaviour {

    public Transform leftTarget;
    public Transform rightTarget;

    float distanceBetweenTargets;
    float centerPosition;

    float maxCameraSize = 1.54f;

    public Camera camera;

    void Update ()
    {
        distanceBetweenTargets = Mathf.Abs(leftTarget.position.x - rightTarget.position.x) * 2;
        centerPosition = (leftTarget.position.x + rightTarget.position.x) / 2;

        transform.position = new Vector3(centerPosition, transform.position.y, transform.position.z);

        camera.orthographicSize = maxCameraSize;
	}
}
