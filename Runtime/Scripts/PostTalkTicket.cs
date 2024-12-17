using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace EasyTalkSystem
{
    /// <summary>
    /// テキスト追記チケット
    /// </summary>
    [System.Serializable]
    public class PostTalkTicket : Ticket
    {
        [SerializeField]
        private string _postText = "追記内容を入力";

        [SerializeField]
        private bool _isWaitForClick = false;

        [SerializeField]
        private bool _isAutoDuration = true;

        [SerializeField]
        private bool _isDisableDialog = false;

        [SerializeField]
        private float _duration = 0;

        [SerializeField]
        private int _typingIntervalFrame = 3;


        [SerializeField]
        private bool _isFollowCharacter = true;

        [SerializeField]
        private TalkCharacter _character;


        private bool _isUsed = false;

        public PostTalkTicket() { }

        public override async UniTask UseAsync(CancellationToken cancellationToken)
        {
            _isUsed = true;
            if (_duration == 0 && string.IsNullOrEmpty(_postText))
            {
                Next();
                return;
            }

            string _currentText = _character.Dialog.GetText();

            CharacterSpacing stringTyping = new(_postText, text => _character.Dialog.SetText(_currentText + text), _typingIntervalFrame);

            FollowDialog(cancellationToken).Forget();

            await stringTyping.StartCharacterSpacingAsync(cancellationToken);

            if (_isWaitForClick)
            {
                await ClickWithNextAsync(cancellationToken);
                return;
            }
            await DelayNextAsync(cancellationToken);
        }

        /// <summary>
        /// UTF8における文字バイトから判別したテキスト幅を返却
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private float GetUTF8CharacterLength(string text)
        {
            float characterLength = System.Text.Encoding.UTF8.GetByteCount(text) / 3;
            characterLength += (text.Length - characterLength) / 3;
            return characterLength;
        }

        /// <summary>
        /// ダイアログの追尾
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async UniTask FollowDialog(CancellationToken cancellationToken)
        {
            while (_isUsed)
            {
                if (_isFollowCharacter)
                {
                    var position = _character.transform.position;
                    position.z = 0;
                    position.y += (_character.DisplayHeight + _character.Dialog.PanelHeight() / 2);

                    _character.Dialog.transform.position = position;
                }
                await UniTask.WaitForFixedUpdate(cancellationToken);
            }
        }

        /// <summary>
        /// クリック待機後次へ
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async UniTask ClickWithNextAsync(CancellationToken cancellationToken)
        {
            await WaitClickAsync(cancellationToken);
            Next();
        }

        /// <summary>
        /// 指定時間後次へ
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async UniTask DelayNextAsync(CancellationToken cancellationToken)
        {
            if (_isAutoDuration)
            {
                _duration = GetUTF8CharacterLength(_postText);
            }
            await UniTask.WaitForSeconds(_duration, false, PlayerLoopTiming.FixedUpdate, cancellationToken);
            Next();
        }

        /// <summary>
        /// 次へ行くときの設定
        /// </summary>
        private void Next()
        {
            _isUsed = false;
            _character.Dialog.gameObject.SetActive(!_isDisableDialog);
        }

        public override void End()
        {
            _character.Dialog.gameObject.SetActive(false);
        }
    }
}
