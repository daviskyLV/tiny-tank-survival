# First Tiny Tank Survival development update!
Hello, this is the first game update blog post for my game - Tiny Tank Survival! Let's take a look at some of the major things done until now and the journey of how we got there.

## Unity Asset Store & Test map
I started off with exploring the Unity Asset Store for free assets - to get some inspiration and also find models to work with. Most of them didn't have a cohesive style, except "low-poly" assets, which is why I decided to aim for cartoonish/low-poly style game. I'm not good at modeling myself either, so with what little I could gather (that stuck together well), a ping-pong on the beach test map was born. For the bullets I found a nice looking rocket asset pack by AurynSky.

## Tank model
While creating the map, I also tried making a simple player model. It wasn't too difficult to create the model, and since I wanted to reuse it for enemies as well I made it a prefab. Turns out, you should try to keep your `Prefabs` at `0,0,0` coordinates, otherwise it could get messy when instantiating them as a child of another `GameObject`. Another thing I noticed is that scaling is local not absolute by default and that when rotating a child `GameObject`, it could get warped. The solution to this was to create an empty parent `GameObject` with scale `1,1,1`.

## Player
After creating the tank model, I went onto creating a _proper_ way to control the player. I found out that a good way to do this is to decouple the movement and camera scripts, which shouldn't be a part of the model but a separate _Player_ GameObject. So I made a _Master_ scene that holds the MainCamera and empty Player GameObject. I also made the test map a separate scene that gets loaded on top of Master scene during runtime. But soon I realised that it was hard for GameObjects to listen to each other between scenes, so I made another Player GameObject Prefab that held the movement script and would be instantiated into the map at runtime.

## Enemies
Spawning the enemies wasn't hard - I made a quick _Spawner_ Prefab and gave a nice spinning effect. The spawning script wasn't a part of the spawner though, as I wanted it to depend per map, rather than per spawn, so I put the spawning script as part of test map scene. Well, turns out you can't have scripts as part of scene, so I put it as part of an empty GameObject. To group enemies and enemy spawns I used empty GameObjects, this also let me traverse them to count how many enemies are there or where can I spawn them.