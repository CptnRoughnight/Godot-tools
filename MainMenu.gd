extends Control


func _ready():
	pass # Replace with function body.



func _process(delta):
	pass


func _on_GotoQuadtree_pressed():
	get_tree().change_scene("res://Quadtree/test.tscn")


func _on_Exit_pressed():
	get_tree().quit()


func _on_GotoMSTDemo_pressed():
	get_tree().change_scene("res://MST/MSTDemo.tscn")
