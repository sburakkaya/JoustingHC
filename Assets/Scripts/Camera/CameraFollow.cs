using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform heroTransform;
    private Vector3 newPosition;
    private Vector3 offset;
    [SerializeField] private float lerpValue;
    private float _startY;
    
    void Start()
    {
        SetOffsetValue();
        _startY = heroTransform.position.y;
    }
    
    void FixedUpdate()
    {
        SetCameraSmoothFollow();
    }
    
    private void SetOffsetValue()
    {
        offset = transform.position - heroTransform.position;

    }

    private void SetCameraSmoothFollow()
    {
        newPosition = Vector3.Lerp(transform.position, new Vector3(heroTransform.position.x, _startY, heroTransform.position.z) + offset, lerpValue * Time.deltaTime);
        transform.position = newPosition;
    }

}