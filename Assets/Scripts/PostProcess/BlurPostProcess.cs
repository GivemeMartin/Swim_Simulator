using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenu("Addition-Post-processing/BlurPostProcess")]
public class BlurPostProcess : PostProcessBase
{
    public ClampedFloatParameter range = new ClampedFloatParameter(0f, 0, 10);

}