extends Node2D

onready var fps = $CanvasLayer/Label

func _ready():
	pass # Replace with function body.



func _process(delta):
	if (Input.is_action_just_pressed("Ende")):
		get_tree().change_scene("res://MainMenu.tscn")
	fps.text = str(Engine.get_frames_per_second())
