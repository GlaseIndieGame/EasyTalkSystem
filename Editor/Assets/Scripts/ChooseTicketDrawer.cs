#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EasyTalkSystem.Editor
{
    [CustomPropertyDrawer(typeof(ChooseTicket))]
    public class ChooseTicketDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // �I�����ݒ胊�X�g�̕\�����Ȍ���
            var container = new VisualElement();
            {
                var foldout = new Foldout();
                {
                    foldout.text = "�I����";
                    foldout.value = false;

                    var inspector = new VisualElement();
                    {
                        inspector.style.marginLeft = -25;

                        var title = new Label("�\���I����=>�J�ڐ�u���b�N��");
                        var selectList = new ListView();
                        {
                            selectList.showAddRemoveFooter = true;
                            selectList.showBorder = true;
                            selectList.focusable = true;
                            selectList.reorderable = true;
                            selectList.showBoundCollectionSize = false;
                            selectList.reorderMode = ListViewReorderMode.Animated;
                            selectList.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
                            selectList.BindProperty(property.FindPropertyRelative("_selectSettings"));
                        }
                        var talkSelectDialog = new PropertyField(property.FindPropertyRelative("_talkSelectDialog"));
                        var useBlockPlayer = new PropertyField(property.FindPropertyRelative("_useBlockPlayer"));

                        inspector.Add(title);
                        inspector.Add(selectList);
                        inspector.Add(talkSelectDialog);
                        inspector.Add(useBlockPlayer);
                    }

                    foldout.Add(inspector);
                }

                container.Add(foldout);
            }

            return container;
        }
    }
}
#endif
