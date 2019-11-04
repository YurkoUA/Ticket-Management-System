using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Enums;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface IPackageService
    {
        int TotalCount { get; }
        PackageCountDTO GetCount();

        IEnumerable<PackageDTO> FindByName(string name);

        IEnumerable<PackageDTO> GetPackages();
        IEnumerable<PackageDTO> GetPackages(int skip, int take);
        IEnumerable<PackageDTO> GetPackages(PackagesFilter filter);
        IEnumerable<PackageDTO> GetPackages(int skip, int take, PackagesFilter filter);

        IEnumerable<PackageDTO> GetPackagesByColor(int colorId);
        IEnumerable<PackageDTO> GetPackagesBySerial(int serialId);

        IEnumerable<TicketDTO> GetPackageTickets(int packageId, bool orderByNumber = false);
        IEnumerable<PackageDTO> GetCompatiblePackages(int colorId, int serialId, int? number = null);

        IEnumerable<PackageDTO> Filter(PackageFilterDTO filter);

        PackageDTO GetPackage(int id);

        PackageEditDTO GetPackageEdit(int id);
        PackageSpecialEditDTO GetSpecialPackageEdit(int id);
        
        void Remove(int id);

        void OpenPackage(int id);
        void ClosePackage(int id);

        void MakeSpecial(PackageMakeSpecialDTO dto);
        void MakeDefault(PackageMakeDefaultDTO dto);

        bool ExistsById(int id);
        bool ExistsByName(string name);
        bool IsNameFree(int id, string name);

        int OpenedCount();
        int SpecialCount();
    }
}
