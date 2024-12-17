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
            // ï\é¶ÇÃçÇÇ≥ÇêﬂñÒ
            var container = new VisualElement();
            {
                var foldOut = new Foldout();
                {
                    foldOut.text = "ä÷êî";
                    foldOut.value = false;

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
