extends Node2D

onready var s = $icon
var mov

func _ready():
	pass # Replace with function body.


func _process(delta):
	mov = Vector2(global_position.x+rand_range(-5,5),global_position.y+rand_range(-5,5))
	global_position = global_position.linear_interpolate(mov,0.4)
