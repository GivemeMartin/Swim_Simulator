using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.Math;
using UnityEngine;

public class RandomTest : MonoBehaviour
{
    public void RandomTestButton()
    {
        var tmp = RandomUtils.RandomWeather(ESeasonState.Warm);
        Debug.Log(tmp);
    }
}
