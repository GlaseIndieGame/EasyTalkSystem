using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

namespace EasyTalkSystem
{
    /// <summary>
    /// イベントの終了を待機するチケット
    /// </summary>
    [System.Serializable]
    public class WaitEventTicket : Ticket, IUniTaskWaitable
    {
        [SerializeField]
        private UnityEvent<CancellationToken, IUniTaskWaitable> _waitAction; // 待機可能なアクションを

        private System.Func<UniTask> _factory = null; // UniTaskのファクトリーメソッドを保持

        public override async UniTask UseAsync(CancellationToken cancellationToken)
        {
            if (_waitAction == null) { return; }
            _waitAction.Invoke(cancellationToken, this);
            if (_factory == null) { return; }
            await UniTask.Create(_factory);
            _factory = null;
        }

        public void SetUniTaskFactory(System.Func<UniTask> factory) => _factory = factory;
    }

    /// <summary>
    /// UniTaskを待機可能
    /// </summary>
    public interface IUniTaskWaitable
    {
        /// <summary>
        /// UniTaskのファクトリーメソッドを設定
        /// </summary>
        /// <param name="factory"></param>
        public void SetUniTaskFactory(System.Func<UniTask> factory);
    }
}
