using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperTelegramBot.Models.Domain
{
    [Table("User")]
    public class User
    {
        [Key]
        public int ChatId { get; set; }
        public string Name { get; set; }
        public string TimeZone { get; set; }
    }
}
