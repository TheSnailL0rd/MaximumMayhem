using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    private Vector2 movement;

    public bool accelerating;
    public bool braking;

    [SerializeField] private float motorForce;
    [SerializeField] private float brakeForce;
    private float currentBrakeForce;
    [SerializeField] private float maxSteeringAngle;
    private float currentSteeringAngle;

    [SerializeField] private WheelCollider FLWheelCol;
    [SerializeField] private WheelCollider FRWheelCol;
    [SerializeField] private WheelCollider BLWheelCol;
    [SerializeField] private WheelCollider BRWheelCol;

    [SerializeField] private Transform FLWheelTra;
    [SerializeField] private Transform FRWheelTra;
    [SerializeField] private Transform BLWheelTra;
    [SerializeField] private Transform BRWheelTra;

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void HandleMotor()
    {
        if (accelerating)
        {
            FLWheelCol.motorTorque = motorForce;
            FRWheelCol.motorTorque = motorForce;
            FLWheelCol.brakeTorque = 0f;
            FRWheelCol.brakeTorque = 0f;
            BLWheelCol.brakeTorque = 0f;
            BRWheelCol.brakeTorque = 0f;
        }
        else
        {
            FLWheelCol.motorTorque = 0f;
            FRWheelCol.motorTorque = 0f;
            FLWheelCol.brakeTorque = brakeForce/3f;
            FRWheelCol.brakeTorque = brakeForce/3f;
        }
        currentBrakeForce = braking ? brakeForce : 0f;
        if (braking)
            ApplyBraking();
    }

    private void HandleSteering()
    {
        currentSteeringAngle = maxSteeringAngle * movement.x;
        FLWheelCol.steerAngle = currentSteeringAngle;
        FRWheelCol.steerAngle = currentSteeringAngle;
    }

    private void ApplyBraking()
    {
        FLWheelCol.motorTorque = -motorForce;
        FRWheelCol.motorTorque = -motorForce;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(FLWheelCol, FLWheelTra);
        UpdateSingleWheel(FRWheelCol, FRWheelTra);
        UpdateSingleWheel(BLWheelCol, BLWheelTra);
        UpdateSingleWheel(BRWheelCol, BRWheelTra);
    }

    public void Accel(InputAction.CallbackContext context)
    {
        if (context.performed)
            accelerating = true;
        if (context.canceled)
            accelerating = false;
    }

    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    public void Brake(InputAction.CallbackContext context)
    {
        if (context.performed)
            braking = true;
        if (context.canceled)
            braking = false;
    }

    private void UpdateSingleWheel(WheelCollider wc, Transform wt)
    {
        Vector3 pos;
        Quaternion rot;
        wc.GetWorldPose(out pos, out rot);
        wt.rotation = Quaternion.Slerp(wt.rotation, rot, 0.3f);
        wt.position = pos;
    }
}
