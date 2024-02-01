using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public static CameraManager cameraManager;
    public Camera focusedCamera; // 메인 카메라로 사용할 카메라를 Inspector에서 할당

    public Camera camera1;
    public Camera camera2;
    public Camera camera3;

    Camera[] cameras;


    private void Awake()
    {
        if (cameraManager == null)
        {
            cameraManager = this;
        }
        else
        {
            Destroy(gameObject);
        }

        cameras = new Camera[] { camera1, camera2, camera3 };
        focusedCamera = camera1;

    }
    void Start()
    {
        foreach (Camera camera in cameras)
        {
            camera.enabled = false;
        }

        focusedCamera.enabled = true;
    }

    public void RequestChangeCamera(int cameraNum)
    {
        ChangeCamera(cameraNum);
    }

    private void ChangeCamera(int cameraNum)
    {
        focusedCamera.enabled = false;

        cameras[cameraNum - 1].enabled = true;
        focusedCamera = cameras[cameraNum - 1];


    }
}
