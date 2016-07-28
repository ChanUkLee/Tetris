using UnityEngine;
using System.Collections;

public class ProgressUI : BaseUI {

    public override void Initialize()
    {
        base.Initialize();

        SetImage("ProgressImage", 0f);
        SetText("ProgressText", "0%");
    }

    public void SetRate(float rate)
    {
        SetImage("ProgressImage", rate);
        SetText("ProgressText", string.Format("{0:F1}%", rate * 100f));
    }
}
