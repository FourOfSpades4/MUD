# MUD

Hi! I've always wanted to create an online game, both to play, but also to learn more about networking and security. I've spent countless hours exploiting security issues in other people's games, so it's time to make my own.  
</br>
This is a [MUD](https://en.wikipedia.org/wiki/Multi-user_dungeon), or a Multi-User Dungeon. It's effectively a Text Adventure Game built from the ground up to be multiplayer. 

### How does it work?
There are two main parts to this, the Server and the Client. The Server, built as a plugin for [DarkRift2](https://github.com/DarkRiftNetworking/DarkRift), handles the majority of the game logic. Persistant data is stored as an SQLite database, and reading and writing to that database is done through the [Database](https://github.com/FourOfSpades4/MUD/blob/main/Server/Server/Database.cs) file.  
</br>
The Client deals with simply displaying and sending data to and from the server. Most MUD games are directly in the browser, but I decided to make it through Unity in order to add additonal customization panels. I also simply have more experience with C# than I do with the web suite. 

### How can I customize it? 
Most of the data controlling things such as the rooms, areas, enemies, drops, skills, items and more are all stored in the database. Simple changes like editing descriptions, names, adding items and such are easily done through the database. For more substantial changes such as new abilities, code changes will be necessary.

### Is it secure?
I hope so! I'm sure once I take more Cybersecurity classes I'll realize some critical security flaws and redo everything, but here's currently how it works:  
- When a client connects, send them a Public Key.
- The client encrypts their username and password using that key.
- That data is then sent to the Server.
- The server decrypts the login information through its Private Key.
- The password is hashed and compared with the SQLite database storing the user information.
- The player is loaded into the Server memory and assigned a unique token, which is sent to the client.
- Whenever a command or chat message is entered, the token will also be sent.
- The Server will verify that the token is coming from the correct client before executing any commands.
