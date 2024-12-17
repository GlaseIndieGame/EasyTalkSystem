using UnityEngine;

namespace EasyTalkSystem
{
    public class TalkCharacter : MonoBehaviour
    {
        [SerializeField]
        private float _displayHeight = 1.0f;

        [SerializeField]
        private TalkDialog _dialog;

        public float DisplayHeight => _displayHeight;

        public TalkDialog Dialog => _dialog;
    }
}
