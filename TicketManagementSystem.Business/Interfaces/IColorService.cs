using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface IColorService
    {
        IEnumerable<ColorDTO> GetColors();
        ColorDTO GetColor(int id);
        ColorEditDTO GetColorEdit(int id);
        ColorDTO Create(ColorCreateDTO colorDTO);
        void Edit(ColorEditDTO colorDTO);
        void Remove(int id);
        bool ExistsById(int id);
        bool ExistsByName(string name);
        bool IsNameFree(int id, string name);
    }
}