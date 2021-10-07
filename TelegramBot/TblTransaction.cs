using System;
using System.Collections.Generic;

#nullable disable

namespace TelegramBot
{
    public partial class TblTransaction
    {
        public int IdCheck { get; set; }
        public Guid IdTransaction { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserChatId { get; set; }
        public string UserPhoneNumber { get; set; }
        public string Messenger { get; set; }
        public string CheckFn { get; set; }
        public string CheckNum { get; set; }
        public DateTime? CheckDate { get; set; }
        public decimal? CheckSumTotal { get; set; }
        public int? CheckCountSku { get; set; }
        public decimal? Discount { get; set; }
    }
}
