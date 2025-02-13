using UnityEngine;
using UnityEngine.Playables;

public class TimelineEffectController : MonoBehaviour
{
    // InspectorでPlayableDirectorをアサイン
    public PlayableDirector playableDirector;

    // エフェクトをまとめたGameObject（例：EffectContainer）をアサイン
    public GameObject effectContainer;

    public CommentManager commentManager;

    // ボタンなどで呼び出す関数
    public void PlayEffectTimeline()
    {
        if (playableDirector == null)
        {
            Debug.LogWarning("PlayableDirectorが設定されていません！");
            return;
        }
        if (effectContainer == null)
        {
            Debug.LogWarning("Effect Containerが設定されていません！");
            return;
        }

        // エフェクトを表示
        effectContainer.SetActive(true);
        commentManager.Attack();

        // 一旦タイムラインの再生を停止し、0秒にリセット
        playableDirector.Stop();
        playableDirector.time = 0;
        playableDirector.Evaluate();

        // 再生完了後にコールバックでエフェクトを非表示にするため、stoppedイベントに登録
        playableDirector.stopped += OnPlayableDirectorStopped;

        // タイムラインの再生開始
        playableDirector.Play();
    }

    // PlayableDirectorの再生が終了したときに呼ばれるイベントコールバック
    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        // イベントが複数回呼ばれないように解除
        director.stopped -= OnPlayableDirectorStopped;

        // エフェクトを非表示にする
        if (effectContainer != null)
        {
            effectContainer.SetActive(false);
        }
    }
}
