using Cysharp.Threading.Tasks;
using System.Threading;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UIElements;

namespace EasyTalkSystem
{
    /// <summary>
    /// クリック待機チケット
    /// </summary>
    [System.Serializable]
    public class WaitClickTicket : Ticket
    {
        public override async UniTask UseAsync(CancellationToken cancellationToken)
        {
            await WaitClickAsync(cancellationToken);
        }
    }

}
