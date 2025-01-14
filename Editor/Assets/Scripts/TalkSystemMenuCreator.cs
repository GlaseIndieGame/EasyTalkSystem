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
        /// �\�[�X�t�@�C���̃p�X���擾
        /// </summary>
        /// <param name="sourceFilePath">�R���p�C�����ɉ���</param>
        /// <returns>Unity�ŗ��p�\�ȑ��΃p�X</returns>
        private static string GetSourceFilePathForUnity([System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "")
        {
            if (sourceFilePath[0] != '.') { return sourceFilePath; }
            sourceFilePath = sourceFilePath[23..];  // ".\Library\PackageCache\"���폜
            string packageName = sourceFilePath.Substring(0, sourceFilePath.IndexOf('@'));
            int delimiterIndex = sourceFilePath.IndexOf(@"\"[0]);
            sourceFilePath = sourceFilePath.Substring(delimiterIndex, sourceFilePath.Length - delimiterIndex);
            return @"Packages\" + packageName + sourceFilePath;
        }
    }
}
#endif
