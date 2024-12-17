using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EasyTalkSystem
{
    [RequireComponent(typeof(Button))]
    public class TalkButton : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _label;

        [SerializeField]
        private Button _button;

        public Button.ButtonClickedEvent onClick
        {
            get => _button.onClick;
        }

        public void SetLabel(string text)
        {
            _label.text = text;
        }
    }
}
