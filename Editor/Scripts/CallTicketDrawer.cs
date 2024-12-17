#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EasyTalkSystem.Editor
{
    [CustomPropertyDrawer(typeof(CallTicket))]
    public class CallTicketDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // 一行で表示をまとめる
            var container = new VisualElement();
            {
                container.style.flexDirection = FlexDirection.Row;

                var title = new Label("呼び出し");
                {
                    title.style.width = Length.Percent(15);
                    title.style.unityTextAlign = TextAnchor.MiddleLeft;
                }
                var useBlockPlayer = new PropertyField(property.FindPropertyRelative("_useBlockPlayer"), "");
                {
                    useBlockPlayer.style.width = Length.Percent(40);
                }
                var label = new Label("の");
                {
                    label.style.width = Length.Percent(5);
                    label.style.unityTextAlign = TextAnchor.MiddleCenter;
                }
                var callBlock = new PropertyField(property.FindPropertyRelative("_callBlock"), "");
                {
                    callBlock.style.width = Length.Percent(40);
                }
                container.Add(title);
                container.Add(useBlockPlayer);
                container.Add(label);
                container.Add(callBlock);
            }

            return container;
        }
    }
}
#endif
