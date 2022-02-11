using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.Math;
using UnityEngine;
using UnityEngine.UIElements;

//鱼类的随机词条
public struct FishEntry
{
    public float fishLength;
    public bool isFlashing;
    public bool isRainbow;

    public FishEntry(float length, bool flash, bool rainbow)
    {
        this.fishLength = length;
        this.isFlashing = flash;
        this.isRainbow = rainbow;
    }
}

public enum EFishSpecies
{
    PatagonianToothFish,//南极鳕鱼
    Euphausia,          //磷虾
    Dissostichus,       //南极犬牙鱼
    SoraingFish,        //腾鱼
    Notothenioidei,     //南极鱼 
    Acipenser,          //鲟鱼
    LionFush,           //狮子鱼
    Scorpionfish,       //鲉鱼
    Null
}

//鱼的种类及其信息表(供调用查询)
public class FishAttribute
{
    public EFishSpecies fishSpecies;
    public float baseLength;
    public float baseValue;
    public float speed;

    
    public FishAttribute(EFishSpecies species, float value, float length, float speed)
    {
        fishSpecies = species;
        baseValue = value;
        baseLength = length;
        this.speed = speed;
    }
}

public static class FishSheet
{
    public static Dictionary<EFishSpecies, FishAttribute> FishSpecies;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    public static void InitFishSheet()
    {
        FishAttribute APatagonianToothFish = new FishAttribute(EFishSpecies.PatagonianToothFish, 10, 1, 1);
        FishAttribute ASoraingFish = new FishAttribute(EFishSpecies.SoraingFish, 12, 1, 1);
        FishAttribute AEuphausia = new FishAttribute(EFishSpecies.Euphausia, 5, 0.3f, 0.5f);
        FishAttribute ADissostichus = new FishAttribute(EFishSpecies.Dissostichus, 12, 1.2f, 1.3f);
        FishAttribute ANotothenioidei = new FishAttribute(EFishSpecies.Notothenioidei, 14, 1.2f, 1.5f);
        FishAttribute AAcipenser = new FishAttribute(EFishSpecies.Acipenser, 17, 1.2f, 1.5f);
        FishAttribute ALionFush = new FishAttribute(EFishSpecies.LionFush, 13, 1.4f, 1.2f);
        FishAttribute AScorpionfish = new FishAttribute(EFishSpecies.Scorpionfish, 18, 0.5f, 1.1f);

        FishSpecies = new Dictionary<EFishSpecies, FishAttribute>();
        FishSpecies.Add(EFishSpecies.PatagonianToothFish,APatagonianToothFish);
        FishSpecies.Add(EFishSpecies.SoraingFish,ASoraingFish);
        FishSpecies.Add(EFishSpecies.Euphausia, AEuphausia);
        FishSpecies.Add(EFishSpecies.Dissostichus, ADissostichus);
        FishSpecies.Add(EFishSpecies.Notothenioidei,ANotothenioidei);
        FishSpecies.Add(EFishSpecies.Acipenser, AAcipenser);
        FishSpecies.Add(EFishSpecies.LionFush, ALionFush);
        FishSpecies.Add(EFishSpecies.Scorpionfish, AScorpionfish);
        Debug.Log("InitSheet");
    }
}

public class Fish : MonoBehaviour
{
    public EFishSpecies fishSpecies;
    public FishEntry fishEntry;

    private void Awake()
    {
        RandomEntry();
    }

    public int CalculateTotalValue()
    {
        float normalValue = FishSheet.FishSpecies[fishSpecies].baseValue * fishEntry.fishLength;
        if (fishEntry.isFlashing && !fishEntry.isRainbow)
            return (int) Mathf.Floor(normalValue * 2f);
        if (!fishEntry.isFlashing && fishEntry.isRainbow)
            return (int) Mathf.Floor(normalValue * 3f);
        if (fishEntry.isFlashing && fishEntry.isRainbow)
            return (int) Mathf.Floor(normalValue * 6f);
        return (int)Mathf.Floor(normalValue);
    }

    private void RandomEntry()
    {
        fishEntry = RandomUtils.RandomFishEntry(fishSpecies);
    }
}


