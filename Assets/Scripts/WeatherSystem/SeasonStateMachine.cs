using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

public class SeasonStateMachine : StateMachineBase
{
    public static SeasonStateMachine Instance;
    private Dictionary<ESeasonState, StateBase> seasonStates;
    private const ESeasonState initState = ESeasonState.Warm;
    [SerializeField] private ESeasonState CurrentState;
    //季节控制天气出现的概率

    private void Awake()
    {
        ConstructStates();
        SetSeasonState(initState);
        Instance = this;
    }

    public void SetSeasonState(ESeasonState state)
    {
        if (GetSeasonState(state) == null)
        {
            Debug.LogError("NULL REFERENCE: Missing State (" + state.ToString() + " )");
        }
        else
        {
            CurrentState = state;
            currentState?.ExitState();
            var nextState = GetSeasonState(state);
            nextState.EnterState();
            currentState = nextState;
        }
    }

    public StateBase GetSeasonState(ESeasonState state)
    {
        if (seasonStates.ContainsKey(state))
        {
            return seasonStates[state];
        }
        else
        {
            Debug.LogError("Required State not exist: " + state);
            return null;
        }
    }
    
    private void ConstructStates ()
    {
        seasonStates = new Dictionary<ESeasonState, StateBase>();

        var stateEnums = Enum.GetValues(typeof(ESeasonState)) as ESeasonState[];

        var occuredTable = new Dictionary<ESeasonState, bool>();

        var allTypes = Assembly.GetExecutingAssembly().GetTypes();

        var attributedType =
            from type in allTypes
            where
                !type.IsAbstract &&
                !type.IsInterface &&
                type.IsSubclassOf(typeof(StateBase)) &&
                type.GetCustomAttribute<SeasonStateAttribute>() != null
            select type;

        foreach (var type in attributedType)
        {
            var attribute = type.GetCustomAttribute<SeasonStateAttribute>();
            var state = attribute.seasonState;
            if (!occuredTable.ContainsKey(state))
            {
                occuredTable[state] = true;
                seasonStates[state] = type.GetConstructor(new Type[] { typeof(StateMachineBase) }).Invoke(new object[] { this }) as StateBase;
            }
            else
            {
                Debug.Log("Repeated definition : " + type.Name);
            }

        }
        
    }
    
// #if UNITY_EDITOR
//     [UnityEditor.CustomEditor(typeof(SeasonStateMachine))]
//     public class SeasonStateEditor : UnityEditor.Editor
//     {
//         public override void OnInspectorGUI()
//         {
//             base.OnInspectorGUI();
//             if (!Application.isPlaying)
//             {
//                 var targetNode = target as SeasonStateMachine;
//                 if (targetNode == null)
//                 {
//                     return;
//                 }
//                 GUILayout.BeginHorizontal();
//                 if (GUILayout.Button("Construct States"))
//                 {
//                     targetNode.ConstructStates();
//                 }
//                 GUILayout.EndHorizontal();
//             }
//         }
//     }
// #endif
}

[SeasonState(ESeasonState.Warm)]
public class WarmState : StateBase
{
    public WarmState(StateMachineBase sm) : base(sm) {}
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
