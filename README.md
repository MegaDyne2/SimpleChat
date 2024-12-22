# SimpleChat

**SimpleChat** is a Unity-based chat application prototype designed to display a list of conversations, individual profiles, and messages, using JSON files to emulate server-side data. The project focuses on reusable components, modular design, and scalability, while prioritizing simplicity and maintainability.

## List of Targeted Goals
- Show a list of conversations.
- Display a friend's name, icon/photo, and time of the last message.
- Clicking on a name should display a conversation with that person.
- Have about 30 conversations in the list.
- Show a friend's profile.
  - Clicking on the friend's icon in the conversation list should display a profile dialog showing their name and a larger version of their photo.
- Display a conversation with a friend (list of messages).
  - Show your icon, friend's icon, message, and date/time the message was sent.
- Have about 300 messages, display the last 50, and consider implementing a "show more" button or loading more by scrolling.
- Make all the data come from various JSON files (please design the data structure), emulating data that would be received from the server.
- Break up chat components into reusable prefabs.

## Project Contributions and Observations
- Icons/Images are downloaded from the internet in real-time.
- JSON files are stored locally. The JSON structure assumes data is downloaded through SQL, although no server is set up for this project.
- Views include:
  - Conversation
  - Profile
  - Message
- JSON for the Conversations List is stored in `Resources/Conversations/conversations_#`. Click the buttons to navigate through conversations.

### Types of Conversations:
1. Show only the selectable conversations (#0-6).
2. Show up to 30 conversations. Conversations beyond this cannot be selected for messages but can still open profiles.
3. Show only the first two conversations (useful for scenarios with minimal data).
4. Show only the first four conversations.
5. Show no conversations.

- Conversations are not sorted because the SQL server should return the array in the correct order.
- Profiles are loaded using the user's ID, and images are redownloaded to reflect changes online.
- JSON for user data is stored in `Resources/UserData/*UserID*`. This design accommodates additional user variables (e.g., last time online, gender, location).

### Messages:
- Messages and conversations in the UI populate from bottom to top.
- JSON for messages is stored in `Resources/Messages/*ConversationID*/`, with files organized by "Recent" and indexed message IDs:
  - The "Recent" file stores the most recent messages.
  - Older messages are fetched by subtracting one from the lowest index in the "Recent" file.
- Each JSON contains:
  - `MainUserID`
  - `OthersID`
  - A boolean (`hasMoreMessage`) to indicate whether additional messages exist.
  - An array of messages, where:
    - `-1` is the main user.
    - Positive indices correspond to other participants.

### Types of Messages:
- `000000`: One full page of 50 messages.
- `000001`: Split into two parts (25 messages each).
- `000002`: Only the second part; if it fails to load more messages, an error is shown.
- `000003`: Split into two parts, but the second part contains an empty array.
- `000004`: Contains 300 messages loaded 50 at a time.
- `000005`: Contains only two messages (demonstrating bottom-to-top rendering).
- `000006`: Contains zero messages.
- `000007`: Starts at message 250-300, followed by 225-249.

- Conversations show the last message but do not always match the message view, as the SQL server is expected to return the last `DateTime` for each conversation.

---

## Future Improvements
- Reload data when returning to the conversation list to reflect server changes.
- Add a loading icon during data retrieval.
- Optimize UI performance by toggling rendering for inactive items.
- Fix the mouse wheel bug (slow and inverted).
- Display timestamps for messages (already included in JSON).
- Implement drag-to-refresh for loading more messages.
- Add a button to view the main user's profile.
- Support multiple users in the conversation UI icons.
- Adjust image size ratio for downloaded textures.
- Implement message looping (e.g., cycling indices to clean up server data).
- Refactor code to improve object hierarchy and parent-child class relationships.
