using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace EasyTalkSystem
{
    /// <summary>
    /// 会話テキスト再生チケット
    /// </summary>
    [System.Serializable]
    public class TalkTicket : Ticket
    {
        [SerializeField]
        private string _text = "会話内容を入力";

        [SerializeField]
        private bool _isWaitForClick = false;

        [SerializeField]
        private bool _isAutoDuration = true;

        [SerializeField]
        private float _duration = 0;


        [SerializeField]
        private bool _isDisableDialog = false;

        [SerializeField]
        private int _typingIntervalFrame = 3;


        [SerializeField]
        private bool _isFollowCharacter = true;

        [SerializeField]
        private TalkFontSetting _fontSetting;

        [SerializeField]
        private TalkCharacter _character;


        private bool _isUsed = false;

        public TalkTicket() { }

#if UNITY_EDITOR
        /// <summary>
        /// エディター設定用コンストラクタ
        /// </summary>
        /// <param name="talkFontSetting"></param>
        public TalkTicket(TalkFontSetting talkFontSetting)
        {
            _fontSetting = talkFontSetting;
        }
#endif
        public override async UniTask UseAsync(CancellationToken cancellationToken)
        {
            _isUsed = true;
            if (_duration == 0 && string.IsNullOrEmpty(_text))
            {
                Next();
                return;
            }

            var textlines = _text.Split('\n');
            float max = GetMaxCharacterLength(textlines);

            _character.Dialog.SettingDialog("", textlines.Length, max, _fontSetting);

            CharacterSpacing stringTyping = new(_text, text => _character.Dialog.SetText(text), _typingIntervalFrame);

            _character.Dialog.transform.position = _character.transform.position + Vector3.up * (_character.Dialog.PanelHeight() / 2);

            FollowDialog(cancellationToken).Forget();

            _character.Dialog.gameObject.SetActive(_isUsed);

            await stringTyping.StartCharacterSpacingAsync(cancellationToken);

            if (_isWaitForClick)
            {
                await ClickWithNextAsync(cancellationToken);
                return;
            }
            await DelayNextAsync(cancellationToken);
        }

        /// <summary>
        /// 入力されたテキストの最大幅を計算します
        /// </summary>
        /// <param name="textlines"></param>
        /// <returns></returns>
        private float GetMaxCharacterLength(string[] textlines)
        {
            float max = 0;
            float characterLength;

            for (int i = 0; i < textlines.Length; i++)
            {
                characterLength = GetUTF8CharacterLength(textlines[i]);

                if (max < characterLength)
                {
                    max = characterLength;
                }
            }

            return max;
        }

        /// <summary>
        /// UTF8における文字バイトから判別したテキスト幅を返却
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private float GetUTF8CharacterLength(string text)
        {
            float utfEightFullPitchSize = 3;
            float characterLength = System.Text.Encoding.UTF8.GetByteCount(text) / utfEightFullPitchSize;
            characterLength += (text.Length - characterLength) / utfEightFullPitchSize;
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
                    var position = Camera.main.WorldToScreenPoint(_character.transform.position);
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
                _duration = GetUTF8CharacterLength(_text) * _fontSetting.TextSpacingSpeed;
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
