using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System;

namespace EasyTalkSystem
{
    /// <summary>
    /// 会話作成システム
    /// </summary>
    [DisallowMultipleComponent]
    public class BlockPlayer : MonoBehaviour
    {
        [SerializeField]
        private List<TicketBlock> _blocks = new();

        // 外側から設定できるようにしたい+コールバック終了後はすべて削除したいのでActionのリスト
        private List<Action> _blockCallBacks = new();

        // 素早く名前からアクセスできるようにDictionaryにしておく
        private Dictionary<string, int> _blockIndexDictionary = new();

        private bool _isPlay = false;

        private CancellationTokenSource _cancellationTokenSource = null;

        // TalkTypeから使用する会話モードを簡単に呼び出せるようにする
        private Action<string>[] _talkTypePlayBlockAction = null;

        public event Action BlockCallBack
        {
            add => _blockCallBacks.Add(value);
            remove => _blockCallBacks.Remove(value);
        }

        private void Awake()
        {
            Init();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        private void Init()
        {
            _talkTypePlayBlockAction = new Action<string>[]
            {
                PlayBlock,
                PlayEventBlock,
                PlayWaitBlock,
                null,
            };
            for (int i = 0; i < _blocks.Count; i++)
            {
                _blockIndexDictionary.Add(_blocks[i].Name, i);
            }
        }

        /// <summary>
        /// 会話をTalkTypeからモードを指定して再生
        /// </summary>
        /// <param name="name"></param>
        /// <param name="talkType"></param>
        public void PlayTalkTypeMatchBlock(string name, TalkManager.TalkType talkType)
        {
            _talkTypePlayBlockAction[(int)talkType]?.Invoke(name);
        }

        /// <summary>
        /// 上書き再生
        /// </summary>
        /// <param name="index">インデックス</param>
        public void PlayBlock(int index)
        {
            EndBlock();
            _cancellationTokenSource = new();
            _isPlay = true;
            PlayBlockDefaultAsync(index, _cancellationTokenSource.Token).Forget();
        }

        /// <summary>
        /// 上書き再生
        /// </summary>
        /// <param name="name">ブロック名</param>
        public void PlayBlock(string name)
        {
            if (_blockIndexDictionary.TryGetValue(name, out int index))
            {
                PlayBlock(index);
            }
        }

        /// <summary>
        /// キャラクターを会話中停止させて再生
        /// </summary>
        /// <param name="index"></param>
        public void PlayEventBlock(int index)
        {
            if (TalkManager.TryGetInstance(out TalkManager talkManager))
            {
                EndBlock();
                talkManager.StopCharacterOperation();
                _cancellationTokenSource = new();
                _isPlay = true;

                PlayBlockAsync(index, _cancellationTokenSource.Token, () =>
                {
                    EndBlock();
                    talkManager.StartCharacterOperation();
                }).Forget();
            }
        }

        /// <summary>
        /// キャラクターを会話中停止させて再生
        /// </summary>
        /// <param name="name"></param>
        public void PlayEventBlock(string name)
        {
            if (_blockIndexDictionary.TryGetValue(name, out int index))
            {
                PlayEventBlock(index);
            }
        }

        /// <summary>
        /// キャラクターを停止してから再生
        /// </summary>
        /// <param name="index"></param>
        public void PlayWaitBlock(int index)
        {
            if (TalkManager.TryGetInstance(out TalkManager talkManager))
            {
                EndBlock();
                talkManager.StopCharacterOperation();
                _cancellationTokenSource = new();
                _isPlay = true;

                PlayBlockDefaultAsync(index, _cancellationTokenSource.Token).Forget();
            }
        }

        /// <summary>
        /// キャラクターを停止してから再生
        /// </summary>
        /// <param name="name"></param>
        public void PlayWaitBlock(string name)
        {
            if (_blockIndexDictionary.TryGetValue(name, out int index))
            {
                PlayWaitBlock(index);
            }
        }

        /// <summary>
        /// 上書きせずに再生
        /// </summary>
        /// <param name="index">インデックス</param>
        public bool SoftPlayBlock(int index)
        {
            if (_isPlay)
            {
                return false;
            }
            _cancellationTokenSource = new();
            _isPlay = true;
            PlayBlockDefaultAsync(index, _cancellationTokenSource.Token).Forget();
            return true;
        }

        /// <summary>
        /// 上書きせずに再生
        /// </summary>
        /// <param name="name">ブロック名</param>
        public bool SoftPlayBlock(string name)
        {
            if (_blockIndexDictionary.TryGetValue(name, out int index))
            {
                return SoftPlayBlock(index);
            }
            return false;
        }

        /// <summary>
        /// ブロックを再生
        /// </summary>
        /// <param name="index"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="isClear"></param>
        /// <returns></returns>
        public async UniTask PlayBlockAsync(int index, CancellationToken cancellationToken, System.Action endAction = null)
        {
            if (_blocks?.Count > 0)
            {
                await _blocks[index].PlayBlockAsync(this, cancellationToken);
            }
            endAction?.Invoke();
        }

        /// <summary>
        /// デフォルト設定でブロックを再生
        /// </summary>
        /// <param name="index"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async UniTask PlayBlockDefaultAsync(int index, CancellationToken cancellationToken)
        {
            await PlayBlockAsync(index, cancellationToken, EndBlock);
        }

        /// <summary>
        /// ブロックを再生
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="endAction"></param>
        /// <returns></returns>
        public async UniTask PlayBlockAsync(string name, CancellationToken cancellationToken, System.Action endAction = null)
        {
            if (_blockIndexDictionary.TryGetValue(name, out int index))
            {
                await PlayBlockAsync(index, cancellationToken, endAction);
            }
        }

        /// <summary>
        /// ブロックを停止
        /// </summary>
        public void EndBlock()
        {
            if (_cancellationTokenSource != null)
            {
                foreach (var item in _blockCallBacks)
                {
                    item?.Invoke();
                }
                _blockCallBacks.Clear();

                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
                _isPlay = false;
            }
        }

        private void OnDestroy()
        {
            EndBlock();
        }

#if UNITY_EDITOR
        /// <summary>
        /// エディター拡張用ブロックを追加
        /// </summary>
        public List<TicketBlock> Blocks => _blocks;

        /// <summary>
        /// エディター拡張用チケット追加
        /// </summary>
        /// <param name="makeIndex"></param>
        /// <param name="ticket"></param>
        public void AddTicket(int makeIndex, Ticket ticket)
        {
            if (_blocks?.Count > 0)
            {
                makeIndex = Mathf.Min(_blocks.Count - 1, makeIndex);
                _blocks[makeIndex].AddTickets(ticket);
                EditorUtility.SetDirty(this);
                return;
            }
            Debug.LogWarning("ブロックを追加してください");
        }
#endif
    }
}
