using Godot;
using System;
using System.Collections.Generic;

public enum TILE2D
    {
        Path = 0,
        Dirt=1,
        Path2=2,
        cdr=3,
        cld=4,
        clu=5,
        cur=6
    }

    public enum NEIGHBOUR2D
    {
        NONE = 0,
        VERTICAL = 1,
        HORIZONTAL = 2,
    }



public struct Description2D
{
    public NEIGHBOUR2D ConnectorUp;
    public NEIGHBOUR2D ConnectorDown;
    public NEIGHBOUR2D ConnectorLeft;
    public NEIGHBOUR2D ConnectorRight;

    public Description2D(NEIGHBOUR2D up, NEIGHBOUR2D down, NEIGHBOUR2D right, NEIGHBOUR2D left)
    {
        ConnectorUp = up;
        ConnectorDown = down;
        ConnectorLeft = left;   
        ConnectorRight = right; 
    }
}

public enum DIRECTION
{
    LEFT = 0,
    RIGHT,
    UP,
    DOWN
}

public static class TileDesciptions2D
{

    public static Description2D[] descriptions = new Description2D[Enum.GetNames(typeof(TILE2D)).Length];

    static TileDesciptions2D()
    {
        //              TILE2D                                      UP                      DOWN                        RIGHT                       LEFT
        descriptions[(int)TILE2D.Path]    = new Description2D(NEIGHBOUR2D.NONE,         NEIGHBOUR2D.NONE,           NEIGHBOUR2D.HORIZONTAL,     NEIGHBOUR2D.HORIZONTAL);
        descriptions[(int)TILE2D.Dirt]    = new Description2D(NEIGHBOUR2D.VERTICAL,     NEIGHBOUR2D.VERTICAL,       NEIGHBOUR2D.HORIZONTAL,     NEIGHBOUR2D.HORIZONTAL);
        descriptions[(int)TILE2D.Path2]   = new Description2D(NEIGHBOUR2D.VERTICAL,     NEIGHBOUR2D.VERTICAL,       NEIGHBOUR2D.NONE,           NEIGHBOUR2D.NONE);

        descriptions[(int)TILE2D.cdr] =     new Description2D(NEIGHBOUR2D.NONE,         NEIGHBOUR2D.VERTICAL,       NEIGHBOUR2D.HORIZONTAL,     NEIGHBOUR2D.NONE);
        descriptions[(int)TILE2D.cld] =     new Description2D(NEIGHBOUR2D.NONE,         NEIGHBOUR2D.VERTICAL,       NEIGHBOUR2D.NONE,           NEIGHBOUR2D.HORIZONTAL);
        descriptions[(int)TILE2D.clu] =     new Description2D(NEIGHBOUR2D.VERTICAL,     NEIGHBOUR2D.NONE,           NEIGHBOUR2D.NONE,           NEIGHBOUR2D.HORIZONTAL);
        descriptions[(int)TILE2D.cur] =     new Description2D(NEIGHBOUR2D.VERTICAL,     NEIGHBOUR2D.NONE,           NEIGHBOUR2D.HORIZONTAL,     NEIGHBOUR2D.NONE);
    }

   
    public static List<TILE2D> getValidTiles(TILE2D meTile,TILE2D tile,DIRECTION myConnectorDir)
    {
        List<TILE2D> result = new List<TILE2D>();
        switch(myConnectorDir)
        {
            case DIRECTION.LEFT:
                for (int t = 0; t < Enum.GetNames(typeof(TILE2D)).Length; t++)
                    if (TileDesciptions2D.descriptions[t].ConnectorLeft == TileDesciptions2D.descriptions[(int)tile].ConnectorRight)
                        result.Add((TILE2D)t);
                break;
            case DIRECTION.RIGHT:
                for (int t = 0; t < Enum.GetNames(typeof(TILE2D)).Length; t++)
                    if (TileDesciptions2D.descriptions[t].ConnectorRight == TileDesciptions2D.descriptions[(int)tile].ConnectorLeft)
                        result.Add((TILE2D)t);
                break;
            case DIRECTION.UP:
                for (int t = 0; t < Enum.GetNames(typeof(TILE2D)).Length; t++)          
                    if (TileDesciptions2D.descriptions[t].ConnectorUp == TileDesciptions2D.descriptions[(int)tile].ConnectorDown)  
                        result.Add((TILE2D)t);
                break;
            case DIRECTION.DOWN:
                for (int t = 0; t < Enum.GetNames(typeof(TILE2D)).Length; t++)
                    if (TileDesciptions2D.descriptions[t].ConnectorDown == TileDesciptions2D.descriptions[(int)tile].ConnectorUp)
                        result.Add((TILE2D)t);
                break;
        }
        return result;
    }
}

public class WfcTile2D
{
    public TILE2D Mesh { get; set; }
    public bool collapsed;

    public List<TILE2D> options;
    public List<TILE2D> newOptions;


    public WfcTile2D()
    {
        options = new List<TILE2D>();
        newOptions = new List<TILE2D>();
        for (int i = 0; i < Enum.GetNames(typeof(TILE2D)).Length; i++)
            options.Add((TILE2D)i);
        collapsed = false;
    }

    public void collapse(RandomNumberGenerator rng)
    {
        // Choose a TILE2D from Options list, prefer not to take NONE
        uint index = (uint)(rng.Randi() % options.Count);
        var t = (uint)options[(int)index];
        Mesh = (TILE2D)t;

        options.Clear();
        options.Add((TILE2D)t);
        collapsed = true;
    }

    public int getEntropy()
    {
        if (options == null)
            return -1;
        return options.Count;
    }

}