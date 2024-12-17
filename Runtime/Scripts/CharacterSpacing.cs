using Cysharp.Threading.Tasks;
using System.Threading;

namespace EasyTalkSystem
{

    /// <summary>
    /// 文字送りクラス
    /// </summary>
    public class CharacterSpacing
    {
        private string _text;
        private int _typingFrame;
        private System.Action<string> _onUpdatedText = null;

        /// <summary>
        /// 文字送りのパラメーターを設定
        /// </summary>
        /// <param name="text"></param>
        /// <param name="onUpdateText">更新イベント</param>
        /// <param name="typingFrame"></param>
        public CharacterSpacing(string text, System.Action<string> onUpdateText, int typingFrame = 5)
        {
            _text = text;
            _onUpdatedText = onUpdateText;
            _typingFrame = typingFrame < 0 ? 0 : typingFrame;
        }

        /// <summary>
        /// 文字送りを行い更新される毎にイベントを発行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async UniTask StartCharacterSpacingAsync(CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_text)) { return; }
            int index = 0;

            ++index;
            _onUpdatedText?.Invoke(_text.Substring(0, index));

            while (index < _text.Length)
            {
                await UniTask.DelayFrame(_typingFrame, PlayerLoopTiming.FixedUpdate, cancellationToken);

                ++index;
                _onUpdatedText?.Invoke(_text.Substring(0, index));
            }
        }
    }
}