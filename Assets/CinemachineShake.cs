using UnityEngine;
using Cinemachine;
using System.Collections;

public class CinemachineShake : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeAmplitude = 3f;
    [SerializeField] private float shakeFrequency = 5f;

    private CinemachineBasicMultiChannelPerlin noiseComponent;

  private void Start()
{
    if (virtualCamera != null)
    {
        noiseComponent = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        // 初期状態は揺れなしにする
        noiseComponent.m_AmplitudeGain = 0f;
        noiseComponent.m_FrequencyGain = 0f;
    }
    else
    {
        Debug.LogError("Virtual Camera が設定されていません。");
    }
}

    public void StartShakeCamera()
    {
       
            StartCoroutine(ShakeCamera());
        
    }

    private IEnumerator ShakeCamera()
    {
        float elapsed = 0f;
        // シェイク開始時に振幅と周波数を設定
        noiseComponent.m_AmplitudeGain = shakeAmplitude;
        noiseComponent.m_FrequencyGain = shakeFrequency;

        // 指定時間経過するまでシェイクを維持し、その間徐々に振幅を減衰させる
        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;
            // 減衰（リニア補間）
            noiseComponent.m_AmplitudeGain = Mathf.Lerp(shakeAmplitude, 0f, elapsed / shakeDuration);
            yield return null;
        }

        // シェイク終了後は値をリセット
        noiseComponent.m_AmplitudeGain = 0f;
        noiseComponent.m_FrequencyGain = 0f;
    }
}
