﻿using System.ComponentModel.DataAnnotations;
using TicketManagementSystem.Data.Models;

namespace TicketManagementSystem.Web.ViewModels.Account
{
    public class AccountIndexModel
    {
        [Display(Name = "ID:")]
        public int Id { get; set; }

        [Display(Name = "Логін")]
        public string Login { get; set; }

        [Display(Name = "Ім'я:")]
        public string Name { get; set; }

        [Display(Name = "Група:")]
        public string Role { get; set; }
    }
}