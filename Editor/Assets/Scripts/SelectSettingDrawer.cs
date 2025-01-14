#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EasyTalkSystem.Editor
{
    [CustomPropertyDrawer(typeof(SelectSetting))]
    public class SelectSettingDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // •\Ž¦‚ðˆês‚É‚Ü‚Æ‚ß‚é
            var container = new VisualElement();
            {
                container.style.flexDirection = FlexDirection.Row;

                var labelField = new TextField();
                {
                    labelField.BindProperty(property.FindPropertyRelative("_label"));
                    labelField.style.width = Length.Percent(45);
                    labelField.style.right = 3;
                }
                var label = new Label("=>");
                {
                    label.style.width = Length.Percent(10);
                    label.style.unityTextAlign = TextAnchor.MiddleCenter;
                }
                var blockNameField = new TextField();
                {
                    blockNameField.BindProperty(property.FindPropertyRelative("_blockName"));
                    blockNameField.style.width = Length.Percent(45);
                    blockNameField.style.right = 3;
                }

                container.Add(labelField);
                container.Add(label);
                container.Add(blockNameField);
            }

            return container;
        }
    }
}
#endif
