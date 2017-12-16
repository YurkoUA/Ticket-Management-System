using System;
using System.Collections.Generic;
using System.Linq;
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
            var packages = Database.Packages.GetAll(p => p.ToString().Contains(name));
            return MapperInstance.Map<IEnumerable<PackageDTO>>(packages);
        }

        public IEnumerable<PackageDTO> GetPackages()
        {
            var packages = Database.Packages.GetAll()
                .OrderByDescending(p => p.IsOpened)
                .ThenByDescending(p => p.IsSpecial)
                .ThenByDescending(p => p.Id);

            return MapperInstance.Map<IEnumerable<PackageDTO>>(packages);
        }

        public IEnumerable<PackageDTO> GetPackages(int skip, int take)
        {
            var packages = Database.Packages.GetAll()
                .OrderByDescending(p => p.IsOpened)
                .ThenByDescending(p => p.IsSpecial)
                .ThenByDescending(p => p.Id)
                //.AsEnumerable()
                .Skip(skip)
                .Take(take);

            return MapperInstance.Map<IEnumerable<PackageDTO>>(packages);
        }

        public IEnumerable<PackageDTO> GetPackages(PackagesFilter filter)
        {
            var packages = Database.Packages.GetAll(GetExpressionByFilter(filter))
                //.AsEnumerable()
                .OrderByDescending(p => p.IsOpened)
                .ThenByDescending(p => p.IsSpecial)
                .ThenByDescending(p => p.Id)
                .AsEnumerable();

            return MapperInstance.Map<IEnumerable<PackageDTO>>(packages);
        }

        public IEnumerable<PackageDTO> GetPackages(int skip, int take, PackagesFilter filter)
        {
            var packages = Database.Packages.GetAll(GetExpressionByFilter(filter))
                //.AsEnumerable()
                .OrderByDescending(p => p.IsOpened)
                .ThenByDescending(p => p.IsSpecial)
                .ThenByDescending(p => p.Id)
                .Skip(skip)
                .Take(take);

            return MapperInstance.Map<IEnumerable<PackageDTO>>(packages);
        }

        public IEnumerable<PackageDTO> GetPackagesByColor(int colorId)
        {
            var packages = Database.Packages.GetAll(p => p.ColorId == colorId);
            return MapperInstance.Map<IEnumerable<PackageDTO>>(packages);
        }

        public IEnumerable<PackageDTO> GetPackagesBySerial(int serialId)
        {
            var packages = Database.Packages.GetAll(p => p.SerialId == serialId);
            return MapperInstance.Map<IEnumerable<PackageDTO>>(packages);
        }

        public IEnumerable<TicketDTO> GetPackageTickets(int packageId, bool orderByNumber = false)
        {
            var tickets = Database.Tickets.GetAll(t => t.PackageId == packageId);

            if (!tickets.Any())
                return null;

            if (orderByNumber)
                tickets = tickets.OrderBy(t => t.Number);

            return MapperInstance.Map<IEnumerable<TicketDTO>>(tickets);
        }

        public IEnumerable<PackageDTO> GetCompatiblePackages(int colorId, int serialId, int? number = null)
        {
            var packages = Database.Packages.GetAll(p => (p.ColorId == null || p.ColorId == colorId)
                    && (p.SerialId == null || p.SerialId == serialId)
                    && p.IsOpened);

            if (number != null)
            {
                packages = packages.Where(p => p.FirstNumber == number || p.FirstNumber == null);
            }

            if (!packages.Any())
                return null;

            return MapperInstance.Map<IEnumerable<PackageDTO>>(packages);
        }

        public IEnumerable<PackageDTO> Filter(PackageFilterDTO filter)
        {
            IQueryable<Package> packages = Database.Packages.GetAll();

            if (filter.ColorId != null)
                packages = packages.Where(p => p.ColorId == filter.ColorId);

            if (filter.SerialId != null)
                packages = packages.Where(p => p.SerialId == filter.SerialId);

            if (filter.FirstNumber != null)
                packages = packages.Where(p => p.FirstNumber == filter.FirstNumber);

            if (filter.Status != PackageStatusFilter.None)
                packages = packages.Where(p => p.IsSpecial == (filter.Status == PackageStatusFilter.Special));

            return MapperInstance.Map<IEnumerable<PackageDTO>>(
                packages.OrderBy(p => p.Id).ThenBy(p => p.IsSpecial)
            );
        }

        public PackageDTO GetPackage(int id)
        {
            var package = Database.Packages.GetById(id);

            if (package == null)
                return null;

            return MapperInstance.Map<PackageDTO>(package);
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

                return MapperInstance.Map<PackageDTO>(package);
            }
            return null;
        }

        public PackageDTO EditSpecialPackage(PackageSpecialEditDTO packageDTO)
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

                return MapperInstance.Map<PackageDTO>(package);
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

        public PackageDTO MakeSpecial(PackageMakeSpecialDTO dto)
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

                return MapperInstance.Map<PackageDTO>(package);
            }
            return null;
        }

        public PackageDTO MakeDefault(PackageMakeDefaultDTO dto)
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

                return MapperInstance.Map<PackageDTO>(package);
            }
            return null;
        }

        #endregion

        #region Exists

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
            return !Database.Packages
                .Contains(p => p.Name?.Equals(name, StringComparison.CurrentCultureIgnoreCase) == true && p.Id != id);
        }

        #endregion

        #region Count

        public int OpenedCount()
        {
            return Database.Packages.GetCount(p => p.IsOpened);
        }

        public int SpecialCount()
        {
            return Database.Packages.GetCount(p => p.IsSpecial);
        }

        #endregion

        private Func<Package, bool> GetExpressionByFilter(PackagesFilter filter)
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
