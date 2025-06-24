using Microsoft.EntityFrameworkCore;
using MySocialMedia.Models.UoW;
using MySocialMedia.Models.Users;

namespace MySocialMedia.Models.Repositories;

public class MessageRepository : Repository<Message>
{
    public MessageRepository(ApplicationDbContext db) : base(db)
    {
    }

    public async Task<List<Message>> GetMessages(User sender, User recipient)
    {
          var query = Set
           .Include(x => x.Recipient)
           .Include(x => x.Sender)
           .Where(x => (x.SenderId == sender.Id && x.RecipientId == recipient.Id) ||
                      (x.SenderId == recipient.Id && x.RecipientId == sender.Id))
           .OrderBy(x => x.Id);  

        return await query.ToListAsync();
    }
}
