#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

namespace EasyTalkSystem.Editor
{
    public class TalkSystemMenuCreator : UnityEditor.Editor
    {
        [MenuItem("Tools/EasyTalkSystem/CreateTalkSystem")]
        private static void CreateTalkSystem()
        {
            string filePath = GetSourceFilePathForUnity();
            filePath = Path.GetDirectoryName(filePath);
            filePath = Path.GetDirectoryName(filePath + ".png") + @"\Prefabs\BlockPlayer.prefab";
            Debug.Log(filePath);
            Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(filePath));
        }

        /// <summary>
        /// ソースファイルのパスを取得
        /// </summary>
        /// <param name="sourceFilePath">コンパイル時に解釈</param>
        /// <returns>Unityで利用可能な相対パス</returns>
        private static string GetSourceFilePathForUnity([System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "")
        {
            if (sourceFilePath[0] != '.') { return sourceFilePath; }
            sourceFilePath = sourceFilePath[23..];  // ".\Library\PackageCache\"を削除
            string packageName = sourceFilePath.Substring(0, sourceFilePath.IndexOf('@'));
            int delimiterIndex = sourceFilePath.IndexOf(@"\"[0]);
            sourceFilePath = sourceFilePath.Substring(delimiterIndex, sourceFilePath.Length - delimiterIndex);
            return @"Packages\" + packageName + sourceFilePath;
        }
    }
}
#endif
