using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject target;

    void Update()
    {
        Vector3 newPosition = target.transform.position;
        newPosition.z = transform.position.z;

        transform.position = newPosition;
    }
}
