using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] Transform cameraPosition = null;

    void Update()
    {
        if (!GravityObject.instance.isRolling) {
            transform.position = cameraPosition.position;
        }
    }
}
