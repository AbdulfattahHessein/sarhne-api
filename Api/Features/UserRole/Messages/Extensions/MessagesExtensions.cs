using Core.Entities;

namespace Api.Features.UserRole.Messages.Extensions;

public static class MessagesExtensions
{
    extension(IQueryable<Message> messages)
    {
        public IQueryable<Message> FilterByUser(Guid userId)
        {
            return messages.Where(m => m.Sender.Id == userId || m.Receiver.Id == userId);
        }
    }
}