#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UIElements;

namespace EasyTalkSystem.Editor
{
    [CustomPropertyDrawer(typeof(WaitClickTicket))]
    public class WaitClickTicketDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // 表示を簡潔に
            var container = new VisualElement();

            container.Add(new Label("クリック待機"));

            return container;
        }
    }
}
#endif
