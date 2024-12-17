using System.Collections.Generic;
using UnityEngine;

namespace EasyTalkSystem
{
    /// <summary>
    /// 会話時のキャラクターの停止を制御𐧌�
    /// </summary>
    public class TalkCharacterStopManager
    {
        private readonly List<ITalkingOperationManaged> _talkingStopables = new();

        /// <summary>
        /// 制御するキャラクターを設定して生成
        /// </summary>
        /// <param name="talkingStopableInterfaces"></param>
        public TalkCharacterStopManager(List<GameObject> talkingStopableInterfaces)
        {
            ITalkingOperationManaged talkingStopable;

            foreach (var item in talkingStopableInterfaces)
            {
                if (item.TryGetComponent(out talkingStopable))
                {
                    _talkingStopables.Add(talkingStopable);
                }
            }
        }

        /// <summary>
        /// キャラクターの動作を停止
        /// </summary>
        public void StopCharacterOperation()
        {
            _talkingStopables.ForEach(stopable => stopable?.StopOperation());
        }


        /// <summary>
        /// キャラクターの動作を開始
        /// </summary>
        public void StartCharacterOperation()
        {
            _talkingStopables.ForEach(stopable => stopable?.StartOperation());
        }
    }

    /// <summary>
    /// 会話の管理を行います܂��B
    /// </summary>
    public class TalkManager : MonoBehaviour
    {
        public enum TalkType
        {
            Play = 0,
            Event,
            WaitEvent,
            None,
        }

        [SerializeField]
        private List<GameObject> _talkingStopableInterfaces = new();

        private TalkCharacterStopManager _talkCharacterStopManager = null;

        private static TalkManager _instance;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(this);
                return;
            }
            _instance = this;
            _talkCharacterStopManager = new(_talkingStopableInterfaces);
        }

        /// <summary>
        /// インスタンスを取得します܂�
        /// </summary>
        /// <param name="talkManager"></param>
        /// <returns></returns>
        public static bool TryGetInstance(out TalkManager talkManager)
        {
            talkManager = _instance;

            if (_instance != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// キャラクターの動作を停止
        /// </summary>
        public void StopCharacterOperation() => _talkCharacterStopManager.StopCharacterOperation();

        /// <summary>
        /// キャラクターの動作を開始
        /// </summary>
        public void StartCharacterOperation() => _talkCharacterStopManager.StartCharacterOperation();
    }
}
