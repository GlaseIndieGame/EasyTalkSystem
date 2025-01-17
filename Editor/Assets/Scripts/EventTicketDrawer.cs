#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EasyTalkSystem.Editor
{
    [CustomPropertyDrawer(typeof(EventTicket))]
    public class EventTicketDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // 表示の高さを節約
            var container = new VisualElement();
            {
                container.viewDataKey = "event-container";
                var foldOut = new Foldout();
                {
                    foldOut.viewDataKey = "event-foldout";
                    foldOut.text = "関数";

                    var action = new PropertyField(property.FindPropertyRelative("_action"), "Action");
                    {
                        action.style.marginTop = -17.5f;
                        action.style.marginLeft = -17.5f;
                    }

                    foldOut.Add(action);
                }

                container.Add(foldOut);
            }

            return container;
        }
    }
}
#endif
