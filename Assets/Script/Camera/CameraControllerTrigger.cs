using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;

public enum PanDirection
{
    Up,
    Down,
    Left,
    Right
}

public class CameraControllerTrigger : MonoBehaviour
{
    public CustonInspecttorObjects CustomInspectorObjects;

    private Collider2D cd;

    private void Start()
    {
        cd = GetComponent<Collider2D>();
    }

    [ContextMenu("ReturnToCamera")]
    public void ReturnToCamera()
    {
        CameraManager.instance.SwapCameraByContidion(CustomInspectorObjects.cameraOnLeft, CustomInspectorObjects.cameraOnRight, !CustomInspectorObjects.whatCameraShouldEnable);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(CustomInspectorObjects.panCameraOnContact)
            {
                CameraManager.instance.PanCameraOnContact(CustomInspectorObjects.panDistance, CustomInspectorObjects.panTime, CustomInspectorObjects.panDirection, false);
            }

            if(CustomInspectorObjects.swapCamerasByCondition && CustomInspectorObjects.cameraOnLeft != null && CustomInspectorObjects.cameraOnRight !=null)
            {
                CameraManager.instance.SwapCameraByContidion(CustomInspectorObjects.cameraOnLeft, CustomInspectorObjects.cameraOnRight, CustomInspectorObjects.whatCameraShouldEnable);

                cd.enabled = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Vector2 exitDirection = (collision.transform.position - cd.bounds.center).normalized;

            if (CustomInspectorObjects.swapCamerasByTransform && CustomInspectorObjects.cameraOnLeft != null && CustomInspectorObjects.cameraOnRight != null)
            {
                CameraManager.instance.SwapCameraByTransform(CustomInspectorObjects.cameraOnLeft, CustomInspectorObjects.cameraOnRight, exitDirection);
            }

            if (CustomInspectorObjects.panCameraOnContact)
            {
                CameraManager.instance.PanCameraOnContact(CustomInspectorObjects.panDistance, CustomInspectorObjects.panTime, CustomInspectorObjects.panDirection, true);
            }
        }
    }
}

[System.Serializable]
public class CustonInspecttorObjects
{
    public bool swapCamerasByTransform = false;
    public bool panCameraOnContact = false;
    public bool swapCamerasByCondition = false;

    [HideInInspector] public CinemachineVirtualCamera cameraOnLeft;
    [HideInInspector] public CinemachineVirtualCamera cameraOnRight;

    [HideInInspector] public bool whatCameraShouldEnable;

    [HideInInspector] public PanDirection panDirection;
    [HideInInspector] public float panDistance = 3f;
    [HideInInspector] public float panTime = 0.35f;
}

#if UNITY_EDITOR
[CustomEditor(typeof(CameraControllerTrigger))]
public class MyScriptEditor : Editor
{
    CameraControllerTrigger cameraControllerTrigger;

    private void OnEnable()
    {
        cameraControllerTrigger = (CameraControllerTrigger)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (cameraControllerTrigger.CustomInspectorObjects.swapCamerasByCondition)
        {
            cameraControllerTrigger.CustomInspectorObjects.cameraOnLeft = EditorGUILayout.ObjectField("Camera on Enter", cameraControllerTrigger.CustomInspectorObjects.cameraOnLeft,
                typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;

            cameraControllerTrigger.CustomInspectorObjects.cameraOnRight = EditorGUILayout.ObjectField("Camera on End", cameraControllerTrigger.CustomInspectorObjects.cameraOnRight,
                typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;

            cameraControllerTrigger.CustomInspectorObjects.whatCameraShouldEnable = EditorGUILayout.Toggle("What Camera Should Enable", cameraControllerTrigger.CustomInspectorObjects.whatCameraShouldEnable);
        }

        if (cameraControllerTrigger.CustomInspectorObjects.swapCamerasByTransform)
        {
            cameraControllerTrigger.CustomInspectorObjects.cameraOnLeft = EditorGUILayout.ObjectField("Camera on Left", cameraControllerTrigger.CustomInspectorObjects.cameraOnLeft,
                typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;

            cameraControllerTrigger.CustomInspectorObjects.cameraOnRight = EditorGUILayout.ObjectField("Camera on Right", cameraControllerTrigger.CustomInspectorObjects.cameraOnRight,
                typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
        }

        if(cameraControllerTrigger.CustomInspectorObjects.panCameraOnContact)
        {
            cameraControllerTrigger.CustomInspectorObjects.panDirection = (PanDirection)EditorGUILayout.EnumPopup("Camera Pan Dirction",
                cameraControllerTrigger.CustomInspectorObjects.panDirection);

            cameraControllerTrigger.CustomInspectorObjects.panDistance = EditorGUILayout.FloatField("Pan Distance", cameraControllerTrigger.CustomInspectorObjects.panDistance);
            cameraControllerTrigger.CustomInspectorObjects.panTime = EditorGUILayout.FloatField("Pan Time", cameraControllerTrigger.CustomInspectorObjects.panTime);
        }

        if(GUI.changed)
        {
            EditorUtility.SetDirty(cameraControllerTrigger);
        }
    }
}
#endif

