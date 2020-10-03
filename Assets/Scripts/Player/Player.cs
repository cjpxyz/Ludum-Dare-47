﻿using System;
using UnityEngine;
using UnityEditor;

public struct PlayerInput
{
    public Vector3 MousePosition;
    //Buttons pressed
}

public class Player : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CharacterController characterController;

    private PlayerInput input = new PlayerInput();

#if UNITY_EDITOR
    [SerializeField] private bool drawDebugInformation;
    Vector3 debugMouseWorldPosition;
#endif

    public void Update()
    {
        if(DebugView.Instance) { }
        PollInput();
        SetPlayerRotation();
    }

    private void PollInput()
    {
        input.MousePosition = Input.mousePosition;
    }

    private void SetPlayerRotation()
    {
        //First we calculate what the current mouses world position would be
        Vector3 mouseWorldPosition = CalculateMouseWorldPosition(mainCamera.nearClipPlane);
#if UNITY_EDITOR
        debugMouseWorldPosition = mouseWorldPosition;
#endif

    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if(drawDebugInformation)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(debugMouseWorldPosition, 1f);
        }
    }
#endif
    private Vector3 CalculateMouseWorldPosition(float z)
    {
        Ray ray = mainCamera.ScreenPointToRay(input.MousePosition);
        Plane xy = new Plane(Vector3.up, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }
}