using UnityEngine;
using UnityEngine.UIElements;
#if UNITY_EDITOR
using UnityEditor.UIElements;
using UnityEditor;
#endif
namespace EasyTalkSystem.Editor
{
#if UNITY_EDITOR
    /// <summary>
    /// 会話システムエディター
    /// </summary>
    [CustomEditor(typeof(BlockPlayer))]
    public class BlockPlayerEditor : UnityEditor.Editor
    {
        [SerializeField]
        private TalkFontSetting _defaultTalkSetting;

        readonly Color CREATE_BUTTON_COLOR = new(0, 1, 0, 0.75f);
        readonly Color DELETE_BUTTON_COLOR = new(1, 0, 0, 0.75f);


        private int _makeIndex = 0;

        public override VisualElement CreateInspectorGUI()
        {
            var instance = target as BlockPlayer;

            // 新しいチケットをenumの数値で生成できるようにするために定義
            System.Func<Ticket>[] tickets =
            {
                ()=>new TalkTicket(_defaultTalkSetting), // editor用初期化
                ()=>new PostTalkTicket(),
                ()=>new EventTicket(),
                ()=>new WaitTicket(),
                ()=>new ChooseTicket(instance), // editor用初期化
                ()=>new WaitClickTicket(),
                ()=>new CallTicket(instance), // editor用初期化
                ()=>new WaitEventTicket(),
            };

            var container = new VisualElement();
            {
                container.viewDataKey = "player-container";
                var listContainer = new VisualElement();
                {
                    listContainer.viewDataKey = "player-listcontainer";
                    var list = new ListView();
                    {
                        list.viewDataKey = "player-list";
                        list.BindProperty(serializedObject.FindProperty("_blocks"));

                        list.bindItem += (VisualElement elem, int index) =>
                        {
                            if (elem is PropertyField propertyField)
                            {
                                propertyField.BindProperty(serializedObject.FindProperty("_blocks").GetArrayElementAtIndex(index));
                                propertyField.viewDataKey = "block-list" + index;
                            }
                        };

                        // 選択したアイテムを編集できるようにするため
                        list.selectedIndicesChanged += (collection) =>
                        {
                            foreach (var item in collection)
                            {
                                _makeIndex = item;
                            }
                        };

                        list.showBorder = true;
                        list.showAddRemoveFooter = true;
                        list.showFoldoutHeader = true;

                        var listAddButton = list.Q<Button>(ListView.footerAddButtonName);
                        {
                            var listFooterButtonGroup = listAddButton.parent;
                            {
                                listFooterButtonGroup.style.width = Length.Percent(100);
                                listFooterButtonGroup.style.right = -14;
                            }

                            listAddButton.text = "ブロックを作成";
                            listAddButton.style.width = Length.Percent(50);
                            listAddButton.style.fontSize = 12;
                            listAddButton.style.backgroundColor = CREATE_BUTTON_COLOR;
                            listAddButton.clicked += () =>
                            {
                                _makeIndex = instance.Blocks.Count - 1;
                                instance.Blocks.RemoveAt(_makeIndex);
                                instance.Blocks.Add(new());
                                EditorUtility.SetDirty(this);
                            };
                            listAddButton.Bind(new SerializedObject(this));
                        }

                        var listRemoveButton = list.Q<Button>(ListView.footerRemoveButtonName);
                        {
                            listRemoveButton.text = "ブロックを削除";
                            listRemoveButton.style.width = Length.Percent(50);
                            listRemoveButton.style.fontSize = 12;
                            listRemoveButton.style.backgroundColor = DELETE_BUTTON_COLOR;
                        }

                        var headerFoldout = list.Q<Foldout>(ListView.foldoutHeaderUssClassName);
                        headerFoldout.text = "ブロック";

                        var sizeField = list.Q(ListView.arraySizeFieldUssClassName);
                        sizeField.SetEnabled(false);

                        list.focusable = true;
                        list.reorderable = true;
                        list.reorderMode = ListViewReorderMode.Animated;
                        list.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
                    }

                    listContainer.Add(list);
                }

                // チケット追加の動作を横並びでスッキリ&関連付けて表示
                var typeContent = new VisualElement();
                {
                    typeContent.style.flexDirection = FlexDirection.Row;
                    typeContent.style.width = Length.Percent(100);
                    typeContent.style.height = 25;

                    var typelabel = new Label("チケット選択");
                    {
                        typelabel.style.width = 60;
                        typelabel.style.height = 20;
                        typelabel.style.unityTextAlign = TextAnchor.LowerLeft;
                    }

                    // チケットの予想外選択(ベースクラス等選択)を防ぐためenumで選択して追加できるようにする
                    var dataTypeField = new EnumField("", TicketType.TALK);
                    {
                        dataTypeField.tooltip = "選択中ブロックに追加されます";
                        dataTypeField.style.position = Position.Absolute;
                        dataTypeField.style.right = 50;
                        dataTypeField.style.left = 60;
                    }

                    var ticketAddButton = CreateButton(() => instance.AddTicket(_makeIndex, tickets[System.Convert.ToInt32(dataTypeField.value)]()), "追加");
                    {
                        ticketAddButton.style.position = Position.Absolute;
                        ticketAddButton.style.right = 0;
                        ticketAddButton.style.height = 20;
                    }

                    typeContent.Add(typelabel);
                    typeContent.Add(dataTypeField);
                    typeContent.Add(ticketAddButton);
                }


                // 取り扱い説明
                var descriptionBox = new Box();
                {
                    descriptionBox.style.paddingTop = 5;
                    descriptionBox.style.paddingBottom = 5;
                    descriptionBox.style.backgroundColor = new StyleColor(Color.black);
                    descriptionBox.style.borderBottomLeftRadius = 5;
                    descriptionBox.style.borderBottomRightRadius = 5;
                    descriptionBox.style.borderTopLeftRadius = 5;
                    descriptionBox.style.borderTopRightRadius = 5;

                    // タイトルを大きめに表示
                    var title = new Label("～使い方～");
                    {
                        title.style.fontSize = 15;
                        title.style.unityTextAlign = TextAnchor.MiddleCenter;
                        title.style.paddingBottom = 5;
                    }

                    var description = new Label("1.話をまとめるブロックを作成します。\n" +
                        "2.行いたい動作に合わせてチケットを選択し追加します。\n" +
                        "詳しくは以下をクリック");
                    {
                        // テキストボックスサイズに合わせて改行
                        description.style.whiteSpace = WhiteSpace.Normal;
                    }

                    // UIElementsを利用したリンクの表示方法がわからなかったため手動でリンクのような見た目に設定
                    var link = new Button(() => Application.OpenURL(@"https://docs.google.com/document/d/1v12IcpF_LQzm_H0-DDiooYhIsW277osyvy6jNosYuxY/edit?usp=drive_link"));
                    {
                        link.text = "公式ドキュメント";
                        link.style.borderTopColor = Color.clear;
                        link.style.borderLeftColor = Color.clear;
                        link.style.borderRightColor = Color.clear;
                        link.style.borderBottomColor = Color.clear;
                        link.style.backgroundColor = Color.clear;
                        link.style.color = Color.blue;
                        link.style.borderBottomWidth = 1;
                        link.style.borderBottomColor = Color.blue;
                        link.style.width = 80;
                    }

                    descriptionBox.Add(title);
                    descriptionBox.Add(description);
                    descriptionBox.Add(link);
                }

                container.Add(listContainer);
                container.Add(typeContent);
                container.Add(descriptionBox);
            }

            return container;

            // ボタンの設定を簡単に
            static Button CreateButton(System.Action action, string label, StyleLength width = default)
            {
                Button button = new(action);
                button.text = label;
                if (width != default)
                {
                    button.style.width = width;
                }
                return button;
            }
        }
    }
#endif
}