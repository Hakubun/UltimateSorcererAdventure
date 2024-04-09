using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialXPBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void OnDestroy()
    {
        tutorialGM.Instance.Skill();
    }
}
