using UnityEngine;

public class RotateOnAxis : MonoBehaviour
{
    // 回転軸を選択するための列挙型
    public enum RotationAxis { X, Y, Z }

    // Inspector上で回転軸を選択可能
    public RotationAxis rotationAxis = RotationAxis.Z;

    // 1秒あたりの回転角度（度）
    public float rotationSpeed = 90f;

    void Update()
    {
        Vector3 axis = Vector3.zero;

        // 選択された軸に応じて、回転軸を決定
        switch (rotationAxis)
        {
            case RotationAxis.X:
                axis = Vector3.right;
                break;
            case RotationAxis.Y:
                axis = Vector3.up;
                break;
            case RotationAxis.Z:
                axis = Vector3.forward;
                break;
        }

        // Time.deltaTime を掛けることで、フレームレートに依存しない回転を実現
        transform.Rotate(axis, rotationSpeed * Time.deltaTime);
    }
}
