using HelperTelegramBot.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperTelegramBot.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public User GetUserByChatId(int chatId)
        {
            return _context.Users.FirstOrDefault(u => u.ChatId == chatId);
        }
    }
}
