using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif
using UnityEngine.UIElements;

namespace EasyTalkSystem
{
    /// <summary>
    /// 選択肢の設定
    /// </summary>
    [System.Serializable]
    public class SelectSetting
    {
        [SerializeField]
        private string _label;

        [SerializeField]
        private string _blockName;

        /// <summary>
        /// ラベルを取得
        /// </summary>
        public string Label => _label;

        /// <summary>
        /// ブロック名を取得
        /// </summary>
        public string BlockName => _blockName;
    }
    /// <summary>
    /// 選択肢用チケット
    /// </summary>
    [System.Serializable]
    public class ChooseTicket : Ticket
    {
        [SerializeField]
        private SelectSetting[] _selectSettings =
        {
            new(),
            new(),
        };

        [SerializeField, DisplayName("使用する選択肢UI")]
        private TalkSelectDialog _talkSelectDialog;

        [SerializeField, DisplayName("使用するブロックプレイヤー")]
        private BlockPlayer _useBlockPlayer;

        private string[] _selectBlockNames;

        private string[] _selectLabels;

        public ChooseTicket() { }

#if UNITY_EDITOR
        /// <summary>
        /// エディター設定用コンストラクタ
        /// </summary>
        /// <param name="blockPlayer"></param>
        public ChooseTicket(BlockPlayer blockPlayer)
        {
            _useBlockPlayer = blockPlayer;
        }
#endif

        public override async UniTask UseAsync(CancellationToken cancellationToken)
        {
            // クリック判定の残りによる誤選択防止
            await UniTask.WaitForFixedUpdate(cancellationToken);

            // それぞれで扱いやすくするため2つの配列に分解
            _selectBlockNames = new string[_selectSettings.Length];
            _selectLabels = new string[_selectSettings.Length];

            for (int i = 0; i < _selectSettings.Length; i++)
            {
                _selectBlockNames[i] = _selectSettings[i].BlockName;
                _selectLabels[i] = _selectSettings[i].Label;
            }

            _talkSelectDialog.gameObject.SetActive(true);

            // 選択肢の選択を待機
            int index = await _talkSelectDialog.SelectButtonAsync(cancellationToken, _selectLabels);

            await _useBlockPlayer.PlayBlockAsync(_selectBlockNames[index], cancellationToken);
        }

        public override void End()
        {
            _talkSelectDialog.gameObject.SetActive(false);
        }
    }
}
