using TMPro;
using UnityEngine;

namespace EasyTalkSystem
{
    [CreateAssetMenu(fileName = "TalkFontSetting", menuName = "ScriptableObjects/TalkFontSetting")]
    public class TalkFontSetting : ScriptableObject
    {
        public const float DEFAULT_FONT_SIZE = 10;
        [Header("フォントサイズ10の時の幅と高さを入力してください")]
        public TMP_FontAsset TMP_FontAsset;
        public float FontWidth = 0.5f;
        public float FontHeight = 0.5f;
        public float FontSize = 2.5f;
        public float TextSpacingSpeed = 0.25f;
    }
}