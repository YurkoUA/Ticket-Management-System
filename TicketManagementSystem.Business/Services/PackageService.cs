using System;
using System.Collections.Generic;
using System.Linq;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Interfaces;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Business.Services
{
    public class PackageService : Service, IPackageService
    {
        public PackageService(IUnitOfWork database) : base(database)
        {
        }

        public int TotalCount => Database.Packages.GetCount();

        public IEnumerable<PackageDTO> GetPackages(int skip, int take)
        {
            var packages = Database.Packages.GetAll().AsEnumerable().Skip(skip).Take(take);
            return MapperInstance.Map<IEnumerable<PackageDTO>>(packages);
        }

        public PackageDTO GetPackage(int id)
        {
            var package = Database.Packages.GetById(id);

            if (package == null)
                return null;

            return MapperInstance.Map<PackageDTO>(package);
        }

        public PackageDTO CreatePackage(PackageCreateDTO packageDTO)
        {
            var package = Database.Packages.Create(MapperInstance.Map<Package>(packageDTO));
            Database.SaveChanges();

            return MapperInstance.Map<PackageDTO>(package);
        }

        public PackageDTO CreateSpecialPackage(PackageSpecialCreateDTO packageDTO)
        {
            var package = MapperInstance.Map<Package>(packageDTO);
            package.IsSpecial = true;
            package = Database.Packages.Create(package);
            Database.SaveChanges();

            return MapperInstance.Map<PackageDTO>(package);
        }

        public PackageEditDTO GetPackageEdit(int id)
        {
            var package = Database.Packages.GetById(id);

            if (package == null)
                return null;

            return MapperInstance.Map<PackageEditDTO>(package);
        }

        public PackageSpecialEditDTO GetSpecialPackageEdit(int id)
        {
            var package = Database.Packages.GetById(id);

            if (package == null)
                return null;

            return MapperInstance.Map<PackageSpecialEditDTO>(package);
        }

        public PackageDTO EditPackage(PackageEditDTO packageDTO)
        {
            var package = Database.Packages.GetById(packageDTO.Id);

            if (package != null && package?.IsSpecial == false)
            {
                package.ColorId = packageDTO.ColorId;
                package.SerialId = packageDTO.SerialId;
                package.FirstNumber = packageDTO.FirstNumber;
                package.Nominal = packageDTO.Nominal;
                package.Note = packageDTO.Note;

                package.RowVersion = packageDTO.RowVersion;

                Database.Packages.Update(package);
                Database.SaveChanges();

                return MapperInstance.Map<PackageDTO>(Database.Packages.Create(package));
            }
            return null;
        }

        public PackageDTO EditSpecialPackage(PackageSpecialEditDTO packageDTO)
        {
            var package = Database.Packages.GetById(packageDTO.Id);

            if (package != null && package?.IsSpecial == true)
            {
                package.Name = packageDTO.Name;
                package.ColorId = packageDTO.ColorId;
                package.SerialId = packageDTO.SerialId;
                package.Nominal = packageDTO.Nominal;
                package.Note = packageDTO.Note;

                package.RowVersion = packageDTO.RowVersion;

                Database.Packages.Update(package);
                Database.SaveChanges();

                return MapperInstance.Map<PackageDTO>(Database.Packages.Create(package));
            }
            return null;
        }

        public void Remove(int id)
        {
            //var package = Database.Packages.GetById(id);

            // TODO: Remove this shit!

            // Hardly ever have to delete the pack, so you can leave the crutch as is.
            // ******** TRASH *** CRUTCH ************
            //package.IsSpecial = true;
            //package.Name = " ";
            //Database.Packages.Update(package);
            //Database.SaveChanges();
            // **************************************

            //Database.Packages.Remove(package);

            Database.Packages.Remove(id, "Packages");
            Database.SaveChanges();
        }

        public void OpenPackage(int id)
        {
            // TODO: To issue error if package is now opened.
            var package = Database.Packages.GetById(id);

            if (package != null)
            {
                package.IsOpened = true;
                Database.Packages.Update(package);
                Database.SaveChanges();
            }
        }

        public void ClosePackage(int id)
        {
            // TODO: To issue error if package is now closed.
            var package = Database.Packages.GetById(id);

            if (package != null)
            {
                package.IsOpened = false;
                Database.Packages.Update(package);
                Database.SaveChanges();
            }
        }

        public PackageDTO MakeSpecial(int id, string name)
        {
            // TODO: To issue error if package is now special.

            var package = Database.Packages.GetById(id);

            if (package != null)
            {
                package.IsSpecial = true;
                package.Name = name;
                Database.Packages.Update(package);
                Database.SaveChanges();

                return MapperInstance.Map<PackageDTO>(package);
            }
            return null;
        }

        public PackageDTO MakeDefault(int id, int colorId, int serialId, int? firstNumber)
        {
            // TODO: To issue error if package is now default.
            // TODO: To issue error if colorId or serialId is not exists.

            var package = Database.Packages.GetById(id);

            if (package != null)
            {
                package.IsSpecial = false;
                package.Name = null;
                package.ColorId = colorId;
                package.SerialId = serialId;

                Database.Packages.Update(package);
                Database.SaveChanges();

                return MapperInstance.Map<PackageDTO>(package);
            }
            return null;
        }

        public bool ExistsById(int id)
        {
            return Database.Packages.ExistsById(id);
        }

        public bool ExistsByName(string name)
        {
            return Database.Packages.Contains(p => p.Name?.Equals(name, StringComparison.CurrentCultureIgnoreCase) == true);
        }

        public bool IsNameFree(int id, string name)
        {
            return Database.Packages
                .Contains(p => p.Name?.Equals(name, StringComparison.CurrentCultureIgnoreCase) == true && p.Id != id);
        }
    }
}
