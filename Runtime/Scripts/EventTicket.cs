using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif
using UnityEngine.UIElements;
using UnityEngine.Events;

namespace EasyTalkSystem
{
    /// <summary>
    /// イベントを呼び出すチケット
    /// </summary>
    [System.Serializable]
    public class EventTicket : Ticket
    {
        [SerializeField]
        private UnityEvent _action;

        public override async UniTask UseAsync(CancellationToken cancellationToken)
        {
            _action?.Invoke();
            await UniTask.CompletedTask; // 形式的に待機しないといけないため
        }
    }
}
