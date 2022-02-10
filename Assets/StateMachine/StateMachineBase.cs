using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------enums----------------------------
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

public enum EGameTime
{
    Dawn,
    Morning,
    Noon,
    Afternoon,
    Night,
    MidNight
}

//-------------------------Attributes----------------------------------
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

public class GameTimeStateAttribute : Attribute
{
    public EGameTime gameTimeState;

    public GameTimeStateAttribute(EGameTime state)
    {
        this.gameTimeState = state;
    }
}

//---------------------------StateMachineBase-------------------------------
public abstract class StateMachineBase : MonoBehaviour
{
    protected StateBase currentState;
    private void LateUpdate()
    {
        currentState.OnUpdate();
    }
    
}


