using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Enums;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface IPackageService
    {
        int TotalCount { get; }

        IEnumerable<PackageDTO> FindByName(string name);

        IEnumerable<PackageDTO> GetPackages();
        IEnumerable<PackageDTO> GetPackages(int skip, int take);
        IEnumerable<PackageDTO> GetPackages(PackagesFilter filter);
        IEnumerable<PackageDTO> GetPackages(int skip, int take, PackagesFilter filter);

        IEnumerable<PackageDTO> GetPackagesByColor(int colorId);
        IEnumerable<PackageDTO> GetPackagesBySerial(int serialId);

        IEnumerable<TicketDTO> GetPackageTickets(int packageId, bool orderByNumber = false);

        PackageDTO GetPackage(int id);

        PackageDTO CreatePackage(PackageCreateDTO packageDTO);
        PackageDTO CreateSpecialPackage(PackageSpecialCreateDTO packageDTO);

        PackageEditDTO GetPackageEdit(int id);
        PackageSpecialEditDTO GetSpecialPackageEdit(int id);

        PackageDTO EditPackage(PackageEditDTO packageDTO);
        PackageDTO EditSpecialPackage(PackageSpecialEditDTO packageDTO);
        
        void Remove(int id);

        void OpenPackage(int id);
        void ClosePackage(int id);

        PackageDTO MakeSpecial(PackageMakeSpecialDTO dto);
        PackageDTO MakeDefault(PackageMakeDefaultDTO dto);

        bool ExistsById(int id);
        bool ExistsByName(string name);
        bool IsNameFree(int id, string name);

        int OpenedCount();
        int SpecialCount();

        IEnumerable<string> Validate(PackageCreateDTO createDTO);
        IEnumerable<string> Validate(PackageSpecialCreateDTO createDTO);
        IEnumerable<string> Validate(PackageEditDTO editDTO);
        IEnumerable<string> Validate(PackageSpecialEditDTO editDTO);
        IEnumerable<string> Validate(PackageMakeDefaultDTO dto);
        IEnumerable<string> Validate(PackageMakeSpecialDTO dto);
    }
}
