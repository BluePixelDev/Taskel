using TaskelDB.Interfaces;

namespace TaskelDB.Models.Conversation
{
    /// <summary>
    /// A data model describbing a conversation.
    /// </summary>
    public class ConversationModel : IElement
    {
        public int ID { get; set; }
        public string Name { get; set; } = "";
        public DateTime Created_At { get; set; }
    }
}
