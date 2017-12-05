Jiahui Chen
u0980890
Mitch Talmadge
u1031378

PS8: Server README

Testing:
We unit tested our World Project and Properties Project with unit tests. For the World Project, 
the code coverage for the World class is low, but it's because we didn't unit test the FindShipSpawnLocation
method (a method that finds open/empty space in the GUI to draw ships) since it's so dependent on
game state. We ensured the method works by generating a lot of ships (with the AI client) and ensuring 
none spawned in a star. 

We created a Properties project that serializes to and from an XML file. The file is called server_properties.xml 
and will be generated automatically on the first run of the application.

Our server uses a logging library from NuGet to log both to the console and to a file. 
Please ensure you download the NuGet packages before running our application.

We created our own game loop that attempts to maintain a constant FPS as specified in the properties file.

Every client is managed by their own "client communicator", whose sole purpose is to talk to and listen to the client.
We employed separation of concerns in all aspects of our application.

Our networking library is heavily modified from the examples given in class and in the assignment description.
We have three main classes - the AbstractNetworking class, ClientNetworking class, and ServerNetworking class.
Each class contains code shared by clients and servers, specific to clients, and specific to servers, respectively.
We use events throughout our networking class for things such as disconnections, data being received, and more.

The "Main" entry point to our server lies in: 

View Projects/Server Projects/Server

We consider our console as a "view", since it displays text and, although we did not implement it, 
may also accept user input in the future.

------------------------------------------------------------------------------------
------------------------------------------------------------------------------------
------------------------------------------------------------------------------------

PS7: Client README
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