using System.Collections;
using UnityEngine;

// モード選択用の列挙型
public enum AttackMode
{
    Delayed,  // 遅延ウェーブモード
    Wave      // 波状拡散モード
}

public class BulletHellAttack : MonoBehaviour
{
    [Header("発射設定")]
    public GameObject bulletPrefab;      // 発射する弾のプレハブ
    public Transform spawnPoint;         // 発射位置
    [Tooltip("弾の初速")]
    public float bulletSpeed = 10f;      // 弾の基本移動速度
    [Tooltip("弾の寿命（秒）。経過後自動的に破棄されます。")]
    public float bulletLifetime = 5f;    // 発射された弾の寿命

    #region 【遅延ウェーブモード用設定】
    [Header("【遅延ウェーブモード】")]
    [Tooltip("遅延ウェーブモード用の扇形角度（例: 360なら円形になります）")]
    public float delayedFanAngle = 360f;
    [Tooltip("発射するウェーブ（列）の数")]
    public int delayedWaveCount = 5;         // 発射するウェーブ数（例：5回の円が順次発射）
    [Tooltip("各ウェーブで発射する弾の数")]
    public int shotsPerWave = 20;            // 1ウェーブあたりの弾数（delayedFanAngleにより円状等に配置）
    [Tooltip("ウェーブ間の待機時間（秒）")]
    public float waveDelay = 2f;             // 各ウェーブ発射後の待機時間
    [Tooltip("各ウェーブごとの回転オフセット（度）")]
    public float waveRotationOffset = 10f;   // 各ウェーブでパターン全体を回転させるオフセット
    [Tooltip("各ウェーブで使用する扇形の角度を個別に設定する場合の配列。要素数が足りなければ delayedFanAngle が使用されます。")]
    public float[] customDelayedFanAngles;
    #endregion

    #region 【波状拡散モード用設定】
    [Header("【波状拡散モード】")]
    [Tooltip("波状拡散モード用の扇形角度（例: 60なら狭い扇状になります）")]
    public float waveFanAngle = 60f;
    [Tooltip("総発射弾数")]
    public int totalShots = 50;
    [Tooltip("弾発射間隔（秒）")]
    public float shotInterval = 0.1f;
    [Tooltip("波状オフセットの振幅（度）")]
    public float waveAmplitude = 5f;
    [Tooltip("波状オフセットの周波数")]
    public float waveFrequency = 1f;
    #endregion

    [Header("モード選択（インスペクタから選択する場合）")]
    [Tooltip("チェックを入れると波状拡散モードを使用します。チェックが外れている場合は遅延ウェーブモードが使用されます。")]
    public bool useWavePattern = false;

    /// <summary>
    /// 引数なしの Fire() は、インスペクタの useWavePattern によりモードを選択して呼び出します。
    /// </summary>
    public void Fire()
    {
        Fire(useWavePattern ? AttackMode.Wave : AttackMode.Delayed);
    }

    /// <summary>
    /// AttackMode を引数として指定することで、発射モードを明示的に選択できます。
    /// </summary>
    /// <param name="mode">AttackMode.Delayed / AttackMode.Wave</param>
    public void Fire(AttackMode mode)
    {
        if (mode == AttackMode.Wave)
        {
            StartCoroutine(FireWavePatternCoroutine());
        }
        else
        {
            StartCoroutine(FireDelayedCoroutine());
        }
    }

    /// <summary>
    /// 【遅延ウェーブモード】
    /// spawnPoint から、例えば delayedFanAngle を 360 に設定すれば円状に shotsPerWave 個の弾を一斉発射し、
    /// waveDelay 秒ごとに delayedWaveCount 回発射します。
    /// 各ウェーブは waveRotationOffset ずつ回転し、さらに customDelayedFanAngles 配列が設定されている場合は
    /// 各ウェーブごとに異なる扇形角度が使用されます。
    /// </summary>
    private IEnumerator FireDelayedCoroutine()
    {
        for (int wave = 0; wave < delayedWaveCount; wave++)
        {
            // 各ウェーブごとに回転オフセットを付与
            float waveOffset = wave * waveRotationOffset;
            // 各ウェーブで使用する扇形角度。customDelayedFanAnglesが設定されていればその値、なければ delayedFanAngle を使用
            float currentFanAngle = (customDelayedFanAngles != null && customDelayedFanAngles.Length > wave)
                                    ? customDelayedFanAngles[wave]
                                    : delayedFanAngle;

            // 1ウェーブで shotsPerWave 個の弾を発射
            for (int j = 0; j < shotsPerWave; j++)
            {
                float baseAngle = 0f;
                if (Mathf.Approximately(currentFanAngle, 360f))
                {
                    // currentFanAngle が360なら、弾は 360/shotsPerWave ごとに配置（例：0, 18, 36, ...）
                    baseAngle = j * (360f / shotsPerWave);
                }
                else
                {
                    // 360以外なら、-currentFanAngle/2～currentFanAngle/2 の範囲で均等配置
                    baseAngle = (shotsPerWave == 1) ? 0f : Mathf.Lerp(-currentFanAngle / 2f, currentFanAngle / 2f, (float)j / (shotsPerWave - 1));
                }
                float finalAngle = baseAngle + waveOffset;

                // spawnPoint の回転を基準に Y 軸周りに finalAngle 回転
                Quaternion rotation = spawnPoint.rotation * Quaternion.Euler(0, finalAngle, 0);
                // 発射時に Y 軸で 180 度回転（必要に応じて）
                rotation = rotation * Quaternion.Euler(0, 180, 0);

                GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, rotation);
                Destroy(bullet, bulletLifetime);

                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = rotation * Vector3.forward * bulletSpeed;
                }
                BulletMovement bm = bullet.GetComponent<BulletMovement>();
                if (bm != null)
                {
                    bm.speed = bulletSpeed;
                }
            }

            if (wave < delayedWaveCount - 1)
            {
                yield return new WaitForSeconds(waveDelay);
            }
        }
    }

    /// <summary>
    /// 【波状拡散モード】
    /// spawnPoint から連続発射し、各弾の発射角にサイン波状のオフセットを加えることで
    /// 波状に広がるパターンを実現します。ここでは waveFanAngle の値が基本となります。
    /// </summary>
    private IEnumerator FireWavePatternCoroutine()
    {
        for (int i = 0; i < totalShots; i++)
        {
            // 基本角度を waveFanAngle を使って -waveFanAngle/2～waveFanAngle/2 の範囲で計算
            float baseAngle = (totalShots == 1) ? 0f : Mathf.Lerp(-waveFanAngle / 2f, waveFanAngle / 2f, (float)i / (totalShots - 1));
            float additionalOffset = waveAmplitude * Mathf.Sin(i * shotInterval * waveFrequency * 2 * Mathf.PI);
            float finalAngle = baseAngle + additionalOffset;

            Quaternion rotation = spawnPoint.rotation * Quaternion.Euler(0, finalAngle, 0);
            rotation = rotation * Quaternion.Euler(0, 180, 0);

            GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, rotation);
            Destroy(bullet, bulletLifetime);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = rotation * Vector3.forward * bulletSpeed;
            }
            BulletMovement bm = bullet.GetComponent<BulletMovement>();
            if (bm != null)
            {
                bm.speed = bulletSpeed;
            }

            yield return new WaitForSeconds(shotInterval);
        }
    }
}
