using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace EasyTalkSystem
{
    /// <summary>
    /// ブロックを呼び出すチケット
    /// </summary>
    [System.Serializable]
    public class CallTicket : Ticket
    {
        [SerializeField]
        private string _callBlock;

        [SerializeField]
        private BlockPlayer _useBlockPlayer;

        public CallTicket() { }

#if UNITY_EDITOR
        /// <summary>
        /// エディター設定用コンストラクタ
        /// </summary>
        /// <param name="blockPlayer"></param>
        public CallTicket(BlockPlayer blockPlayer)
        {
            _useBlockPlayer = blockPlayer;
        }
#endif

        public override async UniTask UseAsync(CancellationToken cancellationToken)
        {
            await _useBlockPlayer.PlayBlockAsync(_callBlock, cancellationToken);
        }
    }
}
