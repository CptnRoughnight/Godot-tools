tool
extends EditorPlugin

func _enter_tree():
	add_custom_type("MST","Node2D",preload("MST.cs"),preload("MST.png"))

func _exit_tree():
	remove_custom_type("Quadtree")

