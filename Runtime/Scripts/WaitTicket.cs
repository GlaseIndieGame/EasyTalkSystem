using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace EasyTalkSystem
{
    /// <summary>
    /// 指定時間待機するチケット
    /// </summary>
    [System.Serializable]
    public class WaitTicket : Ticket
    {
        [SerializeField, DisplayName("待機時間")]
        private float _duration = 2.5f;

        public override async UniTask UseAsync(CancellationToken cancellationToken)
        {
            await UniTask.WaitForSeconds(_duration, false, PlayerLoopTiming.FixedUpdate, cancellationToken);
        }
    }
}
