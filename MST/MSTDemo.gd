extends Node2D

onready var MST = $MST
var tex = preload("res://icon.png")

func _process(delta):
	if (Input.is_action_just_pressed("Ende")):
		get_tree().change_scene("res://MainMenu.tscn")


func _on_AddRandom_pressed():
	var spr = Sprite.new()
	spr.texture = tex
	spr.global_position = Vector2(rand_range(0,1024),rand_range(0,600))
	MST.add_child(spr)
	MST.UpdateMST()
	pass # Replace with function body.


func _on_RandomStart_pressed():
	MST.startPoint = MST.get_child(rand_range(0,MST.get_child_count()))
	MST.UpdateMST()
