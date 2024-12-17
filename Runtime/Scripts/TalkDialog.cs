using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EasyTalkSystem
{
    public class TalkDialog : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _margine = Vector2.one;

        [SerializeField]
        private float _minWidth = 0.65f;

        [SerializeField]
        private float _tweenSeconds = 0.5f;

        [SerializeField]
        private Image _panel;

        [SerializeField]
        private TextMeshProUGUI _text;

        private RectTransform _textTransform;

        private RectTransform _panelTransform;

        private void Awake()
        {
            _panelTransform = _panel.rectTransform;
            _textTransform = _text.rectTransform;
            _panelTransform.sizeDelta = Vector2.zero;
        }

        private void OnDisable()
        {
            _panelTransform.sizeDelta = Vector2.zero;
        }

        public void SettingDialog(string text, int lines, float maxCharaccter, TalkFontSetting talkFontSetting)
        {
            _text.font = talkFontSetting.TMP_FontAsset;
            _text.fontSize = talkFontSetting.FontSize;
            float ratio = talkFontSetting.FontSize / TalkFontSetting.DEFAULT_FONT_SIZE;
            SettingDialog(text, lines * talkFontSetting.FontHeight * ratio, maxCharaccter * talkFontSetting.FontWidth * ratio);
        }

        public void SettingDialog(string text, float height, float width)
        {
            width = Mathf.Max(width, _minWidth);
            Vector3 size = new(width + _margine.x, height + _margine.y);
            Vector2 panelSize = new(_minWidth, size.y);

            _panelTransform.sizeDelta = panelSize;
            DG.Tweening.DOTween.To(() => panelSize.x, x =>
              {
                  panelSize.x = x;
                  _panelTransform.sizeDelta = panelSize;
              }, size.x, _tweenSeconds);

            _textTransform.sizeDelta = size;
            _text.text = text;
        }

        public string GetText() => _text.text;

        public void SetText(string text) => _text.text = text;

        public float PanelHeight() => _panelTransform.sizeDelta.y;
    }
}
