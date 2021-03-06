﻿using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TicketManagementSystem.Business.Attributes;

namespace TicketManagementSystem.Web
{
    public class TicketEditModel
    {
        [HiddenInput(DisplayValue = false)]
        public byte[] RowVersion { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool CanSelectColor { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool CanSelectSerial { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Number { get; set; }

        [Display(Name = "Колір")]
        public int ColorId { get; set; }

        [Display(Name = "Серія")]
        public int SerialId { get; set; }

        [Display(Name = "Номер серії")]
        [Required(ErrorMessage = "Необхідно вказати номер серії.")]
        [SerialNumber(ErrorMessage = "Номер серії повинен бути від 01 до 50.")]
        public string SerialNumber { get; set; }

        [Display(Name = "Примітка")]
        [StringLength(128, ErrorMessage = "Примітка не може бути більшою за 128 символів.")]
        public string Note { get; set; }

        [Display(Name = "Дата")]
        [StringLength(32, ErrorMessage = "Дата не може бути довша за 32 символи.")]
        public string Date { get; set; }

        [HiddenInput(DisplayValue = false)]
        public SelectList Colors { get; set; }

        [HiddenInput(DisplayValue = false)]
        public SelectList Series { get; set; }

        [HiddenInput(DisplayValue = false)]
        public SelectList Packages { get; set; }
    }
}