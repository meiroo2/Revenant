using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

namespace Editor
{
    public class OdinEditorTest : OdinMenuEditorWindow
    {
        [MenuItem("Tools/TestOdinData")]
        static void OpenWindow()
        {
            GetWindow<OdinEditorTest>().Show();
        }
        
        // protected override OdinMenuTree BuildMenuTree()
        // {
        //     var tree = new OdinMenuTree();
        //     
        //     tree.AddAllAssetsAtPath("TestOdinData", "Assets/Scripts", typeof())
        // }
        protected override OdinMenuTree BuildMenuTree()
        {
            throw new System.NotImplementedException();
        }
    }
}

