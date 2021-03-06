﻿using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.Web
{
    public class SerialCreateModel
    {
        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Необхідно вказати назву серії.")]
        [RegularExpression(@"[А-Я]{4}", ErrorMessage = "Назва серії може містити чотири великі літери українського алфавіту.")]
        public string Name { get; set; }

        [Display(Name = "Примітка")]
        [DataType(DataType.MultilineText)]
        [StringLength(128, ErrorMessage = "Довжина не може перевищувати 128 символів.")]
        public string Note { get; set; }
    }
}