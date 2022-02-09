using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWeatherState
{
    Sunny,
    Windy,
    Rainy,
    Snowy,
    Null
}

public enum ESeasonState
{
    Warm,
    Cold,
    Null
}

public class WeatherStateAttribute : Attribute
{
    public EWeatherState weatherState;

    public WeatherStateAttribute(EWeatherState state)
    {
        this.weatherState = state;
    }
}

public class SeasonStateAttribute : Attribute
{
    public ESeasonState seasonState;

    public SeasonStateAttribute(ESeasonState state)
    {
        this.seasonState = state;
    }
}

public abstract class StateMachineBase : MonoBehaviour
{
    protected StateBase currentState;
    private void LateUpdate()
    {
        currentState.OnUpdate();
    }
    
}


