# Bionica
(After "Start" just press and hold 'n' on your keyboard)

Bionic Sandbox

This application is sand box with bmp image, represents some area. There is a time-step with counter. In the area can be placed two types of creatures: plants and herbivores. 
Plant is a green dot with size 1x1 px. Plants are being spawned randomly depends on free space (The bmp's pixels, which has not another plants) each time-step. 
Herbovire is a brown dot with size 2x2 px. Herbivores are spawned once in 1st time-step. After that herbivores are being moved with each time-step in random directions. 
The range of the move is 2 px. If herbivore changed place to place, which is occupied by plant, it eats the plant. If herbivore eats the plant, his fullness increases. 
Each time-step fulness is decreasing. If fullness is sub-zero, herbivore dies. If fullness is great-value, herbivore duplicate itself, producing a new herbivore.
