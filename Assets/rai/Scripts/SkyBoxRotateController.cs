using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxRotateController : MonoBehaviour
{
    public Transform cameraTransform;

    void Update()
    {
        // カメラの回転に合わせてSkyboxの正面が常に+Zを向くように調整
        float yaw = cameraTransform.eulerAngles.y;
        RenderSettings.skybox.SetFloat("_Rotation", yaw);
    }
}
