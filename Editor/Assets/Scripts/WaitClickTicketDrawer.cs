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
            // �\�����Ȍ���
            var container = new VisualElement();

            container.Add(new Label("�N���b�N�ҋ@"));

            return container;
        }
    }
}
#endif
