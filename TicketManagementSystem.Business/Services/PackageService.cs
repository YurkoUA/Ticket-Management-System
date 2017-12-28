using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Enums;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Interfaces;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Business.Services
{
    public class PackageService : Service, IPackageService
    {
        private readonly IColorService _colorService;
        private readonly ISerialService _serialService;

        public PackageService(IUnitOfWork database, IColorService colorService, ISerialService serialService) : base(database)
        {
            _colorService = colorService;
            _serialService = serialService;
        }

        public int TotalCount => Database.Packages.GetCount();

        public IEnumerable<PackageDTO> FindByName(string name)
        {
            var packages = Database.Packages.GetAllIncluding(p => p.Color, p => p.Serial, p => p.Tickets)
                .AsEnumerable()
                .Where(p => p.ToString().Contains(name));

            return MapperInstance.Map<IEnumerable<PackageDTO>>(packages);
        }

        public IEnumerable<PackageDTO> GetPackages()
        {
            return Database.Packages.GetAllIncluding(p => p.Color, p => p.Serial)
                .OrderByDescending(p => p.IsOpened)
                .ThenByDescending(p => p.IsSpecial)
                .ThenByDescending(p => p.Id)
                .Select(PackageDTO.CreateFromPackage);
        }

        public IEnumerable<PackageDTO> GetPackages(int skip, int take)
        {
            return Database.Packages.GetAllIncluding(p => p.Color, p => p.Serial)
                .OrderByDescending(p => p.IsOpened)
                .ThenByDescending(p => p.IsSpecial)
                .ThenByDescending(p => p.Id)
                .Skip(() => skip)
                .Take(() => take)
                .Select(PackageDTO.CreateFromPackage);
        }

        public IEnumerable<PackageDTO> GetPackages(PackagesFilter filter)
        {
            return Database.Packages.GetAllIncluding(GetExpressionByFilter(filter), p => p.Color, p => p.Serial)
                .OrderByDescending(p => p.IsOpened)
                .ThenByDescending(p => p.IsSpecial)
                .ThenByDescending(p => p.Id)
                .Select(PackageDTO.CreateFromPackage);
        }

        public IEnumerable<PackageDTO> GetPackages(int skip, int take, PackagesFilter filter)
        {
            return Database.Packages.GetAllIncluding(GetExpressionByFilter(filter), p => p.Color, p => p.Serial)
                .OrderByDescending(p => p.IsOpened)
                .ThenByDescending(p => p.IsSpecial)
                .ThenByDescending(p => p.Id)
                .Skip(() => skip)
                .Take(() => take)
                .Select(PackageDTO.CreateFromPackage);
        }

        public IEnumerable<PackageDTO> GetPackagesByColor(int colorId)
        {
            return Database.Packages.GetAllIncluding(p => p.ColorId == colorId, p => p.Color, p => p.Serial)
                .Select(PackageDTO.CreateFromPackage);
        }

        public IEnumerable<PackageDTO> GetPackagesBySerial(int serialId)
        {
            return Database.Packages.GetAllIncluding(p => p.SerialId == serialId, p => p.Color, p => p.Serial)
                .Select(PackageDTO.CreateFromPackage);
        }

        public IEnumerable<TicketDTO> GetPackageTickets(int packageId, bool orderByNumber = false)
        {
            var tickets = Database.Tickets.GetAllIncluding(t => t.PackageId == packageId, t => t.Color, t => t.Serial, t => t.Package, t => t.Package.Color, t => t.Package.Serial);

            if (!tickets.Any())
                return null;

            if (orderByNumber)
                tickets = tickets.OrderBy(t => t.Number);

            return MapperInstance.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<PackageDTO> GetCompatiblePackages(int colorId, int serialId, int? number = null)
        {
            Expression<Func<Package, bool>> filter = p => (p.ColorId == null || p.ColorId == colorId)
                    && (p.SerialId == null || p.SerialId == serialId)
                    && p.IsOpened;

            var packages = Database.Packages.GetAllIncluding(filter, p => p.Color, p => p.Serial);

            if (number != null)
            {
                packages = packages.Where(p => p.FirstNumber == number || p.FirstNumber == null);
            }

            if (!packages.Any())
                return null;

            return packages.Select(PackageDTO.CreateFromPackage);
        }

        public IEnumerable<PackageDTO> Filter(PackageFilterDTO filter)
        {
            IQueryable<Package> packages = Database.Packages.GetAllIncluding(p => p.Color, p => p.Serial, p => p.Tickets);

            if (filter.ColorId != null)
                packages = packages.Where(p => p.ColorId == filter.ColorId);

            if (filter.SerialId != null)
                packages = packages.Where(p => p.SerialId == filter.SerialId);

            if (filter.FirstNumber != null)
                packages = packages.Where(p => p.FirstNumber == filter.FirstNumber);

            if (filter.Status != PackageStatusFilter.None)
                packages = packages.Where(p => p.IsSpecial == (filter.Status == PackageStatusFilter.Special));

            packages = packages.OrderBy(p => p.Id).ThenBy(p => p.IsSpecial);

            return packages.Select(PackageDTO.CreateFromPackage);
        }

        public PackageDTO GetPackage(int id)
        {
            return Database.Packages.GetAllIncluding(p => p.Id == id, p => p.Color, p => p.Serial)
                .Select(PackageDTO.CreateFromPackage)
                .SingleOrDefault();
        }

        #region CRUD

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
            return Database.Packages.GetAllIncluding(p => p.Id == id, p => p.Color, p => p.Serial)
                .Select(PackageEditDTO.CreateFromPackage)
                .SingleOrDefault();
        }

        public PackageSpecialEditDTO GetSpecialPackageEdit(int id)
        {
            return Database.Packages.GetAllIncluding(p => p.Id == id, p => p.Color, p => p.Serial)
                .Select(PackageSpecialEditDTO.CreateFromPackage)
                .SingleOrDefault();
        }

        public void EditPackage(PackageEditDTO packageDTO)
        {
            var package = Database.Packages.GetById(packageDTO.Id);

            if (package?.IsSpecial == false)
            {
                package.ColorId = packageDTO.ColorId;
                package.SerialId = packageDTO.SerialId;
                package.FirstNumber = packageDTO.FirstNumber;
                package.Nominal = packageDTO.Nominal;
                package.Note = packageDTO.Note;

                if (packageDTO.RowVersion != null)
                    package.RowVersion = packageDTO.RowVersion;

                Database.Packages.Update(package);
                Database.SaveChanges(() => {
                    
                    Database.ExecuteSql("UPDATE Packages SET SerialId = {0}, ColorId = {1} WHERE Id = {2}",
                        packageDTO.SerialId, packageDTO.ColorId, packageDTO.Id);
                });
            }
        }

        public void EditSpecialPackage(PackageSpecialEditDTO packageDTO)
        {
            var package = Database.Packages.GetById(packageDTO.Id);

            if (package?.IsSpecial == true)
            {
                package.Name = packageDTO.Name;
                package.ColorId = packageDTO.ColorId;
                package.SerialId = packageDTO.SerialId;
                package.Nominal = packageDTO.Nominal;
                package.Note = packageDTO.Note;

                if (packageDTO.RowVersion != null)
                    package.RowVersion = packageDTO.RowVersion;

                Database.Packages.Update(package);
                Database.SaveChanges();
            }
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

        #endregion

        #region Open/Close, Make default/special.

        public void OpenPackage(int id)
        {
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
            var package = Database.Packages.GetById(id);

            if (package != null)
            {
                package.IsOpened = false;
                Database.Packages.Update(package);
                Database.SaveChanges();
            }
        }

        public void MakeSpecial(PackageMakeSpecialDTO dto)
        {
            var package = Database.Packages.GetById(dto.Id);

            if (package != null)
            {
                package.IsSpecial = true;
                package.Name = dto.Name;
                package.FirstNumber = null;

                if (dto.ResetColor)
                    package.ColorId = null;

                if (dto.ResetSerial)
                    package.SerialId = null;

                Database.Packages.Update(package);
                Database.SaveChanges();
            }
        }

        public void MakeDefault(PackageMakeDefaultDTO dto)
        {
            var package = Database.Packages.GetById(dto.Id);

            if (package != null)
            {
                package.IsSpecial = false;
                package.Name = null;
                package.ColorId = dto.ColorId;
                package.SerialId = dto.SerialId;
                package.FirstNumber = dto.FirstNumber;

                Database.Packages.Update(package);
                Database.SaveChanges();
            }
        }

        #endregion

        #region Exists

        public bool ExistsById(int id)
        {
            return Database.Packages.ExistsById(id);
        }

        public bool ExistsByName(string name)
        {
            return Database.Packages.Any(p => p.Name != null && p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) == true);
        }

        public bool IsNameFree(int id, string name)
        {
            return !Database.Packages
                .Any(p => p.Name != null && p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) == true && p.Id != id);
        }

        #endregion

        #region Count

        public int OpenedCount() => Database.Packages.GetCount(p => p.IsOpened);

        public int SpecialCount() => Database.Packages.GetCount(p => p.IsSpecial);

        #endregion

        private Expression<Func<Package, bool>> GetExpressionByFilter(PackagesFilter filter)
        {
            switch(filter)
            {
                case PackagesFilter.Opened:
                    return p => p.IsOpened;

                case PackagesFilter.Special:
                    return p => p.IsSpecial;

                default:
                    return p => true;
            }
        }
    }
}
