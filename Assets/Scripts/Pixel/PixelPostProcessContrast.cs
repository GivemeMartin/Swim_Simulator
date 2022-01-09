using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenu("Addition-Post-processing/PixelPostProcessContrast")]
public class PixelPostProcessContrast : VolumeComponent, IPostProcessComponent
{
    public ClampedFloatParameter blockCount = new ClampedFloatParameter(0f, 0, 1000);

    public bool IsActive()
    {
        return active;
    }

    public bool IsTileCompatible()
    {
        return false;
    }
}