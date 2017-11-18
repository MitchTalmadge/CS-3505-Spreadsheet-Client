Jiahui Chen
u0980890
Mitch Talmadge
u1031378

Our Spacewars game has a very different visual aesthetic than the one demoed in class. 

For the GUI we made separate WorldPanel and ScoreboadPanel classes, both which extend Panel. 
This separation made it clearer in the initial design to focus on redrawing and handling threads
for the game's components display and the scoreboard's functionality. Our scoreboard includes
a health bar that fills to scale according to the player's health and also sorts/draws the 
scores in order from highest scorer to lowest. 

Our Networking class of static methods is structured differently from what was recommended in the 
instructions, we did this so we could catch all the possible exceptions thrown by networking code. 
The biggest change is we have 3 delegates: ConnectionEstablished which is called when a connection
is successfully established, having the parameter of a SocketStat; ConnectionFailed which is called
when a connection fails, having the parameter of a string (the reason the connection failed); and 
DataReceived which is called when data is received, its parameter is the data.
