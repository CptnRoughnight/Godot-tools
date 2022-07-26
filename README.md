# Godot-tools Godot 3.2.2 mono version
 This repository is used to collect various tools that I use in Godot (3.2.2 Mono Version).
 Feel free to use these tools too!

## currently implemented tools and demos
### Quadtree
Add Quadtree-Plugin into project folder, activate the plugin in the project settings.
Add a Quadtree-Node in the Editor. Change the bounds of the Quadtree. Select the desired amount of Nodes per Cell.
All Node2D related Nodes under the Quadtree node will be automatically in the quadtree. 
You can get a list of nodes in a certain rect by using $Quadtree.returnNodeList(Rect2)

### MST
The MST-Node gives you a minimum spanning tree from the nodes under the MST-node.
Get the Path-List (Vector2) by using $MST.getMST()

### WFC
Wave Function Collapse Algo, inspired by "The coding Train"