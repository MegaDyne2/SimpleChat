# SimpleChat
SimpleChat is a Unity-based chat application prototype designed to display a list of conversations, individual profiles, and messages, using JSON files to emulate server-side data. The project focuses on reusable components, modular design, and scalability, while prioritizing simplicity and maintainability.
List of Targeted Goals:​
  Show a list of conversations
  Display a friend name, icon/photo, time of the last message
  Clicking on name should display a conversation with that person
  Have about 30 conversations in the list
  Show friend’s profile
  Clicking on the friend’s icon in conversation list should display a profile dialog showing name and a larger version of photo
  Display a conversation with a friend (list of messages)
  Show your icon, friend’s icon, message and date/time message was sent
  Have about 300 messages, display last 50, consider implementing ‘show more’ button or load more by scrolling. Or propose another way of dealing with many messages.
  Make all the data come from various JSON files (please design data structure), emulating data that would be received from the server
  Break up chat components into reusable prefabs

Project Contributions and Observations​​:
  ​Icons/Images are downloaded off the internet in real-time.
  Json files are stored locally.  I imagine that Json is downloaded through SQL, but I don't have any way to create a new server for it.  And the format and order of the values are set on the server-side
  Views are Conversation, Profile, and Message
  Json for the Conversations List is stored in Resources/Conversations/conversations_#.  please click on the buttons below to start.
  Type of Conversations:
    0. Show only the Selectable Conversation #0-6
    1. Show up to 30 Conversations. all other Conversations cannot be selected as message but is still able to open profile.
    2. Shows the 1st two conversations. So that you can see what it looks like if there's isn't much of it.
    ​3. Shows the 1st four conversations.
    4. Has no conversations
  Conversations in the list are not being sorted. From my understanding, the SQL server should have returned the array in order.
  Profile is loaded using User's ID, Images will be redownloaded again. In case there's a change in the internet.
  Json for the User's data are stored in Resources/UserData/*UserID*
  I kept User's data in its Json in case we need to add more variables to it. Last time Online, Male/Female, Location, Ect.
  Both Conversations and Message List in the UI fill out from bottom to top.
  Json for Messages are stored in Resources/Messages/*ConversationID*/*Files with "Recent" or the index number of the Message-ID:
    It is stored by Recent file and index number because I believe I would be passing in an SQL to either get the most recent and the past 50 messages, or I pass in the ending index and get the pass 50 from there.
    "Recent" will have an index with their Messages.  I can look at the lowest one and subtract one from it to tell the SQL to get the next set of messages.
  The Messages Json will contain the MainUserID, OthersID, a boolean if there's are more messages and an array of Messages.
  I made it so that there can be multiple users in the Conversation.
  The array of messages will have the ID, index of the user (-1 is the Main User, any number above will be the speaker.)
  Type of Messages:
    000000: One full page of 50 messages.
    000001: Split the first one into 2 parts. 25 messages and another 25 messages. 
    000002: Only the second part with an error of "hasMoreMessage" when it doesn't load more messages will show you an error.
    000003: Split the first one into 2 parts. while the second part is an empty array.
    000004: Have 300 Messages going 50 at a time.
    000005: Have 2 Messages. to shows that it goes from bottom to top.
    000006: Have an array of 0 messages.
    000007: Start at Message 250 - 300 and then 225 - 249.
  The Conversation shows the last messages but it doesn't match with the Message View of it. This is because I believe that the SQL server will spit out the last DateTime for the conversation.
