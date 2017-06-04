using System;
using System.Collections.Generic;
using AutoMapper;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Interfaces;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Business.Services
{
    public class SerialService : Service, ISerialService
    {
        public SerialService(IUnitOfWork database) : base(database)
        {
        }

        public IEnumerable<SerialDTO> GetSeries()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Serial, SerialDTO>()
                .ForMember(dest => dest.PackagesCount, opt => opt.MapFrom(src => src.Packages.Count))
                .ForMember(dest => dest.TicketsCount, opt => opt.MapFrom(src => src.Tickets.Count))
            );
            return Mapper.Map<IEnumerable<Serial>, IEnumerable<SerialDTO>>(Database.Series.GetAll());
        }

        public SerialDTO GetSerial(int id)
        {
            var serial = Database.Series.GetById(id);

            if (serial == null)
                return null;

            Mapper.Initialize(cfg => cfg.CreateMap<Serial, SerialDTO>()
                .ForMember(dest => dest.PackagesCount, opt => opt.MapFrom(src => src.Packages.Count))
                .ForMember(dest => dest.TicketsCount, opt => opt.MapFrom(src => src.Tickets.Count))
            );

            return Mapper.Map<Serial, SerialDTO>(serial);
        }

        public SerialEditDTO GetSerialEdit(int id)
        {
            var serial = Database.Series.GetById(id);

            if (serial == null)
                return null;

            return MapperInstance.Map<SerialEditDTO>(serial);
        }

        public SerialDTO Create(SerialCreateDTO serialDTO)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<SerialCreateDTO, Serial>());

            var serial = Database.Series.Create(Mapper.Map<SerialCreateDTO, Serial>(serialDTO));
            Database.SaveChanges();

            Mapper.Initialize(cfg => cfg.CreateMap<Serial, SerialDTO>()
                 .ForMember(dest => dest.PackagesCount, opt => opt.MapFrom(src => src.Packages.Count))
                 .ForMember(dest => dest.TicketsCount, opt => opt.MapFrom(src => src.Tickets.Count)));

            return Mapper.Map<Serial, SerialDTO>(serial);
        }

        public void Edit(SerialEditDTO serialDTO)
        {
            var serial = Database.Series.GetById(serialDTO.Id);
            serial.Name = serialDTO.Name;
            serial.Note = serialDTO.Note;
            serial.RowVersion = serialDTO.RowVersion;

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
                .Contains(s => s.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        public bool IsNameFree(int id, string name)
        {
            return !Database.Series
                .Contains(m => m.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && m.Id != id);
        }
    }
}
