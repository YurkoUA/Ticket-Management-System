using System;
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

        public bool CanBeEdited(int id, string name)
        {
            return !Repository.Contains(m => m.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && m.Id != id);
        }

        public Serial CreateSerial(string name, string note, byte[] rowVersion)
        {
            return _repo.Create(new Serial { Name = name, Note = note, RowVersion = rowVersion });
        }

        public void EditSerial(int id, string name, string note, byte[] rowVersion)
        {
            var serial = _repo.GetById(id);
            serial.Name = name;
            serial.Note = note;
            serial.RowVersion = rowVersion;
            _repo.Update(serial);
        }

        public void RemoveSerial(int id)
        {
            _repo.Remove(_repo.GetById(id));
        }
    }
}
