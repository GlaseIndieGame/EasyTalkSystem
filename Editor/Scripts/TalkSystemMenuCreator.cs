#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace EasyTalkSystem.Editor
{
    public class TalkSystemMenuCreator : UnityEditor.Editor
    {
        [MenuItem("Tools/EasyTalkSystem/CreateTalkSystem")]
        private static void CreateTalkSystem()
        {
            Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Packages/com.glase.easytalksystem/Editor/Assets/Prefabs/BlockPlayer.prefab"));
        }
    }
}
#endif
