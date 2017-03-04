using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagementSystem.Data.Models;

namespace TicketManagementSystem.Business.Services
{
    public class SerialService : Service<Serial>
    {
        public SerialService()
        {
            _repo = _uow.Series;
        }

        #region Singleton implementation
        private static SerialService _serviceSingleton;

        public static SerialService GetInstance()
        {
            if (_serviceSingleton == null)
                _serviceSingleton = new SerialService();

            return _serviceSingleton;
        }
        #endregion

        public void CreateSerial(string name, string note, byte[] rowVersion)
        {
            _repo.Create(new Serial { Name = name, Note = note, RowVersion = rowVersion });
        }

        public void EditColor(int id, string name, string note, byte[] rowVersion)
        {
            var serial = _repo.GetById(id);
            serial.Name = name;
            serial.Note = note;
            serial.RowVersion = rowVersion;
            _repo.Update(serial);
        }

        public void RemoveColor(int id)
        {
            _repo.Remove(_repo.GetById(id));
        }
    }
}
