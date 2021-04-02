using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera shakingCamera;

    // Start is called before the first frame update
    void Start()
    {
        shakingCamera = GetComponent<CinemachineVirtualCamera>();
    }

    void StopShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin  cameraDetail;
        cameraDetail = shakingCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cameraDetail.m_AmplitudeGain = 0;
    }
    void StartShakeCamera()
    {
        CinemachineBasicMultiChannelPerlin cameraDetail;
        cameraDetail = shakingCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cameraDetail.m_AmplitudeGain = 0.4f;
    }
    private void OnEnable()
    {
        //SceneController.CameraShakeStop += StopShakeCamera;
        //SceneController.CameraShakeStart += StartShakeCamera;
    }
}
