using HelperTelegramBot.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperTelegramBot.Repositories
{
    public interface IUserRepository
    {
        User GetUserByChatId(int chatId);
    }
}
