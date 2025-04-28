using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerDebugLogger : MonoBehaviour
{
    void Start()
    {
        if (Gamepad.current != null)
        {
            Debug.Log("🎮 コントローラーが接続されています: " + Gamepad.current.name);
        }
        else
        {
            Debug.LogWarning("❗ コントローラーが見つかりません。接続されていますか？");
        }
    }

    void Update()
    {
        if (Gamepad.current == null) return;

        Vector2 leftStick = Gamepad.current.leftStick.ReadValue();
        if (leftStick != Vector2.zero)
        {
            Debug.Log("🕹 左スティック入力: " + leftStick);
        }

        if (Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            Debug.Log("🅰 ボタンSouth（通常はAボタン）が押されました");
        }

        if (Gamepad.current.buttonWest.wasPressedThisFrame)
        {
            Debug.Log("🅧 ボタンWest（通常はXボタン）が押されました");
        }

        if (Gamepad.current.startButton.wasPressedThisFrame)
        {
            Debug.Log("▶ Startボタンが押されました");
        }
    }
}
