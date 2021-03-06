﻿using System;
using System.Collections.Generic;
using System.Linq;
using TicketManagementSystem.Business.DTO;
using TicketManagementSystem.Business.Interfaces;
using TicketManagementSystem.Data.EF.Enums;
using TicketManagementSystem.Data.EF.Interfaces;
using TicketManagementSystem.Data.EF.Models;

namespace TicketManagementSystem.Business.Services
{
    public class TodoService : Service, ITodoService
    {
        public TodoService(IUnitOfWork database) : base(database)
        {
        }

        public IEnumerable<TodoTaskDTO> GetTasks()
        {
            var tasks = Database.Tasks.GetAll()
                .OrderByDescending(t => t.Priority)
                .ThenByDescending(t => t.Date)
                .AsEnumerable();

            return Mapper.Map<IEnumerable<TodoTaskDTO>>(tasks);
        }

        public IEnumerable<TodoTaskDTO> GetTasks(TaskStatus status)
        {
            var tasks = Database.Tasks.GetAll(t => t.Status == status)
                .OrderByDescending(t => t.Priority)
                .ThenByDescending(t => t.Date)
                .AsEnumerable();

            return Mapper.Map<IEnumerable<TodoTaskDTO>>(tasks);
        }

        public Dictionary<TaskStatus, IEnumerable<TodoTaskDTO>> GetTasksGroupByStatus()
        {
            var tasks = Database.Tasks.GetAll()
                .AsEnumerable()
                .OrderByDescending(t => t.Priority)
                .GroupBy(t => t.Status)
                .ToDictionary(g => g.Key, g => Mapper.Map<IEnumerable<TodoTaskDTO>>(
                    g.AsEnumerable()));

            return tasks;
        }

        public Dictionary<TaskStatus, IEnumerable<TodoTaskDTO>> GetTasksGroupByStatus(int take)
        {
            if (take < 1)
                throw new ArgumentException("take");

            var tasks = Database.Tasks.GetAll()
                .AsEnumerable()
                .OrderByDescending(t => t.Priority)
                .GroupBy(t => t.Status)
                .ToDictionary(g => g.Key, g => Mapper.Map<IEnumerable<TodoTaskDTO>>(
                    g.Take(take).AsEnumerable()));

            return tasks;
        }

        public TodoTaskDTO GetById(int id)
        {
            var task = Database.Tasks.GetById(id);

            if (task == null)
                return null;

            return Mapper.Map<TodoTaskDTO>(task);
        }

        public TodoTaskDTO Create(TodoTaskDTO taskDTO)
        {
            var task = Database.Tasks.Create(
                    Mapper.Map<TodoTask>(taskDTO));

            Database.SaveChanges();

            return Mapper.Map<TodoTaskDTO>(task);
        }

        public void Update(TodoTaskDTO taskDTO)
        {
            var task = Database.Tasks.GetById(taskDTO.Id);

            if (task != null)
            {
                task.Title = taskDTO.Title;
                task.Description = taskDTO.Description;

                Database.Tasks.Update(task);
                Database.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var task = Database.Tasks.GetById(id);

            if (task != null)
            {
                Database.Tasks.Remove(task);
                Database.SaveChanges();
            }
        }

        public void SetStatus(int id, TaskStatus status)
        {
            var task = Database.Tasks.GetById(id);

            if (task != null)
            {
                task.Status = status;

                Database.Tasks.Update(task);
                Database.SaveChanges();
            }
        }

        public void SetPriority(int id, TaskPriority priority)
        {
            var task = Database.Tasks.GetById(id);

            if (task != null)
            {
                task.Priority = priority;

                Database.Tasks.Update(task);
                Database.SaveChanges();
            }
        }

        public bool ExistsByTitle(string title)
        {
            return Database.Tasks
                .Any(t => t.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase));
        }

        public bool IsTitleFree(int id, string title)
        {
            return !Database.Tasks
                .Any(t => t.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase) && t.Id != id);
        }
    }
}
