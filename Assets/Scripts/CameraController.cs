/****************************************************
    文件：CameraController.cs
    作者：Olivia
    日期：2022/2/13 21:28:16
    功能：摄像机控制器
*****************************************************/

using UnityEngine;

namespace Game
{
    public class CameraController : MonoBehaviour
    {
        public PlayerInput mPI;
        public float HorizontalSpeed;
        public float VerticalSpeed;
        public float CameraDampValue = 0.05f;

        private GameObject mPlayerHandler;
        private GameObject mCameraHandler;
        private float mTempEulerX;
        private GameObject mActorObj;
        private GameObject mCamera;

        private Vector3 mCameraDampVelocity;

        private void Awake()
        {
            mCameraHandler = transform.parent.gameObject;
            mPlayerHandler = mCameraHandler.transform.parent.gameObject;
            mTempEulerX = 20;
            mActorObj = mPlayerHandler.GetComponent<ActorController>().ActorObj;
            mCamera = Camera.main.gameObject;
        }

        private void FixedUpdate()
        {
            Vector3 rTempActorEuler = mActorObj.transform.eulerAngles;
            mPlayerHandler.transform.Rotate(Vector3.up, mPI.Jright * HorizontalSpeed * Time.fixedDeltaTime);
            //mCameraHandler.transform.Rotate(Vector3.right, mPI.Jup * -VerticalSpeed * Time.deltaTime);
            //mTempEulerX = mCameraHandler.transform.eulerAngles.x;
            mTempEulerX -= mPI.Jup * VerticalSpeed * Time.deltaTime;
            mTempEulerX = Mathf.Clamp(mTempEulerX, -40, 30);
            mCameraHandler.transform.localEulerAngles = new Vector3(mTempEulerX, 0, 0);
            mActorObj.transform.eulerAngles = rTempActorEuler;

            mCamera.transform.position = Vector3.SmoothDamp(mCamera.transform.position, transform.position, ref mCameraDampVelocity, CameraDampValue);
            mCamera.transform.eulerAngles = transform.eulerAngles;
        }
    }
}

