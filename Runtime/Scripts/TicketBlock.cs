using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace EasyTalkSystem
{
    [Serializable]
    public class TicketBlock
    {
        [SerializeField]
        private string _name = "ブロック名を入力";

        [SerializeReference]
        private List<Ticket> _talkBlock = new();

        internal string Name => _name;

        /// <summary>
        /// ブロックを再生
        /// </summary>
        /// <param name="_blockPlayer"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async UniTask PlayBlockAsync(BlockPlayer _blockPlayer, CancellationToken cancellationToken)
        {
            if (_talkBlock?.Count > 0)
            {
                for (int i = 0; i < _talkBlock.Count; i++)
                {
                    // 中断時のアクションを設定しておく
                    _blockPlayer.BlockCallBack += _talkBlock[i].End;
                    await _talkBlock[i].UseAsync(cancellationToken);
                }
            }
        }

        /// <summary>
        /// チケットの追加
        /// </summary>
        /// <param name="ticket"></param>
        public void AddTickets(Ticket ticket)
        {
            _talkBlock.Add(ticket);
        }
    }
}
