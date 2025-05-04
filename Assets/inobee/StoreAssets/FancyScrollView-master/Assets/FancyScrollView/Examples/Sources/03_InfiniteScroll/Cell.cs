/*
 * FancyScrollView (https://github.com/setchi/FancyScrollView)
 * Copyright (c) 2020 setchi
 * Licensed under MIT (https://github.com/setchi/FancyScrollView/blob/master/LICENSE)
 */

using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example03
{
    class Cell : FancyCell<ItemData, Context>
    {
        [SerializeField] Animator animator = default;
        [SerializeField] Text message = default;
        [SerializeField] Text messageLarge = default;
        [SerializeField] Image image = default;
        [SerializeField] Image imageLarge = default;
        [SerializeField] Button button = default;

        //ここから追加
        [SerializeField] Text stageNumber;
        [SerializeField] Sprite[] stageSamune;
        [SerializeField] Image setSameuneImage;
        private KeteiButton keteiButton;

        static class AnimatorHash
        {
            public static readonly int Scroll = Animator.StringToHash("scroll");
        }

        void Start()
        {
            button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));
            keteiButton=GameObject.Find("Kettei").GetComponent<KeteiButton>();
            
        }

        public override void UpdateContent(ItemData itemData)
        {
            message.text = itemData.Message;
            messageLarge.text = Index.ToString();

            /*
            var selected = Context.SelectedIndex == Index;
            imageLarge.color = image.color = selected
                ? new Color32(0, 255, 255, 100)
                : new Color32(255, 255, 255, 77);
            */
            
            
            var selected = Context.SelectedIndex == Index;
            image.color = selected
                ? new Color32(0, 0, 0, 200)
                : new Color32(255, 255, 255, 150);
            
            setSameuneImage.sprite=stageSamune[int.Parse(stageNumber.text)];
            if(selected)
            {

                keteiButton.GetNumber(int.Parse(stageNumber.text));
            }
            
            
            
        }

        public override void UpdatePosition(float position)
        {
            currentPosition = position;
            

            if (animator.isActiveAndEnabled)
            {
                animator.Play(AnimatorHash.Scroll, -1, position);
            }

            animator.speed = 0;
        }

        // GameObject が非アクティブになると Animator がリセットされてしまうため
        // 現在位置を保持しておいて OnEnable のタイミングで現在位置を再設定します
        float currentPosition = 0;

        void OnEnable() => UpdatePosition(currentPosition);
    }
}
