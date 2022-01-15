using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MacroPostProcess : PostProcessBase
{
    public ClampedFloatParameter range = new ClampedFloatParameter(0f, 0, 1);
    public ClampedFloatParameter edge = new ClampedFloatParameter(0f, 0, 1);

}
