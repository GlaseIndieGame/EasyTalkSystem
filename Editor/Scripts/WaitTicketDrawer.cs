#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EasyTalkSystem.Editor
{

    [CustomPropertyDrawer(typeof(WaitTicket))]
    public class WaitTicketDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // •\Ž¦‚ðŠÈŒ‰‚É
            var container = new VisualElement();
            {
                var durationField = new PropertyField(property.FindPropertyRelative("_duration"));

                container.Add(durationField);
            }

            return container;
        }
    }
}
#endif
