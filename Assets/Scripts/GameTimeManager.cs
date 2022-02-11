using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BehaviorDesigner.Runtime.ObjectDrawers;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class GameTimeManager : StateMachineBase
{
    public static GameTimeManager Instance;
    public static int GameDate = 1;
    
    private Dictionary<EGameTime, StateBase> timeStates;
    private const EGameTime initState = EGameTime.MidNight;
    public EGameTime CurrentTimeSeg;
    private const float DAYTIME = 300;
    
    //timeK为测试用
    [Tooltip("游戏时间运行速度倍率")]
    [SerializeField] private int timeK = 1;
    [Tooltip("游戏实际时间(每日)")]
    [SerializeField] private float GTime;
    [Tooltip("游戏内时间")]
    [Range(0,24)]public float GameTime;

    private static float gameTime;
    private bool timing;

    [SerializeField] private Light globalLight;
    private void Awake()
    {
        ConstructStates();
        SetTimeState(initState);
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        timing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timing)
        {
            if (GTime <= DAYTIME)
            {
                GTime += Time.deltaTime * timeK;
            }
            else
            {
                WeatherStateMachine.Instance.UpdateWeather();
                GameDate++;
                GTime = 0;
            }
            //同步光照（暂时不知道放在哪，先写这里）
            globalLight.intensity = Mathf.Clamp(Mathf.Abs(Mathf.Sin(gameTime / 24f * Mathf.PI)),0.1f,1f)  ;
        }

        gameTime = GTime * 0.08f;
        GameTime = gameTime;
        SetGameTime();
    }

    public void SetGameTimeEngine(bool isStart)
    {
        timing = isStart;
    }

    private void SetGameTime()
    {
        if (gameTime >= 4 && gameTime < 6)
        {
            SetTimeState(EGameTime.Dawn);
        }
        else if (gameTime >= 6 && gameTime < 12)
        {
            SetTimeState(EGameTime.Morning);
        }
        else if (gameTime >= 12 && gameTime < 14)
        {
            SetTimeState(EGameTime.Noon);
        }
        else if (gameTime >= 14 && gameTime < 16)
        {
            SetTimeState(EGameTime.Afternoon);
        }
        else if (gameTime >= 18 && gameTime < 24)
        {
            SetTimeState(EGameTime.Night);
        }
        else if (gameTime >= 0 && gameTime < 4)
        {
            SetTimeState(EGameTime.MidNight);
        }
    }
    
    private void SetTimeState(EGameTime state)
    {
        if (GetTimeState(state) == null)
        {
            Debug.LogError("NULL REFERENCE: Missing State (" + state.ToString() + " )");
        }
        else
        {
            CurrentTimeSeg = state;
            currentState?.ExitState();
            var nextState = GetTimeState(state);
            nextState.EnterState();
            currentState = nextState;
        }
    }

    private StateBase GetTimeState(EGameTime state)
    {
        if (timeStates.ContainsKey(state))
        {
            return timeStates[state];
        }
        else
        {
            Debug.LogError("Required State not exist: " + state);
            return null;
        }
    }
    
    private void ConstructStates()
    {
        timeStates = new Dictionary<EGameTime, StateBase>();

        var stateEnums = Enum.GetValues(typeof(EGameTime)) as EGameTime[];

        var occuredTable = new Dictionary<EGameTime, bool>();

        var allTypes = Assembly.GetExecutingAssembly().GetTypes();

        var attributedType =
            from type in allTypes
            where
                !type.IsAbstract &&
                !type.IsInterface &&
                type.IsSubclassOf(typeof(StateBase)) &&
                type.GetCustomAttribute<GameTimeStateAttribute>() != null
            select type;

        foreach (var type in attributedType)
        {
            var attribute = type.GetCustomAttribute<GameTimeStateAttribute>();
            var state = attribute.gameTimeState;
            if (!occuredTable.ContainsKey(state))
            {
                occuredTable[state] = true;
                timeStates[state] = type.GetConstructor(new Type[] { typeof(StateMachineBase) }).Invoke(new object[] { this }) as StateBase;
            }
            else
            {
                Debug.Log("Repeated definition : " + type.Name);
            }

        }
        
    }
}

[GameTimeState(EGameTime.Afternoon)]
public class AfternoonState : StateBase
{
    public AfternoonState(StateMachineBase sm) : base(sm)
    {
    }

    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}

[GameTimeState(EGameTime.Dawn)]
public class DawnState : StateBase
{
    public DawnState(StateMachineBase sm) : base(sm)
    {
    }

    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}

[GameTimeState(EGameTime.Morning)]
public class MorningState : StateBase
{
    public MorningState(StateMachineBase sm) : base(sm)
    {
    }

    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}

[GameTimeState(EGameTime.Night)]
public class NightState : StateBase
{
    public NightState(StateMachineBase sm) : base(sm)
    {
    }

    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}

[GameTimeState(EGameTime.Noon)]
public class NoonState : StateBase
{
    public NoonState(StateMachineBase sm) : base(sm)
    {
    }

    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}

[GameTimeState(EGameTime.MidNight)]
public class MidNightState : StateBase
{
    public MidNightState(StateMachineBase sm) : base(sm)
    {
    }

    public override void EnterState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}

