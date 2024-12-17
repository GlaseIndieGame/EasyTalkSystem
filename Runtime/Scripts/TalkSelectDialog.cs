using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace EasyTalkSystem
{
    /// <summary>
    /// 選択肢を表示するダイアログ
    /// </summary>
    public class TalkSelectDialog : MonoBehaviour
    {
        [SerializeField]
        private TalkButton _buttonPrefab;

        [SerializeField]
        List<TalkButton> _buttons = new();

        private int _result = -1;

        private void Awake()
        {
            for (int i = 0; i < _buttons.Count; i++)
            {
                int selectIndex = i;
                _buttons[i].onClick.AddListener(() => _result = selectIndex);
            }
        }

        /// <summary>
        /// 指定文字列配列で選択肢を表示し待機
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="buttonLabels"></param>
        /// <returns>押された選択肢のIndex</returns>
        public async UniTask<int> SelectButtonAsync(CancellationToken cancellationToken, params string[] buttonLabels)
        {
            _result = -1; // 待機条件用に初期化

            for (int i = 0; i < buttonLabels.Length; i++)
            {
                // 動的に選択肢を生成するため処理を割り当てる
                if (_buttons.Count <= i)
                {
                    int selectIndex = i;
                    _buttons.Add(Instantiate(_buttonPrefab, transform));
                    _buttons[i].onClick.AddListener(() => _result = selectIndex);
                }

                _buttons[i].SetLabel(buttonLabels[i]);
                _buttons[i].gameObject.SetActive(true);
            }

            await UniTask.WaitWhile(() => _result < 0, PlayerLoopTiming.FixedUpdate, cancellationToken);

            for (int i = 0; i < buttonLabels.Length; i++)
            {
                _buttons[i].gameObject.SetActive(false);
            }

            return _result;
        }
    }
}
