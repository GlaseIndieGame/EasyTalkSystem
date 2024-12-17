#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif
using UnityEngine.UIElements;
using UnityEngine;

namespace EasyTalkSystem.Editor
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(TicketBlock))]
    public class TicketBlockDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // チケットの予期せぬ追加を防止するため
            var container = new VisualElement();
            {
                var talkBlockField = new ListView();
                {
                    talkBlockField.showAddRemoveFooter = true;
                    talkBlockField.showFoldoutHeader = true;
                    talkBlockField.showBorder = true;
                    talkBlockField.showBoundCollectionSize = false;
                    talkBlockField.focusable = true;
                    talkBlockField.reorderable = true;
                    talkBlockField.reorderMode = ListViewReorderMode.Animated;
                    talkBlockField.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
                    talkBlockField.BindProperty(property.FindPropertyRelative("_talkBlock"));

                    var addButton = talkBlockField.Q<Button>(ListView.footerAddButtonName);
                    {
                        var footerButtonGroup = addButton.parent;
                        {
                            footerButtonGroup.style.position = Position.Absolute;
                            footerButtonGroup.style.top = 0;
                            footerButtonGroup.style.left = -25;
                            footerButtonGroup.style.width = 25;
                            footerButtonGroup.style.height = 28;
                            footerButtonGroup.style.borderTopWidth = 1;
                            footerButtonGroup.style.borderTopLeftRadius = 3;
                            footerButtonGroup.style.borderTopRightRadius = 3;
                        }
                        addButton.style.display = DisplayStyle.None;
                    }
                }

                var nameContent = new VisualElement();
                {
                    nameContent.style.position = Position.Absolute;
                    nameContent.style.top = 0;
                    nameContent.style.right = 2;
                    nameContent.style.left = 10;

                    var nameField = new TextField();
                    {
                        nameField.BindProperty(property.FindPropertyRelative("_name"));
                        nameField.label = "";
                        nameField.style.position = Position.Absolute;
                        nameField.style.width = Length.Percent(100);
                        nameField.style.minWidth = 80;
                        nameField.style.right = 0;
                    }

                    nameContent.Add(nameField);
                }

                container.Add(talkBlockField);
                container.Add(nameContent);
            }

            return container;
        }
    }
#endif
}
