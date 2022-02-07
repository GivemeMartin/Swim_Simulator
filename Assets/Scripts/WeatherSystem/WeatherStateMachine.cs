using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

[Serializable]
public class weatherChance
{
    public EWeatherState weatherState;
    [Range(0, 1)] public float chance;

    public weatherChance(EWeatherState state, float probability)
    {
        this.weatherState = state;
        this.chance = probability;
    }
}

public class WeatherStateMachine : StateMachineBase
{
    public static WeatherStateMachine Instance;
    private Dictionary<EWeatherState, StateBase> weatherStates;
    private const EWeatherState IniState = EWeatherState.Sunny;
    [SerializeField] private EWeatherState CurrentState;

    [SerializeField] private List<weatherChance> weatherChances = new List<weatherChance>();
    //季节控制天气出现的概率

    private void Awake()
    {
        Instance = this;
        ConstructStates();
        SetWeatherState(EWeatherState.Sunny);
    }

    public void SetWeatherState(EWeatherState state)
    {
        if (GetWeatherState(state) == null)
        {
            Debug.LogError("NULL REFERENCE: Missing State (" + state.ToString() + " )");
        }
        else
        {
            CurrentState = state;
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

    private void ConstructStates()
    {
        weatherStates = new Dictionary<EWeatherState, StateBase>();
        weatherChances.Clear();
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
                weatherChances.Add(new weatherChance(state,0));
            }
            else
            {
                Debug.Log("Repeated definition : " + type.Name);
            }

        }
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