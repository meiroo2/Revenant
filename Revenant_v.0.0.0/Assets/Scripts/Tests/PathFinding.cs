using System;
using System.Data;
using Unity.Jobs;
using UnityEngine;
using Unity.Collections; // NativeArray
using UnityEngine.Jobs;  // IJobParallelForTransform
using Unity.Burst;

[BurstCompile]
public class PathFinding : MonoBehaviour
{
    struct JobSingle : IJob
    {
        public int a;
        public int b;
        public NativeArray<int> result;
        
        public void Execute()
        {
            result[0] = a + b;
        }
    }

    private void Start()
    {
        JobSingle single = new JobSingle();
        single.a = 1;
        single.b = 2;
        single.result = new NativeArray<int>(1, Allocator.TempJob);

        JobHandle handle = single.Schedule();
        
        handle.Complete();
        
        Debug.LogWarning("result : " + single.result[0]);
        single.result.Dispose();
    }
}