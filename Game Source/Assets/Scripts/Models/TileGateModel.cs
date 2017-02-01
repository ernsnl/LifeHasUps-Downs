using System;
using UnityEngine;
using System.Collections;

public class TileGateModel : MonoBehaviour
{


    [HideInInspector]
    public bool isCertainNorth = false;
    [HideInInspector]
    public bool isCertainSouth = false;
    [HideInInspector]
    public bool isCertainWest = false;
    [HideInInspector]
    public bool isCertainEast = false;

    public int northGate;
    public int southGate;
    public int eastGate;
    public int westGate;


    public TileGateModel(int N, int S, int E, int W, bool initialSet = true)
    {
        northGate = N;
        eastGate = E;
        southGate = S;
        westGate = W;

        isCertainEast = false;
        isCertainNorth = false;
        isCertainSouth = false;
        isCertainWest = false;

    }

}
