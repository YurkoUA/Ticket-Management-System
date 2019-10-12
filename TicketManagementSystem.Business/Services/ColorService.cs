using System;
using System.Collections.Generic;
using System.Linq;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Interfaces;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Business.Services
{
    public class ColorService : Service, IColorService
    {
        public ColorService(IUnitOfWork database) : base(database)
        {
        }

        public IEnumerable<ColorDTO> GetColors()
        {
            return Database.Colours.GetAll().Select(ColorDTO.CreateFromColor);
        }

        public ColorDTO GetColor(int id)
        {
            return Database.Colours.GetAll(c => c.Id == id).Select(ColorDTO.CreateFromColor)
                .SingleOrDefault();
        }

        public ColorEditDTO GetColorEdit(int id)
        {
            return Database.Colours.GetAll(c => c.Id == id).Select(ColorEditDTO.CreateFromColor)
                .SingleOrDefault();
        }

        public ColorDTO Create(ColorCreateDTO colorDTO)
        {
            var color = Database.Colours.Create(Mapper.Map<Color>(colorDTO));
            Database.SaveChanges();

            return Mapper.Map<ColorDTO>(color);
        }

        public void Edit(ColorEditDTO colorDTO)
        {
            var color = Database.Colours.GetById(colorDTO.Id);
            color.Name = colorDTO.Name;

            Database.Colours.Update(color);
            Database.SaveChanges();
        }

        public void Remove(int id)
        {
            Database.Colours.Remove(Database.Colours.GetById(id));
            Database.SaveChanges();
        }

        public bool ExistsById(int id)
        {
            return Database.Colours.ExistsById(id);
        }

        public bool ExistsByName(string name)
        {
            return Database.Colours
                .Any(c => c.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public bool IsNameFree(int id, string name)
        {
            return !Database.Colours
                .Any(m => m.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && m.Id != id);
        }
    }
}
