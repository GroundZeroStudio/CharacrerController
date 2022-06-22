/****************************************************
    文件：PlayerMoveMotor.cs
    作者：Olivia
    日期：2022/1/23 0:0:50
    功能：Nothing
*****************************************************/

using UnityEngine;
using System;

public class AnimatorConst
{
    //public const string
    public const string Forward = "forward";
    public const string Jump = "jump";
    public const string IsGround = "isGround";
    public const string Roll = "roll";
    public const string JabVelocity = "jabVelocity";
}

public class ActorController : MonoBehaviour
{
    public GameObject ActorObj;
    [SerializeField]
    private Animator mAnim;
    [SerializeField]
    private PlayerInput mInput;
    [SerializeField]
    private Rigidbody mRigid;
    
    [SerializeField] private float mWalkSpeed = 2.4f;
    [SerializeField] private float mRunMultiplier = 2.7f;
    [SerializeField] private float mJumpVelocity = 4.0f;
    [SerializeField] private float mRollSpeed = 1.0f;  //翻滚的速度
    [SerializeField] private float mRollVelocity = 3.0f;

    private Vector3 mMovingVect;
    private Vector3 mThrusVec;
    private bool mLockPlanar;
    
    private void Awake()
    {
        mAnim = GetComponentInChildren<Animator>();
        mInput = GetComponent<PlayerInput>();
        mRigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float targetMulti = mInput.Dmag * (mInput.Run ? mRunMultiplier : 1.0f);
        mAnim.SetFloat(AnimatorConst.Forward, Mathf.Lerp(mAnim.GetFloat(AnimatorConst.Forward), targetMulti, 0.5f));
        if(mRigid.velocity.magnitude > mRollSpeed)
        {
            mAnim.SetTrigger(AnimatorConst.Roll);
        }


        if (mInput.Jump)
        {
            mAnim.SetTrigger(AnimatorConst.Jump);
        }
        //s有移动
         if (mInput.Dmag > 0.1f)
         {
             ActorObj.transform.forward = Vector3.Slerp(ActorObj.transform.forward, mInput.DVect, 0.1f);
         }

         if (!mLockPlanar)
         {
             mMovingVect = mInput.Dmag * ActorObj.transform.forward * mWalkSpeed * (mInput.Run ? mRunMultiplier : 1.0f);    
         }
    }

    private void FixedUpdate()
    {
        mRigid.velocity = new Vector3(mMovingVect.x, mRigid.velocity.y, mMovingVect.z) + mThrusVec;
        mThrusVec = Vector3.zero;
    }


    #region 消息事件
    public void OnJumpEnter()
    {
        mInput.InputEnabled = false;
        mLockPlanar = true;
        mThrusVec = new Vector3(0, mJumpVelocity, 0);
    }

    public void OnGround()
    {
        mAnim.SetBool(AnimatorConst.IsGround, true);
    }

    public void NotOnGround()
    {
        mAnim.SetBool(AnimatorConst.IsGround, false);
    }

    public void OnGroundEnter()
    {
        mInput.InputEnabled = true;
        mLockPlanar = false;
    }

    public void OnFallEnter()
    {
        mInput.InputEnabled = false;
        mLockPlanar = true;
    }

    public void OnRollEnter()
    {
        mInput.InputEnabled = false;
        mLockPlanar = true;
        mThrusVec = new Vector3(0, mRollVelocity, 0);
    }

    public void OnJabEnter()
    {
        mInput.InputEnabled = false;
        mLockPlanar = true;
    }

    public void OnJabUpdate()
    {
        mThrusVec = ActorObj.transform.forward * mAnim.GetFloat(AnimatorConst.JabVelocity) ;
    }
    #endregion

}
