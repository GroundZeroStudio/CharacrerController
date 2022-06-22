/****************************************************
    文件：OnGroundSensor.cs
    作者：Olivia
    日期：2022/1/28 0:21:2
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour
{
    public CapsuleCollider mCollider;

    private float mRadius;
    public Vector3 Point1;
    public Vector3 Point2;

    public float Offset = 0.1f;
    private void Awake()
    {
        mRadius = mCollider.radius - 0.05f;
    }

    private void Update()
    {
        Point1 = transform.position + transform.up * (mRadius - Offset);
        Point2 = transform.position + transform.up * ((mCollider.height - 1) - mRadius);

        Collider[] cols = Physics.OverlapCapsule(Point1, Point2, mRadius, LayerMask.GetMask("Ground"));
        if (cols.Length != 0)
        {
            SendMessageUpwards("OnGround");
        }
        else
        {
            SendMessageUpwards("NotOnGround");
        }
    }
}
