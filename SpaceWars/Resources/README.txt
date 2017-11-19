Jiahui Chen
u0980890
Mitch Talmadge
u1031378

Our Spacewars game has a very different visual aesthetic than the one demoed in class. It does
have music, so we encourage you to turn up your volume! We had many more plans for this game
(such as exploding animations, different colored projectils, etc) but due to time constraints 
we had to prioritize.

Controls: WASD to move, Space to fire. 

For the GUI we made separate WorldPanel and ScoreboadPanel classes, both which extend Panel. 
This separation made it clearer in the initial design to focus on redrawing and handling threads
for the game's components display and the scoreboard's functionality. Our scoreboard includes
a health bar that fills to scale according to the player's health and also sorts/draws the 
scores in order from highest scorer to lowest. 

We have created an MP3Player which uses NAudio from NuGet, so please make sure to download that package.

Our Networking class of static methods is structured differently from what was recommended in the 
instructions, we did this so we could catch all the possible exceptions thrown by networking code. 
The biggest change is we have 3 delegates: ConnectionEstablished which is called when a connection
is successfully established, having the parameter of a SocketState; ConnectionFailed which is called
when a connection fails, having the parameter of a string (the reason the connection failed); and 
DataReceived which is called when data is received, its parameter is the data.

Our ships, projectiles, stars, etc. all extend a base class of GameComponent, which allowed us to create
a generic dictionary of components in our World class.

The sprites, music, and other resources for our projects are contained in Resources.resx files found in 
the "Properties" collapsible group in the solution explorer for the respective projects.

We created a "Factory" for our SpaceWarsClient (controller) that prevents the user from accidentally interacting
with a SpaceWarsClient instance that did not connect successfully. 