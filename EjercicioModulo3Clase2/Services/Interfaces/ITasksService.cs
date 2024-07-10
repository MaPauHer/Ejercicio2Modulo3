using EjercicioModulo3Clase2.Domain.DTOs;
using EjercicioModulo3Clase2.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace EjercicioModulo3Clase2.Services.Interfaces
{
    public interface ITasksService
    {
        public List<Tasks> GetTasks();

        public List<Tasks> GetTasksByStatus(bool isCompleted, bool isActive);

        public List<Tasks> GetActiveTasks();

        public List<Tasks> GetCompletedTasks();

        public Tasks GetTaskById(int id);

        public Tasks CompleteTaskById(int id);

        public Tasks DesactiveTaskById(int id);

        public Tasks AddTask(Tasks tarea);
    }

}
