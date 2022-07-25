extends Control



func _on_GotoQuadtree_pressed():
	get_tree().change_scene("res://Quadtree/test.tscn")


func _on_Exit_pressed():
	get_tree().quit()


func _on_GotoMSTDemo_pressed():
	get_tree().change_scene("res://MST/MSTDemo.tscn")


func _on_WfC_pressed():
	get_tree().change_scene("res://wfc/Wfc2D.tscn")
