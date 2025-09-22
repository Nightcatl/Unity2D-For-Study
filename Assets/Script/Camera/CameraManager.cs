using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour, IManager
{
    public static CameraManager instance;

    [SerializeField] private List<CinemachineVirtualCamera> allVirtualCameras;

    [SerializeField] private float fallPanAmount_Damp = 0.25f;
    [SerializeField] private float fallYpanTime_Damp = 0.35f;
    public float fallSpeedYDampingChangeThreshold = -15f;

    [SerializeField] private float fallYpanTime_Offset = 0.7f;

    public bool IsLerpingYDamping {  get; private set; }
    public bool LerpedFromPlayerFalling_Damp;

    public bool IsLerpingYOffset {  get; private set; }
    public bool LerpedFromPlayerFalling_Offset;

    private Coroutine lerpYPanCoroutine;
    private Coroutine lerpYOffsetCoroutine;
    private Coroutine panCameraCoroutine;

    private CinemachineVirtualCamera currentCamera;
    private CinemachineFramingTransposer framingTranposer;

    private float normYPanAmount;
    private float normYOffest;

    private Vector2 startingTrackedObjectOffset;

    public void Initialize()
    {
        if (instance == null)
            instance = this;

        allVirtualCameras = new List<CinemachineVirtualCamera>();
    }

    public void FindAllVirtualCamera()
    {
        if(CameraListManager.instance != null)
        {
            allVirtualCameras = CameraListManager.instance.cameraList;

            for (int i = 0; i < allVirtualCameras.Count; i++)
            {
                if (allVirtualCameras[i].enabled)
                {
                    currentCamera = allVirtualCameras[i];

                    framingTranposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

                    normYPanAmount = framingTranposer.m_YDamping;
                    normYOffest = framingTranposer.m_TrackedObjectOffset.y;

                    startingTrackedObjectOffset = framingTranposer.m_TrackedObjectOffset;
                }
            }
        }
    }

    #region Handle Falling And Rise
    public void LerpYDamping(bool changeYDamp)
    {
        lerpYPanCoroutine = StartCoroutine(LerpYAction(changeYDamp));
    }

    private IEnumerator LerpYAction(bool changeYDamp)
    {
        IsLerpingYDamping = true;

        float startDampAmount = framingTranposer.m_YDamping;
        float endDampAmount = 0f;

        if(changeYDamp)
        {
            endDampAmount = fallPanAmount_Damp;
            LerpedFromPlayerFalling_Damp = true;
        }
        else
        {
            endDampAmount = normYPanAmount;
        }

        float elapsedTime = 0f;

        while(elapsedTime < fallYpanTime_Damp)
        {
            elapsedTime += Time.deltaTime;

            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / fallYpanTime_Damp));

            framingTranposer.m_YDamping = lerpedPanAmount;

            yield return null;
        }

        IsLerpingYDamping = false;
    }

    public void LerpYOffest(bool changeYOffest)
    {
        lerpYOffsetCoroutine = StartCoroutine(LerpYOffestFor(changeYOffest));
    }

    public IEnumerator LerpYOffestFor(bool changeYOffest)
    {
        IsLerpingYOffset = true;

        float startOffsetAmount = framingTranposer.m_TrackedObjectOffset.y;
        float endOffestAmount = 0f;

        if (changeYOffest)
        {
            endOffestAmount = -2.0f;
            LerpedFromPlayerFalling_Offset = true;
        }
        else
        {
            endOffestAmount = 5.0f;
        }

        float elapsedTime = 0f;

        while (elapsedTime < fallYpanTime_Offset)
        {
            elapsedTime += Time.deltaTime;

            float lerpedOffestAmount = Mathf.Lerp(startOffsetAmount, endOffestAmount, (elapsedTime / fallYpanTime_Offset));

            framingTranposer.m_TrackedObjectOffset.y = lerpedOffestAmount;

            yield return null;
        }

        IsLerpingYOffset = false;
    }

    #endregion

    #region Pan Camera
    public void PanCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
    }

    private IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;

        if(!panToStartingPos)
        {
            switch(panDirection)
            {
                case PanDirection.Up:
                    endPos = Vector2.up;
                    break;
                case PanDirection.Down:
                    endPos = Vector2.down;
                    break;
                case PanDirection.Left:
                    endPos = Vector2.left; 
                    break;
                case PanDirection.Right:
                    endPos = Vector2.right;
                    break;
                default:
                    break;
            }
            endPos *= panDistance;

            startingPos = startingTrackedObjectOffset;

            endPos += startingPos;
        }

        else
        {
            startingPos = framingTranposer.m_TrackedObjectOffset;
            endPos = startingTrackedObjectOffset;
        }

        float elapsedTime = 0f;

        while(elapsedTime < panTime)
        {
            elapsedTime += panTime;

            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, (elapsedTime / panTime));
            framingTranposer.m_TrackedObjectOffset = panLerp;

            yield return null;
        }
    }
    #endregion

    #region Swap Cameras

    public void SwapCameraByContidion(CinemachineVirtualCamera cameraFromEnter, CinemachineVirtualCamera cameraFromEnd, bool whatCameraShouldEnable)
    {
        if (whatCameraShouldEnable)
        {
            cameraFromEnd.enabled = false;

            cameraFromEnter.enabled = true;

            currentCamera = cameraFromEnter;

            framingTranposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        else
        {
            cameraFromEnter.enabled = false;

            cameraFromEnd.enabled = true;

            currentCamera = cameraFromEnd;

            framingTranposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

            framingTranposer.m_TrackedObjectOffset.y = 5f;
        }   
    }

    public void SwapCameraByTransform(CinemachineVirtualCamera cameraFromLeft, CinemachineVirtualCamera cameraFromRight, Vector2 triggerExitDirection)
    {
        if(currentCamera == cameraFromLeft && triggerExitDirection.x > 0f)
        {
            cameraFromRight.enabled = true;

            cameraFromLeft.enabled = false;

            currentCamera = cameraFromRight;

            framingTranposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        else if(currentCamera == cameraFromRight && triggerExitDirection.x < 0f)
        {
            cameraFromLeft.enabled = true;

            cameraFromRight.enabled = false;

            currentCamera = cameraFromLeft;

            framingTranposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }

    #endregion
}
