using Godot;
using System;
using System.Collections.Generic;

public class MST : Node2D
{
	[Export]
	public bool DrawLines = false;
	[Export]
	public NodePath StartPoint;
	private Node2D startPoint;

	private List<Node2D> pointList = new List<Node2D>();
	private List<Vector2> result = new List<Vector2>();
	

	public override void _Ready()
	{
		if (StartPoint != null)
			startPoint = GetNode<Node2D>(StartPoint);
		
		UpdateMST();    
	}

	public void UpdateMST()
	{
		pointList.Clear();
		result.Clear();
		// get all child nodes

		foreach (Node2D n in GetChildren())
			pointList.Add(n);
		
		calcMST();
	}

	public void calcMST()
	{
		// Wenn Startpunkt ausgewählt wurde
		if (startPoint != null)
		{
			result.Add(startPoint.GlobalPosition);
			pointList.Remove(startPoint);
		}
		else
		{
			result.Add(pointList[0].GlobalPosition);
			pointList.RemoveAt(0);
		}
				   
		while (pointList.Count>0)
		{
			// speichern des kleinsten Abstandes
			float minDist = float.MaxValue;
			// Welcher Visited-Punkt war das
			int min_visited = -1;
			// Zu welchem Unvisited-Punkt
			int min_point = -1;

			// Alle besuchten Punkte durchgehen
			for (int visitedIndex=0;visitedIndex<result.Count;visitedIndex++)
			{
				// Alle unbesuchten Punkte
				for (int unVisitedIndex=0;unVisitedIndex<pointList.Count;unVisitedIndex++)
				{
					// Abstand berechnen
					if (result[visitedIndex].DistanceTo(pointList[unVisitedIndex].GlobalPosition)<minDist)
					{
						// Neues Minimun -> alles abspeichern
						minDist = result[visitedIndex].DistanceTo(pointList[unVisitedIndex].GlobalPosition);
						min_visited = visitedIndex;
						min_point = unVisitedIndex;
					}
				}
			}
			if (min_point != -1 && min_visited != -1)
			{
				// Punkt min_point in result speichern
				result.Add(pointList[min_point].GlobalPosition);
				// Punkt aus Liste pointList löschen
				pointList.RemoveAt(min_point);
			}
		}
		if (DrawLines)
			Update();
	}

	public override void _Draw()
	{
		for (int index = 0;index < result.Count-1; index++)
		{
			DrawLine(result[index], result[index + 1], new Color(1, 1, 1, 1));
		}
	}

	
	public List<Vector2> getMST() { return result; }

}
