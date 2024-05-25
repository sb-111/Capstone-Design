using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class timelineSpeed : MonoBehaviour
{
    public PlayableDirector director;
    void Start()
    {
        if (director != null)
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(0.5);
        }
    }
}
