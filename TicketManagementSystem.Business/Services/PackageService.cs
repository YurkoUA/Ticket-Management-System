using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Data.EF.Interfaces;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Business.Services
{
    // TODO: Extract interface.
    public class PackageService : Service
    {
        public PackageService(IUnitOfWork database) : base(database)
        {
        }

        // TODO: Make recieve PackageCreateDTO.
        public Package CreateDefaultPackage(int color, int serial, double nominal, int? firstNumber, string note, byte[] rowVersion)
        {
            var package = new Package
            {
                ColorId = color,
                SerialId = serial,
                Nominal = nominal,
                FirstNumber = firstNumber,
                Note = note,
                RowVersion = rowVersion
            };

            return Database.Packages.Create(package);
        }

        // TODO: Make recieve PackageEditDTO.
        public Package CreateSpecialPackage(string name, int color, int serial, double nominal, string note, byte[] rowVersion)
        {
            var package = new Package
            {
                IsSpecial = true,
                Name = name,
                ColorId = color,
                SerialId = serial,
                Nominal = nominal,
                Note = note,
                RowVersion = rowVersion
            };

            return Database.Packages.Create(package);
        }
    }
}
