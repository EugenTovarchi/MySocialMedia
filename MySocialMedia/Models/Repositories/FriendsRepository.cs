using Microsoft.EntityFrameworkCore;
using MySocialMedia.Models.UoW;
using MySocialMedia.Models.Users;
using System.Collections.Immutable;

namespace MySocialMedia.Models.Repositories;

public class FriendsRepository : Repository<Friend>
{
    public FriendsRepository(ApplicationDbContext db) : base(db)
    {

    }

    public async Task AddFriend(User target, User Friend)
    {
        var friends = Set.AsEnumerable().FirstOrDefault(x => x.UserId == target.Id && x.CurrentFriendId == Friend.Id);

        if (friends == null)
        {
            var item = new Friend()
            {
                UserId = target.Id,
                User = target,
                CurrentFriend = Friend,
                CurrentFriendId = Friend.Id,
            };

            await Create(item);
        }
    }

    public async Task <List<User>> GetFriendsByUser(User target)
    {
        var friends = await Set
            .Include(x => x.CurrentFriend).Include(x => x.User)
            .Where(x => x.User.Id == target.Id)
            .Select(x => x.CurrentFriend)
            .ToListAsync();

        return  friends;
    }

    public async Task DeleteFriend(User target, User Friend)
    {
        var friends = Set.AsEnumerable().FirstOrDefault(x => x.UserId == target.Id && x.CurrentFriendId == Friend.Id);

        if (friends != null)
        {
            await Delete(friends);
        }
    }
}
