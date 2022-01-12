using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenu("Addition-Post-processing/PixelPostProcess")]
public class PixelPostProcess : PostProcessBase
{
    public ClampedFloatParameter blockCount = new ClampedFloatParameter(0f, 0, 1000);
}