using TaskelDB.Interfaces;
using TaskelDB.Models.User;

namespace TaskelDB.Models.Conversation
{
    /// <summary>
    /// A data model for messages.
    /// </summary>
    public class MessageModel : IElement
    {
        public int ID { get; set; }
        public int Conversation_ID { get; set; }
        public int Sender_ID { get; set; }
        public string Body { get; set; } = "";
        public DateTime Created_At { get; set; }

        public ConversationModel? Conversation { get; set; }
        public UserModel? Sender { get; set; }
    }
}
