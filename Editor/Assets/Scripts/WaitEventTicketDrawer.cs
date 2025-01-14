#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EasyTalkSystem.Editor
{
    [CustomPropertyDrawer(typeof(WaitEventTicket))]
    public class WaitEventTicketDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // �G�f�B�^�[�̍��������Ȃ����邽�ߕ\�����l�߂�
            var container = new VisualElement();
            {
                var foldOut = new Foldout();
                {
                    foldOut.text = "�֐��ҋ@";
                    foldOut.value = false;

                    var action = new PropertyField(property.FindPropertyRelative("_waitAction"), "WaitAction");
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
