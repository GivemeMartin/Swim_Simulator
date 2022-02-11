using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class RandomUtils 
{
    //输入一个0~1之间的概率，返回一个bool值
    public static bool RandomBool(float posibility)
    {
        var tmp = Random.value;
        return tmp < posibility;
    }

    public static EWeatherState RandomWeather(ESeasonState season)
    {
        //从低数字往高数字排：Sunny,Snowy,Cloudy,Coldy,Storm,Aurora,Rainy
        var k = Random.value;
        float tmp = 0;
        switch (season)
        {
            case ESeasonState.Warm :
                if (k > 0 && k <= WeatherStateMachine.WeatherChances[EWeatherState.Sunny].warmChance)
                {
                    return EWeatherState.Sunny;
                }
                tmp += WeatherStateMachine.WeatherChances[EWeatherState.Sunny].warmChance;
                
                if(k > tmp && k < tmp + WeatherStateMachine.WeatherChances[EWeatherState.Snowy].warmChance)
                {
                    return EWeatherState.Snowy;
                }
                tmp += WeatherStateMachine.WeatherChances[EWeatherState.Snowy].warmChance;
                
                if(k > tmp && k <= tmp + WeatherStateMachine.WeatherChances[EWeatherState.Cloudy].warmChance)
                {
                    return EWeatherState.Cloudy;
                }
                tmp += WeatherStateMachine.WeatherChances[EWeatherState.Cloudy].warmChance;
                
                if(k > tmp && k <= tmp + WeatherStateMachine.WeatherChances[EWeatherState.Coldy].warmChance)
                {
                    return EWeatherState.Coldy;
                }
                tmp += WeatherStateMachine.WeatherChances[EWeatherState.Coldy].warmChance;
                
                if(k > tmp && k <= tmp + WeatherStateMachine.WeatherChances[EWeatherState.Storm].warmChance)
                {
                    return EWeatherState.Storm;
                }
                tmp += WeatherStateMachine.WeatherChances[EWeatherState.Storm].warmChance;
                
                if(k > tmp && k <= tmp + WeatherStateMachine.WeatherChances[EWeatherState.Aurora].warmChance)
                {
                    return EWeatherState.Aurora;
                }
                tmp += WeatherStateMachine.WeatherChances[EWeatherState.Aurora].warmChance;
                
                if(k > tmp && k <= tmp + WeatherStateMachine.WeatherChances[EWeatherState.Rainy].warmChance)
                {
                    return EWeatherState.Rainy;
                }
                break;
            
            case ESeasonState.Cold :
                if (k > 0 && k <= WeatherStateMachine.WeatherChances[EWeatherState.Sunny].coldChance)
                {
                    return EWeatherState.Sunny;
                }
                tmp += WeatherStateMachine.WeatherChances[EWeatherState.Sunny].coldChance;
                
                if(k > tmp && k < tmp + WeatherStateMachine.WeatherChances[EWeatherState.Snowy].coldChance)
                {
                    return EWeatherState.Snowy;
                }
                tmp += tmp + WeatherStateMachine.WeatherChances[EWeatherState.Snowy].coldChance;
                
                if(k > tmp && k <= tmp + WeatherStateMachine.WeatherChances[EWeatherState.Cloudy].coldChance)
                {
                    return EWeatherState.Cloudy;
                }
                tmp += WeatherStateMachine.WeatherChances[EWeatherState.Cloudy].coldChance;
                
                if(k > tmp && k <= tmp + WeatherStateMachine.WeatherChances[EWeatherState.Coldy].coldChance)
                {
                    return EWeatherState.Coldy;
                }
                tmp += WeatherStateMachine.WeatherChances[EWeatherState.Coldy].coldChance;
                
                if(k > tmp && k <= tmp + WeatherStateMachine.WeatherChances[EWeatherState.Storm].coldChance)
                {
                    return EWeatherState.Storm;
                }
                tmp += WeatherStateMachine.WeatherChances[EWeatherState.Storm].coldChance;
                
                if(k > tmp && k <= tmp + WeatherStateMachine.WeatherChances[EWeatherState.Aurora].coldChance)
                {
                    return EWeatherState.Aurora;
                }
                tmp += WeatherStateMachine.WeatherChances[EWeatherState.Aurora].coldChance;
                
                if(k > tmp && k <= tmp + WeatherStateMachine.WeatherChances[EWeatherState.Rainy].coldChance)
                {
                    return EWeatherState.Rainy;
                }
                break;
        }
        
        return EWeatherState.Null;
    }
}
