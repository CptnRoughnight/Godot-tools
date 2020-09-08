using Godot;
using System;
using System.Collections.Generic;


public class Quadtree : Node2D
{
	private List<Node2D> elements = new List<Node2D>();
	[Export]
	private int subDivideThreshold;
	[Export]
	private Rect2 boundary;
	[Export]
	private bool DrawLines;
	[Export]
	public bool updateQuadtree;


	public Rect2 Boundary { get => boundary; set => boundary = value; }

	// Subsections
	private bool subdivided = false;
	private Quadtree northwest = null;
	private Quadtree northeast = null;
	private Quadtree southeast = null;
	private Quadtree southwest = null;

	public override void _Ready()
	{
		BuildQuadTree();
	}

	public override void _Process(float delta)
	{
		if (updateQuadtree)
		{
			ClearQuadTree();
			BuildQuadTree();
		}
	}

	public void ClearQuadTree()
	{
		if (subdivided)
		{
			northwest.ClearQuadTree();
			northeast.ClearQuadTree();
			southeast.ClearQuadTree();
			southwest.ClearQuadTree();
		}
		elements.Clear();
	}

	public void BuildQuadTree()
	{
		subdivided = false;
		// Get All Child-Nodes
		foreach (Node2D n in GetChildren())
		{
			Insert(n);
		}
		if (DrawLines)
			Update();
	}

	public void Insert(Node2D point)
	{
		if (!boundary.HasPoint(point.GlobalPosition))
			return;
		
		if (elements.Count < subDivideThreshold)
		{
			elements.Add(point);
		}
		else
		{
			if (!subdivided)
			{
				subdivide();
				subdivided = true;
			}
			northeast.Insert(point);
			northwest.Insert(point);
			southeast.Insert(point);
			southwest.Insert(point);
		}
	}

	private void subdivide()
	{		
		Rect2 nw = new Rect2(boundary.Position.x, boundary.Position.y, boundary.Size.x / 2, boundary.Size.y / 2);
		northwest = new Quadtree();
		northwest.Boundary = nw;
		northwest.subDivideThreshold = subDivideThreshold;
		
		Rect2 ne = new Rect2(boundary.Position.x + boundary.Size.x / 2, boundary.Position.y, boundary.Size.x / 2, boundary.Size.y / 2);
		northeast = new Quadtree();
		northeast.Boundary = ne;
		northeast.subDivideThreshold = subDivideThreshold;

		Rect2 se = new Rect2(boundary.Position.x + boundary.Size.x / 2, boundary.Position.y + boundary.Size.y / 2, boundary.Size.x / 2, boundary.Size.y / 2);
		southeast = new Quadtree();
		southeast.Boundary = se;
		southeast.subDivideThreshold = subDivideThreshold;

		Rect2 sw = new Rect2(boundary.Position.x, boundary.Position.y + boundary.Size.y / 2, boundary.Size.x / 2, boundary.Size.y / 2);
		southwest = new Quadtree();
		southwest.Boundary = sw;
		southwest.subDivideThreshold = subDivideThreshold;
	}

	public override void _Draw()
	{
		if (DrawLines)
			DrawRects(this);
	}
	public void Draw()
	{		
		Update();
	}

	public void DrawRects(Quadtree node)
	{
		if (node == null)
			return;
		DrawRect(node.Boundary, Color.Color8(255, 255, 255),false);

		if (node.elements!=null)
			foreach (Node2D n in node.elements)
			{
				Vector2 p = n.GlobalPosition;
				Vector2 p2 = new Vector2(p.x + 1, p.y + 1);
				DrawLine(p,p2, Color.Color8(255, 0, 0));
			}
			
		if (subdivided)
		{
			DrawRects(node.northeast);
			DrawRects(node.northwest);
			DrawRects(node.southeast);
			DrawRects(node.southwest);
		}
	}

	public List<Node2D> returnNodeList(Rect2 bounds)
	{
		List<Node2D> found = new List<Node2D>();
		if (!this.boundary.Intersects(bounds))
			return found;
		else
		{
			if (elements != null)
			{
				foreach (Node2D n in elements)
					if (bounds.HasPoint(n.GlobalPosition))
					{
						found.Add(n);
					}
				if (subdivided)
				{
					found.AddRange(northwest.returnNodeList(bounds));
					found.AddRange(northeast.returnNodeList(bounds));
					found.AddRange(southeast.returnNodeList(bounds));
					found.AddRange(southwest.returnNodeList(bounds));
				}
			}
		}
		return found;		
	}
}
