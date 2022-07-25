using Godot;    
using System;
using System.Collections.Generic;
using System.Linq;

public class wfc2d : Node2D
{
    /**************************************** Exports */
    [Export]
    private int GridSize = 5;

    /**************************************** onreadys */

    private Label fpsLabel;
    private Camera camera;

    /**************************************** Modules */
    private Texture sprPath = (Texture)ResourceLoader.Load("res://wfc/path1.png");
    private Texture sprDirt = (Texture)ResourceLoader.Load("res://wfc/dirt_patch.png");
    private Texture sprPath2 = (Texture)ResourceLoader.Load("res://wfc/path2.png");

    private Texture sprcdr = (Texture)ResourceLoader.Load("res://wfc/cdr.png");
    private Texture sprcld = (Texture)ResourceLoader.Load("res://wfc/cld.png");
    private Texture sprclu = (Texture)ResourceLoader.Load("res://wfc/clu.png");
    private Texture sprcur = (Texture)ResourceLoader.Load("res://wfc/cur.png");

    /**************************************** Const */

    private const int TileWidth = 14;       // Godot meters
    private const int TileHeight = 14;


    /**************************************** Variables */
    private WfcTile2D[,] grid;
    private WfcTile2D[,] standardGrid;
    private WfcTile2D[,] newGrid;

    private List<TILE2D> fullEntropyList;


    private RandomNumberGenerator rng;
    private Vector2 walker;
    private List<Vector2> lowEntropyList;
    private int passes;

    public override void _Ready()
    {
        fpsLabel = (Label)GetNode("CanvasLayer/VBoxContainer/FPS");
        rng = new RandomNumberGenerator();
        rng.Randomize();
        // Init Grid with highest entropy possible
        grid = new WfcTile2D[GridSize, GridSize];
        standardGrid = new WfcTile2D[GridSize, GridSize];
        newGrid = new WfcTile2D[GridSize, GridSize];

        for (int i = 0; i < GridSize; i++)
            for (int j = 0; j < GridSize; j++)
                grid[i, j] = new WfcTile2D();

        List<TILE2D> fullEntropyList = new List<TILE2D>();
        for (int l = 0; l < Enum.GetNames(typeof(TILE2D)).Length; l++)
            fullEntropyList.Add((TILE2D)l);

        Array.Copy(grid, standardGrid, grid.Length);

        
        lowEntropyList = new List<Vector2>();
        passes = 0;

        grid[0, 0].collapse(rng);
        
        DebugPlaceMesh(new Vector2(0, 0));
        UpdateNeighbours();

        run();

    }

    public void run()
    {
        while (CheckFullyCollapsed() == false)
        {
            // die neuen Tiles zum collapsen
            GetLowEntropyList();

            if (lowEntropyList.Count <= 0)
                return;


            // Random Item picken
            var index =  (uint)rng.Randi() % lowEntropyList.Count;

            var rWalker = lowEntropyList[(int)index];
            // Collapse tile
            grid[(int)rWalker.x, (int)rWalker.y].collapse(rng);

            UpdateNeighbours();
            DebugPlaceMesh(rWalker);

        }
    }

    private void CheckValid(ref List<TILE2D> arr,List<TILE2D> valids)
    {
        arr = arr.Intersect(valids).ToList();
    }

