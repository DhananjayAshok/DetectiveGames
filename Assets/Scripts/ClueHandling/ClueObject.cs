using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueObject
{
    public int index;
    public string clueName;
    public string clueString;
    public string clueLocation;
    public Sprite clueSprite;
    public string clueAutopsy;
    public bool isAutopsySuccess;
    public bool isAutopsied = false;

    public ClueObject(int index, string clueName, string clueString, string clueLocation, Sprite clueSprite, string clueAutopsy, bool isAutopsySuccess)
    {
        this.index = index;
        this.clueName = clueName;
        this.clueString = clueString;
        this.clueLocation = clueLocation;
        this.clueSprite = clueSprite;
        this.clueAutopsy = clueAutopsy;
        this.isAutopsySuccess = isAutopsySuccess;
    }

    public ClueObject(ClueScript cs, int index) {
        this.index = index;
        this.clueName = cs.clueName;
        this.clueString = cs.clueString;
        this.clueLocation = cs.clueLocation;
        this.clueSprite = cs.clueSprite;
        this.clueAutopsy = cs.clueAutopsy;
        this.isAutopsySuccess = cs.isAutopsySuccess;
    }

}
