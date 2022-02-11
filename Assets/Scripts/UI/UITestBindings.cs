using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITestBindings : MonoBehaviour
{
    public TMP_Text TMPdate;

    public TMP_Text TMPtime;

    public TMP_Text TMPSeg;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TMPdate.text = "Date:" + GameTimeManager.GameDate.ToString();
        TMPtime.text = "Time:" + GameTimeManager.Instance.GameTime.ToString();
        TMPSeg.text = "TimeSeg:" + GameTimeManager.Instance.CurrentTimeSeg.ToString();
    }
}
