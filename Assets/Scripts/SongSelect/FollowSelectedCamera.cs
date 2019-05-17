using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FollowSelectedCamera : MonoBehaviour {

    public float maxSpeed = 10f;

    //private bool lastWasMoving;
    //private float speedMultiplier;
    private Vector2 positionToBeAt;

    // Update is called once per frame
    void Update () {
        //get
        positionToBeAt = new Vector2(EventSystem.current.currentSelectedGameObject.transform.position.x, 
            EventSystem.current.currentSelectedGameObject.transform.position.y);
        Vector2 thisPositionXY = new Vector2(transform.position.x, transform.position.y);
        //do
        Vector2 newPosition = Vector2.MoveTowards(thisPositionXY, positionToBeAt, maxSpeed);
        //set
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);

    }
}
