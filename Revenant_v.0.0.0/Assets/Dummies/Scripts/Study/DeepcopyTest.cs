using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MyCopy : MonoBehaviour
{
    public int id = 0;

    public MyCopy(int _val)
    {
        id = _val;
    }
}
public class DeepcopyTest : MonoBehaviour
{
    public List<MyCopy> copyList = new List<MyCopy>();
    
    void Start()
    {
        MyCopy copy = new MyCopy(0);
        copyList.Add(copy);
        PrintMyCopy(copyList);

        Debug.Log("이제 1로 바꾼 후 삽입");
        copy.id = 1;
        copyList.Add(copy);
        PrintMyCopy(copyList);
    }

    void PrintMyCopy(List<MyCopy> _val)
    {
        foreach (var VARIABLE in _val)
        {
            Debug.Log(VARIABLE.id + " 출력");
        }
    }
}
