extends Node2D

onready var fps = $CanvasLayer/Label
onready var nodes = $CanvasLayer/foundNodes
onready var qtree = $Quadtree

func _ready():
	pass # Replace with function body.



func _process(delta):
	if (Input.is_action_just_pressed("Ende")):
		get_tree().change_scene("res://MainMenu.tscn")
	fps.text = str(Engine.get_frames_per_second())
	var n = qtree.returnNodeList(qtree.TestRect)
	nodes.text = "nodes in testrect : " +str(n.size())
