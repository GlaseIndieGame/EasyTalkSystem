#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;

namespace EasyTalkSystem.Editor
{
    [CustomPropertyDrawer(typeof(TalkTicket))]
    public class TalkTicketDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            Foldout container = new();
            {
                container.text = "会話";
                container.value = false;

                TextField textField = new("");
                {
                    textField.BindProperty(property.FindPropertyRelative("_text"));
                    textField.style.maxHeight = 65;
                    textField.multiline = true;
                    textField.SetVerticalScrollerVisibility(ScrollerVisibility.Auto);
                }
                VisualElement durationContainer = new();
                {
                    durationContainer.style.flexDirection = FlexDirection.Row;

                    VisualElement durationField = new();
                    {
                        durationField.style.marginLeft = 3;
                        durationField.style.flexDirection = FlexDirection.Row;
                        durationField.style.width = Length.Percent(100);

                        Label label = new("持続時間");
                        {
                            label.style.unityTextAlign = TextAnchor.MiddleCenter;
                        }
                        PropertyField field = new(property.FindPropertyRelative("_duration"), "");
                        {
                            field.style.width = Length.Percent(100);
                        }

                        durationField.Add(label);
                        durationField.Add(field);
                    }

                    Label autoTitle = new("自動で決定");
                    {
                        autoTitle.style.marginLeft = 2;
                        autoTitle.style.unityTextAlign = TextAnchor.MiddleCenter;
                    }
                    Toggle autoToggle = new("");
                    {
                        autoToggle.BindProperty(property.FindPropertyRelative("_isAutoDuration"));
                        durationField.SetEnabled(!autoToggle.value);
                        autoToggle.RegisterValueChangedCallback(changeEvent =>
                        {
                            durationField.SetEnabled(!changeEvent.newValue);
                        });
                    }

                    durationContainer.Add(autoTitle);
                    durationContainer.Add(autoToggle);
                    durationContainer.Add(durationField);
                }
                VisualElement waitForClickField = new();
                {
                    waitForClickField.style.flexDirection = FlexDirection.Row;

                    Label label = new("クリック待機");
                    {
                        label.style.marginLeft = 2;
                        label.style.unityTextAlign = TextAnchor.MiddleCenter;
                    }
                    Toggle field = new();
                    field.BindProperty(property.FindPropertyRelative("_isWaitForClick"));
                    durationContainer.style.display = GetDisplayStyle(!field.value);
                    field.RegisterValueChangedCallback(changeEvent =>
                    {
                        durationContainer.style.display = GetDisplayStyle(!changeEvent.newValue);
                    });

                    waitForClickField.Add(label);
                    waitForClickField.Add(field);
                }

                container.Add(new Label("セリフ"));
                container.Add(textField);
                container.Add(waitForClickField);
                container.Add(durationContainer);
                container.Add(new PropertyField(property.FindPropertyRelative("_isDisableDialog"), "終了後ダイアログ非表示"));
                container.Add(new PropertyField(property.FindPropertyRelative("_typingIntervalFrame"), "文字送り間隔"));
                container.Add(new PropertyField(property.FindPropertyRelative("_isFollowCharacter"), "キャラクターを追う"));
                container.Add(new PropertyField(property.FindPropertyRelative("_fontSetting"), "フォント設定"));
                container.Add(new PropertyField(property.FindPropertyRelative("_character"), "話者"));
            }
            return container;

            DisplayStyle GetDisplayStyle(bool isVisible) => isVisible ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
#endif
