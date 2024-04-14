# Roll-a-ball Game Implementation
This markdown file describes the steps, thought process and my opinion on the tutorial, which taught me how to create my first game in Unity. Anyway, let's dig right in!

## Setting up the game
1. Since the main idea of developing this _game_ was to let you get started with Unity, I set up the project as asked in the tutorial. When creating the project from Unity Hub, I picked 3D URP template, as at the time I didn't know the difference between URP and Core.
2. After that I made 2 additional folders within Assets - one for Scripts and the other for Materials. This let me organize things better in the future.
3. I continued following the tutorial by creating the MiniGame scene, although was confused why we couldn't work with existing scene.
4. After creating the new scene, I followed the tutorial and made the base plane the player will stand on. I wanted it to feel a little different, so I chose the size myself and researched how to apply custom color. For that I made a green material, which I attached to the plane and decided the map should have a "grassy" feeling.
5. For the player, I made it as a sphere, so it can easily roll around, added blue color material, so it stands out from the map and a rigidbody, so it can actually be affected by physics.

## Moving the player
1. Following the tutorial, I first added the new Input System package to the game and then made a PlayerController script for the player. 
2. To use the new input system, I followed the tutorial and added a Player Input InputAction to the player. I was a little confused on how it worked, so I googled around to see some examples. I tried to make a custom "Fire" method, but it didn't quite work out, so I returned to the tutorial.
3. Obviously, following the tutorial, the "OnMove" method worked as expected - I could move in any direction I wanted by applying force to player's rigidbody. I still tried to fiddle around with custom methods and eventually made a "OnJump" method that let me jump around.
4. After setting up the methods I tried out various values for the variables to get them to feel nice.

## Moving the camera
1. Just like in the tutorial, I first set up my camera to look at the player I wanted and then noted down the values, so I know how it should be rotated.
2. At first, just like the tutorial suggested, I tried placing the camera as player's child object, but quickly realised that it's a bad idea. My screen just kept on rolling :D
3. I kept following the tutorial and set up the camera, so it's in a fixed position from the player - way better, although sadly it didn't face the way I was moving :(
4. Forgot to mention, but all of that was done in a separate CameraController script and in a LateUpdate() method, to have consistent camera movement.

## Play area and collectibles
1. I finally decided to furnish the map by setting up boundary walls, a small green hill and some challenging wall jumps to make it more fun.
2. After that, I made some gold cubes similar to the tutorial and placed them in difficult to reach spots, just so that they're not too easy to get. I first wanted them to be coins, but then decided to stick to the tutorial and have floating cubes instead. For effects, I added a separate gold cube script and gold color material.
3. I copied the rotation script from the tutorial into gold cube script, as well as made it a prefab, so I can copy paste it and adjust all of them at same time.
4. I followed the tutorial and added collision code to player controller, which let me collect the cubes. For fun I also added an "Explosion" push back, when the player collects them - so they have a harder time collecting the adjacent cubes >:D
5. To make the collisions work **correctly**, I also had to set up a tag for the cube and make so whenever player touches it, it becomes inactive. I also had to set the cubes collision as trigger.

## Displaying score
1. To display score, I once again just followed the tutorial and added a score counter variable within PlayerController, as well as a 2D TextMeshPro UI Element in the scene.
2. I quickly moved the Text in top left corner within the canvas and set the text to display score.
3. I made a reference to the UI Text within Player Controller and whenever the player's score changed (in the OnTriggerEnter method), I updated the UI text.
4. After that I had some fun trying to collect all the gold cubes and get max score :D

And that's my journey on how I made my first game in Unity. Tutorial used: https://learn.unity.com/project/roll-a-ball?uv=2022.3