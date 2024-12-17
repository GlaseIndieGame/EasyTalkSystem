using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace EasyTalkSystem
{
    /// <summary>
    /// チケットの種類
    /// </summary>
    public enum TicketType
    {
        TALK = 0,
        POSTTALK,
        EVENT,
        WAIT,
        CHOOSE,
        WAITCLICK,
        CALL,
        WAITEVENT,
    }

    /// <summary>
    /// 会話を構成するチケットのベースクラス
    /// </summary>
    [System.Serializable]
    public abstract class Ticket
    {
        /// <summary>
        /// チケットの終了処理
        /// </summary>
        public virtual void End() { }

        /// <summary>
        /// チケットの処理を実行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public abstract UniTask UseAsync(CancellationToken cancellationToken);

        /// <summary>
        /// クリック待機
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async UniTask WaitClickAsync(CancellationToken cancellationToken) => await UniTask.WaitWhile(() => !Input.GetButtonDown("Submit") && !Input.GetMouseButtonDown(0), PlayerLoopTiming.Update, cancellationToken);
    }
}