    private void UpdateNeighbours()
    {
        Array.Copy(standardGrid,newGrid, standardGrid.Length);

        for (int i = 0; i < GridSize; i++)
            for (int j = 0; j < GridSize; j++)
            {
                if (grid[i, j].collapsed)
                    newGrid[i, j] = grid[i, j];
                else
                {
                    List<TILE2D> options = new List<TILE2D>();
                    for (int l = 0; l < Enum.GetNames(typeof(TILE2D)).Length; l++)
                        options.Add((TILE2D)l);

                    // look above
                    if (j>0)
                    {
                        List<TILE2D> valids = new List<TILE2D>();
                        if (grid[i, j - 1].collapsed)
                        {
                            valids.AddRange(TileDesciptions2D.getValidTiles((TILE2D)grid[i, j].Mesh, grid[i, j - 1].Mesh, DIRECTION.UP));
                        }
                        else
                        {
                            for (int k = 0; k < grid[i, j - 1].options.Count; k++)
                            {
                                valids.AddRange(TileDesciptions2D.getValidTiles((TILE2D)grid[i, j].Mesh, grid[i, j - 1].options[k], DIRECTION.UP));

                            }
                        }
                        valids = valids.Distinct().ToList();
                        options = options.Intersect(valids).ToList();
                    }

                    // look down
                    if (j < GridSize-1)
                    {
                        List<TILE2D> valids = new List<TILE2D>();
                        if (grid[i, j + 1].collapsed)
                        {
                            valids.AddRange(TileDesciptions2D.getValidTiles((TILE2D)grid[i, j].Mesh, grid[i, j + 1].Mesh, DIRECTION.DOWN));
                        }
                        else
                        {
                            for (int k = 0; k < grid[i, j + 1].options.Count; k++)
                            {
                                valids.AddRange(TileDesciptions2D.getValidTiles((TILE2D)grid[i, j].Mesh, grid[i, j + 1].options[k], DIRECTION.DOWN));

                            }
                        }
                        valids = valids.Distinct().ToList();
                        options = options.Intersect(valids).ToList();
                    }

                    // look left
                    if (i > 0)
                    {
                        List<TILE2D> valids = new List<TILE2D>();
                        if (grid[i - 1, j ].collapsed)
                        {
                            valids.AddRange(TileDesciptions2D.getValidTiles((TILE2D)grid[i, j].Mesh, grid[i - 1, j ].Mesh, DIRECTION.LEFT));
                        }
                        else
                        {
                            for (int k = 0; k < grid[i - 1, j].options.Count; k++)
                            {
                                valids.AddRange(TileDesciptions2D.getValidTiles((TILE2D)grid[i, j].Mesh, grid[i - 1, j].options[k], DIRECTION.LEFT));

                            }
                        }
                        valids = valids.Distinct().ToList();
                        options = options.Intersect(valids).ToList();
                    }

                    // look right
                    if (i < GridSize-1)
                    {
                        List<TILE2D> valids = new List<TILE2D>();
                        if (grid[i + 1, j].collapsed)
                        {
                            valids.AddRange(TileDesciptions2D.getValidTiles((TILE2D)grid[i, j].Mesh, grid[i + 1, j].Mesh, DIRECTION.RIGHT));
                        }
                        else
                        {
                            for (int k = 0; k < grid[i + 1, j].options.Count; k++)
                            {
                                valids.AddRange(TileDesciptions2D.getValidTiles((TILE2D)grid[i, j].Mesh, grid[i + 1, j].options[k], DIRECTION.RIGHT));

                            }
                        }
                        valids = valids.Distinct().ToList();
                        options = options.Intersect(valids).ToList();
                    }
                    newGrid[i, j].options = options;
                }
            }
        grid = newGrid;

    }
    

    // Check if the grid is completed
    private bool CheckFullyCollapsed()
    {
        for (int i = 0; i < GridSize; i++)
            for (int j = 0; j < GridSize; j++)
                if (grid[i, j].collapsed == false)
                    return false;
        return true;
    }

    // Get the lowest entropy tiles
    private void GetLowEntropyList()
    {
        int lowest = int.MaxValue;
        lowEntropyList.Clear();

        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                if ((grid[i, j].collapsed == true) || (grid[i, j].getEntropy() > lowest))  // tile is done or entropy is greater
                    continue;
                

                if (grid[i, j].getEntropy() < lowest)
                {
                    
                    lowest = grid[i, j].getEntropy();
                    lowEntropyList.Clear();
                    lowEntropyList.Add(new Vector2(i, j));
                } else if (grid[i, j].getEntropy() == lowest)
                    // gleich wie low dann rein
                    lowEntropyList.Add(new Vector2(i, j));
            }
        }
    }

    private void DebugPlaceMesh(Vector2 walker)
    {
        Sprite instance = null;
        if (grid[(int)walker.x,(int)walker.y].collapsed)
        {
            switch (grid[(int)walker.x, (int)walker.y].Mesh)
            {
                case TILE2D.Path:
                    instance = new Sprite();
                    instance.Texture = sprPath;
                    break;
                case TILE2D.Dirt:
                    instance = new Sprite();
                    instance.Texture = sprDirt;
                    break;
                case TILE2D.Path2:
                    instance = new Sprite();
                    instance.Texture = sprPath2;
                    break;
                case TILE2D.cdr:
                    instance = new Sprite();
                    instance.Texture = sprcdr;
                    break;
                case TILE2D.cld:
                    instance = new Sprite();
                    instance.Texture = sprcld;
                    break;
                case TILE2D.clu:
                    instance = new Sprite();
                    instance.Texture = sprclu;
                    break;
                case TILE2D.cur:
                    instance = new Sprite();
                    instance.Texture = sprcur;
                    break;
                default:
                    instance = null;
                    break;

            }
            if (instance == null)
                return;
            AddChild(instance);
            instance.GlobalPosition = new Vector2(walker.x * 32, walker.y * 32);
        }
    }

    
    public override void _Process(float delta)
    {
        fpsLabel.Text = "FPS : " + Engine.GetFramesPerSecond();

    }
}
