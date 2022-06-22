/****************************************************
    文件：PlayerInput.cs
    作者：Olivia
    日期：2022/1/22 21:8:42
    功能：Nothing
*****************************************************/

using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("Key Settings")]
    public string KeyUp = "w";
    public string KeyDown = "s";
    public string KeyLeft = "a";
    public string KeyRight = "d";
    public string KeyA;
    public string KeyB;
    public string KeyC;
    public string KeyD;

    public string KeyJRight;
    public string KeyJLeft;
    public string KeyJup;
    public string KeyJDown;

    [Header("Output Signals")]
    public float Dup = 0;
    public float Dright = 0;
    //控制动画参数
    public float Dmag;
    public Vector3 DVect;
    public bool Run;
    public bool Jump;
    private bool mLastJump;
    public float Jup;
    public float Jright;
    
    [Header("Others")]
    public bool InputEnabled;
    
    private float mTargetDup;
    private float mTargetDright;
    private float mCurVelocityUp = 0;
    private float mCurVelocityRight = 0;
    
    [SerializeField]
    [Range(0, 1)]
    private float mSmoothTime = 0.1f;

    void Update()         
    {
        Jup = (Input.GetKey(KeyJup) ? 1.0f : 0) - (Input.GetKey(KeyJDown) ? 1.0f : 0);
        Jright = (Input.GetKey(KeyJRight) ? 1.0f : 0) - (Input.GetKey(KeyJLeft) ? 1.0f : 0);

        mTargetDup = (Input.GetKey(KeyUp) ? 1.0f : 0) - (Input.GetKey(KeyDown) ? 1.0f : 0);
        mTargetDright = (Input.GetKey(KeyRight) ? 1.0f : 0) - (Input.GetKey(KeyLeft) ? 1.0f : 0);
        if (!InputEnabled)
        {
            mTargetDup = 0;
            mTargetDright = 0;
        }
        
        if (Mathf.Abs(mTargetDup) - Mathf.Abs(Dup) > 0.001f || Mathf.Abs(Dup) - Mathf.Abs(mTargetDup) > 0.001f)
        {
            Dup = Mathf.SmoothDamp(Dup, mTargetDup, ref mCurVelocityUp, 0.1f);
        }
        if (Mathf.Abs(mTargetDright) - Mathf.Abs(Dright) > 0.001f || Mathf.Abs(Dright) - Mathf.Abs(mTargetDright) > 0.001f)
        {
            Dright = Mathf.SmoothDamp(Dright, mTargetDright, ref mCurVelocityRight, 0.1f);
        }
        Vector2 driection = SuqateToCricle(new Vector2(Dright, Dup));
        float tempup = driection.y;
        float tempright = driection.x;
        Dmag = Mathf.Sqrt(tempup * tempup + tempright * tempright);
        DVect = tempright * transform.right + tempup * transform.forward;

        Run = Input.GetKey(KeyA);

        bool bNewJump = Input.GetKey(KeyB);
        if (bNewJump != mLastJump && bNewJump)
        {
            Jump = true;
        }
        else
        {
            Jump = false;
        }
        mLastJump = bNewJump;
    }

    public Vector2 SuqateToCricle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
        return output;
    }
}
