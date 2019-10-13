using System;
using System.Collections.Generic;
using System.Linq;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Interfaces;
using TicketManagementSystem.Data.Entities;

namespace TicketManagementSystem.Business.Services
{
    public class SerialService : Service, ISerialService
    {
        public SerialService(IUnitOfWork database) : base(database)
        {
        }

        public IEnumerable<SerialDTO> GetSeries()
        {
            return Database.Series.GetAll().Select(SerialDTO.CreateFromSerial);
        }

        public SerialDTO GetSerial(int id)
        {
            return Database.Series.GetAll(s => s.Id == id).Select(SerialDTO.CreateFromSerial)
                .SingleOrDefault();
        }

        public SerialEditDTO GetSerialEdit(int id)
        {
            return Database.Series.GetAll(s => s.Id == id).Select(SerialEditDTO.CreateFromSerial)
                .SingleOrDefault();
        }

        public SerialDTO Create(SerialCreateDTO serialDTO)
        {
            var serial = Database.Series.Create(Mapper.Map<Serial>(serialDTO));
            Database.SaveChanges();

            return Mapper.Map<SerialDTO>(serial);
        }

        public void Edit(SerialEditDTO serialDTO)
        {
            var serial = Database.Series.GetById(serialDTO.Id);
            serial.Name = serialDTO.Name;
            serial.Note = serialDTO.Note;
            Database.Series.Update(serial);
            Database.SaveChanges();
        }

        public void Remove(int id)
        {
            Database.Series.Remove(Database.Series.GetById(id));
            Database.SaveChanges();
        }

        public bool ExistsById(int id)
        {
            return Database.Series.ExistsById(id);
        }

        public bool ExistsByName(string name)
        {
            return Database.Series
                .Any(s => s.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public bool IsNameFree(int id, string name)
        {
            return !Database.Series
                .Any(m => m.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && m.Id != id);
        }
    }
}
