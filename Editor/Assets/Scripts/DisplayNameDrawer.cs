#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace EasyTalkSystem.Editor
{
    // カスタムプロパティDrawerクラス
    [CustomPropertyDrawer(typeof(DisplayNameAttribute))]
    public class DisplayNameDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var displayNameAttribute = (DisplayNameAttribute)attribute;

            var field = new PropertyField(property, displayNameAttribute.DisplayName);
            field.tooltip = property.displayName;
            field.style.unityFontStyleAndWeight = FontStyle.Bold;


            return field;
        }
    }
}
#endif