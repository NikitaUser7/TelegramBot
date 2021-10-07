using System;
using System.Collections.Generic;

#nullable disable

namespace TelegramBot
{
    public partial class TblCheckBonusesUser
    {
        public int Id { get; set; }
        public Guid IdTransaction { get; set; }
        public int IdSku { get; set; }
        public string NameInCheck { get; set; }
        public decimal CountProduct { get; set; }
        public decimal PriceSku { get; set; }
        public string UserPhoneNumber { get; set; }
        public decimal UserBonuses { get; set; }
        public DateTime AddBonusesDate { get; set; }
    }
}
