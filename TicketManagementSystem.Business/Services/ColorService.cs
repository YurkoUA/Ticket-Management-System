using System;
using TicketManagementSystem.Data.Models;

namespace TicketManagementSystem.Business.Services
{
    public class ColorService : Service<Color>
    {
        public ColorService()
        {
            _repo = _uow.Colours;
        }

        #region Singleton implementation
        private static ColorService _serviceSingleton;

        public static ColorService GetInstance()
        {
            if (_serviceSingleton == null)
                _serviceSingleton = new ColorService();

            return _serviceSingleton;
        }
        #endregion

        public bool CanBeEdited(int id, string name)
        {
            return !Repository.Contains(m => m.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && m.Id != id);
        }

        public Color CreateColor(string name, byte[] rowVersion)
        {
            return _repo.Create(new Color { Name = name, RowVersion = rowVersion });
        }

        public void EditColor(int id, string name, byte[] rowVersion)
        {
            var color = _repo.GetById(id);
            color.Name = name;
            color.RowVersion = rowVersion;
            _repo.Update(color);
        }

        public void RemoveColor(int id)
        {
            _repo.Remove(_repo.GetById(id));
        }
    }
}
