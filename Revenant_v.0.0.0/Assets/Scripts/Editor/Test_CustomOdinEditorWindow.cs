using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

public class Test_CustomOdinEditorWindow : OdinEditorWindow
{
    [MenuItem("My Game/My Window")]
    public static void OpenWindow() => GetWindow<Test_CustomOdinEditorWindow>();

    [TabGroup("Tab A"), ShowInInspector] public static Dictionary<string, string> SomeData;

    //[TabGroup("Tab B"), TableList(ScrollViewHeight = 270)]
    //public List<SomeData> MyData;
}
