using EjercicioModulo3Clase2.Domain.Entities;
using EjercicioModulo3Clase2.Repository;
using EjercicioModulo3Clase2.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading;


namespace EjercicioModulo3Clase2.Services
{
    public class TasksService : ITasksService
    {

        private readonly DBContext _context;

        public TasksService(DBContext context)
        {
            _context = context;
        }

        public List<Tasks> GetTasks()
        {
            return _context.Tasks.ToList();
        }

        public Tasks GetTaskById(int id)
        {
            return _context.Tasks.FirstOrDefault(f => f.Id == id);
        }

        public List<Tasks> GetTasksByStatus(bool isCompleted, bool isActive)
        {
            return _context.Tasks.Where(w => w.IsCompleted == isCompleted && w.IsActive == isActive).ToList();
        }

        public List<Tasks> GetActiveTasks()
        {
            return _context.Tasks.Where(w => w.IsActive == true).ToList();
        }

        public List<Tasks> GetCompletedTasks()
        {
            return _context.Tasks.Where(w => w.IsCompleted == true).ToList();
        }

        public Tasks CompleteTaskById(int id)
        {
            var tarea = _context.Tasks.FirstOrDefault(f => f.Id == id);
            tarea.IsCompleted = true;
            _context.SaveChanges();
            return (tarea);
        }

        public Tasks DesactiveTaskById(int id)
        {
            var tarea = _context.Tasks.FirstOrDefault(f => f.Id == id);
            tarea.IsActive = false;
            _context.SaveChanges();
            return (tarea);
        }

        public Tasks AddTask(Tasks tarea)
        {
            _context.Add(tarea);
            _context.SaveChanges();
            return(tarea);
        }

    }

}
