using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineCamera : MonoBehaviour
{
    float storedShadowDistance;

    void OnPreRender () {
    storedShadowDistance = QualitySettings.shadowDistance;
    QualitySettings.shadowDistance = 0;
    }

    void OnPostRender () {
    QualitySettings.shadowDistance = storedShadowDistance;
    }
}
