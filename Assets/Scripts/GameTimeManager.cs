using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.ObjectDrawers;
using UnityEngine;

public enum EGameTime
{
    Dawn,
    Morning,
    Noon,
    Afternoon,
    Night,
    MidNight
}
public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager gameTimeManager;
    private const float DAYTIME = 300;
    public EGameTime EgameTime;
    [SerializeField] private float GTime;

    [Range(0,24)]public int GameTime;

    private static int gameTime;
    private bool timing;
    private void Awake()
    {
        if (gameTimeManager == null)
        {
            gameTimeManager = this;
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
                GTime += Time.deltaTime;
            }
            else
            {
                GTime = 0;
            }
        }

        gameTime = (int)(GTime * 0.08f);
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
            EgameTime = EGameTime.Dawn;
        }
        else if (gameTime >= 6 && gameTime < 12)
        {
            EgameTime = EGameTime.Morning;
        }
        else if (gameTime >= 12 && gameTime < 14)
        {
            EgameTime = EGameTime.Noon;
        }
        else if (gameTime >= 14 && gameTime < 16)
        {
            EgameTime = EGameTime.Afternoon;
        }
        else if (gameTime >= 18 && gameTime < 24)
        {
            EgameTime = EGameTime.Night;
        }
        else if (gameTime >= 0 && gameTime < 4)
        {
            EgameTime = EGameTime.MidNight;
        }
    }
}
