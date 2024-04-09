using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCooldownBar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<GameObject> Skills = new List<GameObject>();
    [SerializeField] private Transform minPos, maxPos;
    [SerializeField] private List<Vector3> IconPositions = new List<Vector3>();
    void Start()
    {
        //SetIconPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIconPosition()
    {
        IconPositions.Clear();

        Vector3 distanceBetweenPoints = Vector3.zero;
        if (Skills.Count > 1)
        {
            distanceBetweenPoints =  (maxPos.position - minPos.position) / (Skills.Count -1 );
        }

        for (int i = 0; i < Skills.Count; i++)
        {
            IconPositions.Add(minPos.position + (distanceBetweenPoints * i));
            Skills[i].transform.position = IconPositions[i];
        }
    }

    public void AddIcon(GameObject _icon)
    {
        Skills.Add(_icon);
    }
}
