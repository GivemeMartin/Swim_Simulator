using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

[Serializable]
public class weatherChance
{
    public EWeatherState weather;
    [Range(0, 1)] public float warmChance = 0;
    [Range(0, 1)] public float coldChance = 0;
    public weatherChance(EWeatherState state, float warmProbability,float coldProbability)
    {
        this.weather = state;
        if(warmProbability < 1.0f && warmProbability > 0f)
            this.warmChance = warmProbability;
        if(coldProbability < 1.0f && coldProbability > 0f)
            this.coldChance = coldProbability;
    }
}

public class WeatherStateMachine : StateMachineBase
{
    public static WeatherStateMachine Instance;
    private Dictionary<EWeatherState, StateBase> weatherStates;
    private const EWeatherState IniState = EWeatherState.Sunny;
    public EWeatherState CurrentWeather;
    public EWeatherState NextWeather;
    
    //供调用查询的天气概率词典
    public static Dictionary<EWeatherState, weatherChance> WeatherChances;
    //Inspector修改天气概率
    [SerializeField] private List<weatherChance> weatherChances = new List<weatherChance>();
    //季节控制天气出现的概率

    private void Awake()
    {
        ConstructStates();
        Instance = this;
        SetWeatherState(EWeatherState.Sunny);
        NextWeather = EWeatherState.Sunny;
    }
    private void Update()
    {
        //Debug.Log(WeatherChances[EWeatherState.Sunny].warmChance);
    }


    public void SetWeatherState(EWeatherState state)
    {
        if (GetWeatherState(state) == null)
        {
            Debug.LogError("NULL REFERENCE: Missing State (" + state.ToString() + " )");
        }
        else
        {
            CurrentWeather = state;
            currentState?.ExitState();
            var nextState = GetWeatherState(state);
            nextState.EnterState();
            currentState = nextState;
        }
    }

    public StateBase GetWeatherState(EWeatherState state)
    {
        if (weatherStates.ContainsKey(state))
        {
            return weatherStates[state];
        }
        else
        {
            Debug.LogError("Required State not exist: " + state);
            return null;
        }
    }

    public void UpdateWeather()
    {
        SetWeatherState(NextWeather);
        NextWeather = RandomUtils.RandomWeather(SeasonStateMachine.CurrentState);
    }

    private void ConstructDictionary()
    {
        WeatherChances = new Dictionary<EWeatherState, weatherChance>();
        foreach (var weatherValue in weatherChances)
        {
            if(!WeatherChances.ContainsKey(weatherValue.weather))
                WeatherChances.Add(weatherValue.weather,weatherValue);
        }
    }

    private void ConstructStates()
    {
        weatherStates = new Dictionary<EWeatherState, StateBase>();
        // weatherChances.Clear();
        var stateEnums = Enum.GetValues(typeof(ESeasonState)) as ESeasonState[];

        var occuredTable = new Dictionary<EWeatherState, bool>();

        var allTypes = Assembly.GetExecutingAssembly().GetTypes();

        var attributedType =
            from type in allTypes
            where
                !type.IsAbstract &&
                !type.IsInterface &&
                type.IsSubclassOf(typeof(StateBase)) &&
                type.GetCustomAttribute<WeatherStateAttribute>() != null
            select type;

        foreach (var type in attributedType)
        {
            var attribute = type.GetCustomAttribute<WeatherStateAttribute>();
            var state = attribute.weatherState;
            if (!occuredTable.ContainsKey(state))
            {
                occuredTable[state] = true;
                weatherStates[state] =
                    type.GetConstructor(new Type[] {typeof(StateMachineBase)}).Invoke(new object[] {this}) as StateBase;
                bool isOccuredinList = false;
                foreach (var weatherChance in weatherChances)
                {
                    if (weatherChance.weather == state)
                        isOccuredinList = true;
                }
                if(!isOccuredinList)
                    weatherChances.Add(new weatherChance(state,0,0));
            }
            else
            {
                Debug.Log("Repeated definition : " + type.Name);
            }
        }
        ConstructDictionary();
    }
    
#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(WeatherStateMachine))]
    public class ViveInputAdapterManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!Application.isPlaying)
            {
                var targetNode = target as WeatherStateMachine;
                if (targetNode == null)
                {
                    return;
                }
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Construct States"))
                {
                    targetNode.ConstructStates();
                }
                GUILayout.EndHorizontal();
            }
        }
    }
#endif
}

[WeatherState(EWeatherState.Sunny)]
public class SunnyState : StateBase
{
    public SunnyState(StateMachineBase sm) : base(sm) {}
    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}

[WeatherState(EWeatherState.Snowy)]
 public class SnowyState : StateBase
 {
     public SnowyState(StateMachineBase sm) : base(sm) {}
     public override void EnterState()
     {
         
     }
 
     public override void ExitState()
     {
         
     }
 
     public override void OnUpdate()
     {
         base.OnUpdate();
     }
 }
 
[WeatherState(EWeatherState.Rainy)]
public class RainyState : StateBase
{
    public RainyState(StateMachineBase sm) : base(sm) {}
    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}

[WeatherState(EWeatherState.Cloudy)]
public class CloudyState : StateBase
{
    public CloudyState(StateMachineBase sm) : base(sm) {}
    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}

[WeatherState(EWeatherState.Coldy)]
public class ColdyState : StateBase
{
    public ColdyState(StateMachineBase sm) : base(sm) {}
    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}

[WeatherState(EWeatherState.Storm)]
public class StormState : StateBase
{
    public StormState(StateMachineBase sm) : base(sm) {}
    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}

[WeatherState(EWeatherState.Aurora)]
public class AuroraState : StateBase
{
    public AuroraState(StateMachineBase sm) : base(sm) {}
    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }
}