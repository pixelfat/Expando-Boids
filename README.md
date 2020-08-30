# Squla-Boids-Unity-Challenge

## Dan Turner's Expando-boids

This is a **simple** toy, no need for resource or state control. Just set references and load the scene. 
It's a 2D/ortho only application.

Instruction priovided by Squla can be viewed [here](https://github.com/pixelfat/Squla-Boids-Unity-Challenge/blob/master/doc/Squla_Unity_Challenge.pdf).

### Notes:

- This took around 3 hours to write after a day of thinking about how to approach it. I keep tinkering with it so including uploading to github and writing this readme, I guess I may have gone over four or five hours... But I'm happy with the result.

- Boid code uses Unity lib for vector math but can be easily adapted to pure .net for use in other c# apps.

- Instead of a simple coh, sep, ali boid model I figured it might be fun to make an extendable model. 
For the purpose of this project, I call these 'Drivers'. - See: IBoidDriver.cs A default set is provided for regular Boid motion.

- Boids could interract with specific other boids through Drivers. Currently all boids share a single set of drivers for ease of use, so make sure
to create instances and assign them individually to get individual behaviors.

- All influences / driver values are properly normalised before being applied, so no magic numbers are required (I haven't seen a single tutorial or paper that doies this.. which makes me worry) for a balance in motion, just a uniform view-scale.
 
- There's no collision, but easy to implement. It uses time-steps so should be easy to integrate into Box2d without breaking determinism.
 
### Known Issues
GetBoidsInRange() doesn't account for  boids that are close to the opposite edge (boids are wrapped to 
screen-space, not killed and spawned... portals!) so they can get grouped.
 
Sorting would work better using a quadtree adress or other spacial sorting system, but it's not in scope and not required for only 50 agents.
