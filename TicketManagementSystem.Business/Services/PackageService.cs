using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        private IColorService _colorService;
        private ISerialService _serialService;

        public PackageService(IUnitOfWork database, IColorService colorService, ISerialService serialService) : base(database)
        {
            _colorService = colorService;
            _serialService = serialService;
        }

        public int TotalCount => Database.Packages.GetCount();

        public IEnumerable<PackageDTO> FindByName(string name)
        {
            return MapperInstance.Map<IEnumerable<PackageDTO>>(Database.Packages.GetAll(p => p.ToString().Contains(name)));
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
                .AsEnumerable()
                .Skip(skip)
                .Take(take);
            return MapperInstance.Map<IEnumerable<PackageDTO>>(packages);
        }

        public IEnumerable<PackageDTO> GetPackages(PackagesFilter filter)
        {
            var packages = Database.Packages.GetAll(GetExpressionByFilter(filter))
                .AsEnumerable()
                .OrderByDescending(p => p.IsOpened)
                .ThenByDescending(p => p.IsSpecial)
                .ThenByDescending(p => p.Id)
                .AsEnumerable();

            return MapperInstance.Map<IEnumerable<PackageDTO>>(packages);
        }

        public IEnumerable<PackageDTO> GetPackages(int skip, int take, PackagesFilter filter)
        {
            var packages = Database.Packages.GetAll(GetExpressionByFilter(filter))
                .AsEnumerable()
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
            var package = Database.Packages.GetById(packageId);

            if (package == null) return null;

            var tickets = package.Tickets.AsEnumerable();

            if (orderByNumber)
                tickets = tickets.OrderBy(t => t.Number);

            return MapperInstance.Map<IEnumerable<TicketDTO>>(tickets);
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
                    //Database.ExecuteSql("UPDATE [Packages] SET [SerialId] = @serial, [ColorId] = @color WHERE [Id] = @id", 
                    //    new SqlParameter("@color", packageDTO.ColorId),
                    //    new SqlParameter("@serial", packageDTO.SerialId),
                    //    new SqlParameter("@id", packageDTO.Id));

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

                if (dto.ResetColor) package.ColorId = null;
                if (dto.ResetSerial) package.SerialId = null;

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

        #region Validation

        public IEnumerable<string> Validate(PackageCreateDTO createDTO)
        {
            var errors = new List<string>();
            errors.AddRange(ValidateObject(createDTO));

            if (!_colorService.ExistsById(createDTO.ColorId))
            {
                errors.Add($"Кольору ID: {createDTO.ColorId} не існує.");
            }

            if (!_serialService.ExistsById(createDTO.SerialId))
            {
                errors.Add($"Серії ID: {createDTO.SerialId} не існує.");
            }

            return errors;
        }

        public IEnumerable<string> Validate(PackageSpecialCreateDTO createDTO)
        {
            var errors = new List<string>();
            errors.AddRange(ValidateObject(createDTO));

            if (ExistsByName(createDTO.Name))
            {
                errors.Add($"Пачка \"{createDTO.Name}\" вже існує.");
            }

            if (createDTO.ColorId != null && !_colorService.ExistsById((int)createDTO.ColorId))
            {
                errors.Add($"Кольору ID: {createDTO.ColorId} не існує.");
            }

            if (createDTO.SerialId != null && !_serialService.ExistsById((int)createDTO.SerialId))
            {
                errors.Add($"Серії ID: {createDTO.SerialId} не існує.");
            }

            return errors;
        }

        public IEnumerable<string> Validate(PackageEditDTO editDTO)
        {
            var errors = new List<string>();
            errors.AddRange(ValidateObject(editDTO));
            var package = GetPackage(editDTO.Id);

            if (!_colorService.ExistsById(editDTO.ColorId))
            {
                errors.Add($"Кольору ID: {editDTO.ColorId} не існує.");
            }

            if (!_serialService.ExistsById(editDTO.SerialId))
            {
                errors.Add($"Серії ID: {editDTO.SerialId} не існує.");
            }

            if (package.TicketsCount > 0 && editDTO.FirstNumber != null)
            {
                var tickets = GetPackageTickets(editDTO.Id).ToList();

                if (!tickets.TrueForAll(t => int.Parse(t.Number.First().ToString()) == editDTO.FirstNumber))
                {
                    errors.Add("Для цієї пачки неможливо встановити першу цифру.");
                }
            }

            return errors;
        }

        public IEnumerable<string> Validate(PackageSpecialEditDTO editDTO)
        {
            var errors = new List<string>();
            errors.AddRange(ValidateObject(editDTO));
            var package = GetPackage(editDTO.Id);

            if (!IsNameFree(editDTO.Id, editDTO.Name))
            {
                errors.Add($"Пачка \"{editDTO.Name}\" вже існує.");
            }

            if (editDTO.ColorId != null && !_colorService.ExistsById((int)editDTO.ColorId))
            {
                errors.Add($"Кольору ID: {editDTO.ColorId} не існує.");
            }

            if (editDTO.SerialId != null && !_serialService.ExistsById((int)editDTO.SerialId))
            {
                errors.Add($"Серії ID: {editDTO.SerialId} не існує.");
            }

            if (package.TicketsCount > 0)
            {
                var tickets = GetPackageTickets(editDTO.Id).ToList();

                if (!tickets.TrueForAll(t => t.ColorId == editDTO.ColorId) && editDTO.ColorId != null)
                {
                    errors.Add("Для цієї пачки неможливо встановити єдиний колір.");
                }

                if (!tickets.TrueForAll(t => t.SerialId == editDTO.SerialId) && editDTO.SerialId != null)
                {
                    errors.Add("Для цієї пачки неможливо встановити єдину серію.");
                }
            }

            return errors;
        }

        public IEnumerable<string> Validate(PackageMakeDefaultDTO dto)
        {
            var errors = new List<string>();
            errors.AddRange(ValidateObject(dto));

            var package = GetPackage(dto.Id);

            if (!package.IsSpecial)
            {
                errors.Add("Пачка й так звичайна");
                return errors;
            }

            var tickets = GetPackageTickets(dto.Id).ToList();

            if (!_colorService.ExistsById(dto.ColorId))
            {
                errors.Add($"Кольору ID: {dto.ColorId} не існує.");
            }

            if (!_serialService.ExistsById(dto.SerialId))
            {
                errors.Add($"Серії ID: {dto.SerialId} не існує.");
            }

            if (tickets.Any())
            {
                if (dto.FirstNumber != null && !tickets.TrueForAll(t => int.Parse(t.Number.First().ToString()) == dto.FirstNumber))
                {
                    errors.Add("Для цієї пачки неможливо встановити першу цифру.");
                }

                if (!tickets.TrueForAll(t => t.ColorId == dto.ColorId))
                {
                    errors.Add("Для цієї пачки неможливо встановити серію.");
                }

                if (!tickets.TrueForAll(t => t.SerialId == dto.SerialId))
                {
                    errors.Add("Для цієї пачки неможливо встановити колір.");
                }
            }

            return errors;
        }

        public IEnumerable<string> Validate(PackageMakeSpecialDTO dto)
        {
            var errors = new List<string>();
            errors.AddRange(ValidateObject(dto));

            var package = GetPackage(dto.Id);

            if (package.IsSpecial)
            {
                errors.Add("Пачка й так спеціальна");
                return errors;
            }

            if (ExistsByName(dto.Name))
                errors.Add($"Пачка \"{dto.Name}\" вже існує.");

            return errors;
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
