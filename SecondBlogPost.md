# Second Tiny Tank Survival development update!
After some development, there have been a lot of developments for the game and with lots of pain debugging. Let's take a look!

## Core bullet mechanics
After the last update I started working on implementing the core bullet mechanics, including bouncing off of walls. Initially it went well - I used the rocket model provided by _Rocket & Missile_ asset pack by _AurynSky_ and started scripting its mechanics. <br/>
Setting up the collision was easy and as a temporary solution the bullet would explode upon impact with anything other than walls. Of course it would also destroy players and enemies (literally `Destroy()` method). <br/>
Moving and rotating the rocket was another deal - the struggle with understanding `quaternions` and how to correctly apply them, especially since I should only set position and rotation of a `rigidbody` once per physics frame, meant that I had to precalculate the end rotation rather than use `transform.LookAt()` (for heat seeking missiles).

## Player & Enemy shooting mechanics
Surprisingly, implementing player & enemy shooting mechanics were most likely the easiest parts. The tank model, which is shared by player and enemy as a prefab, manages spawning in the bullet. In the script all I had to do was spawn the bullet in the scene, set up its `transform` properties and other parameters. For aiming it was also simple, I used `transform.LookAt()` with same Y height of target position as turret's position to rotate the turret. <br/>
To shoot as a player, I hooked up `Fire` input to a script, that checks if the player has enough ammo via `AmmoManager`, and uses the previously mentioned tank model to shoot the bullet. For the enemies it was a little harder, as I had to use a ray to check is the player in range and nothing is blocking the view and only then fire. <br/>
The `AmmoManager` started to also implement upgrade system, but I didn't go too far with it. While making it, I learned a bit more about `coroutines` in Unity, as well as reused `Actions` to signal when ammo has reloaded or current reload progress.

## Enemy movement
The hardest part definitely was making the enemies move. Since I wanted a little bit custom movement from the built-in `NavMeshAgent` movement behaviour, I had to (right?) do it myself. After watching a couple of tutorials by _LlamaAcademy_ on _YouTube_ about AI & NavMeshes, I got the path working. The problem was making the `agent` move from point to point, as I wanted it to stop, rotate and then continue moving. The battle with quaternions from the bullet implementation continued here and in the end, while quite buggy, the rotation managed to work? After the enemy had rotated, I allowed it to move towards the next corner in `NavMeshPath` via `agent.Move()`.

## Score HUD
Creating UI in Unity was a bit annoying at first, especially trying to make the `HorizontalLayoutGroup` work. For some reason it didn't align the ammo elements in the UI correctly, so I had to disable and enable it to "refresh". Made also a small color animation as the reload time for new ammo goes down, as well as used `filled image` to show visual progress.