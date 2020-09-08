tool
extends EditorPlugin

func _enter_tree():
	add_custom_type("Quadtree","Node2D",preload("Quadtree.cs"),preload("quadtree.png"))

func _exit_tree():
	remove_custom_type("Quadtree")

