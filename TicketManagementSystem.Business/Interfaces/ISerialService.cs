using System.Collections.Generic;
using TicketManagementSystem.Business.DTO;

namespace TicketManagementSystem.Business.Interfaces
{
    public interface ISerialService
    {
        IEnumerable<SerialDTO> GetSeries();

        SerialDTO GetSerial(int id);
        SerialEditDTO GetSerialEdit(int id);
        SerialDTO Create(SerialCreateDTO serialDTO);

        void Edit(SerialEditDTO serialDTO);
        void Remove(int id);

        bool ExistsById(int id);
        bool ExistsByName(string name);
        bool IsNameFree(int id, string name);
    }
}