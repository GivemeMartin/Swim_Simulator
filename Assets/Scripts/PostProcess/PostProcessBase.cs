using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public abstract class PostProcessBase : VolumeComponent, IPostProcessComponent
{
    public BoolParameter IsRender = new BoolParameter(true);

    public bool IsActive()
    {
        return active && IsRender.value;
    }

    public bool IsTileCompatible()
    {
        return false;
    }
}