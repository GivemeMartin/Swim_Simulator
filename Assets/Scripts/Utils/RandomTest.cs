using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.Math;
using UnityEngine;

public class RandomTest : MonoBehaviour
{
    public void RandomTestButton()
    {
        var tmp = RandomUtils.RandomFishEntry(EFishSpecies.PatagonianToothFish);
        Debug.Log("Length:"+ tmp.fishLength + " flash:" + tmp.isFlashing + " rainbow:" + tmp.isRainbow);
    }
}
