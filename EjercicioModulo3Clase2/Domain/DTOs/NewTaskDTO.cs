using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EjercicioModulo3Clase2.Domain.DTOs
{
    public class NewTaskDTO
    {
        
        [Required(ErrorMessage = "El título de la tarea es requerido.")]
        public string Title { get; set; }
        
        public string Description { get; set; }

        [Required(ErrorMessage = "La fecha de vencimiento de la tarea es requerida.")]
        public string DueDate { get; set; }

        public bool IsCompleted { get; } = false;
        
        public bool IsActive { get; } = true; 

    }
}
