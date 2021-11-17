using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GoalSet
{
    public Transform[] goalWaypoints;
    [HideInInspector]
    int state = 0;

    public Transform TransitionWaypoint() {
        Transform to_ret = goalWaypoints[state];
        UpdateState();
        return to_ret;
    }

    public void UpdateState() {
        state++;
        if (state >= goalWaypoints.Length) {
            state = 0;
        }
    }

    public int GetState() {
        return state;
    }

    public bool noGoals() {
        if (goalWaypoints == null)
        {
            return true;
        }
        else {
            return (goalWaypoints.Length == 0);
        }
    }
}
