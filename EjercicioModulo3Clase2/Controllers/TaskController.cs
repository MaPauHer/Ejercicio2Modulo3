using EjercicioModulo3Clase2.Domain.Entities;
using EjercicioModulo3Clase2.Domain.DTOs;
using EjercicioModulo3Clase2.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace EjercicioModulo3Clase2.Controllers
{
    //[Route("api/[controller]")]
    [Route("V1")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        #region Pasos previos
        /*
         * 1 - Instalar EF Core y EF Core SQL Server
         * 2 - Crear contexto de base de datos y los modelos. Se puede usar Ingenieria Inversa de EF Core Power Tools
         * 3 - Configurar la inyección de dependencia del contexto tanto en Program.cs como en el controlador. No olvidar el string de conexión.
         * 4 - Las rutas de los endpoints queda a criterio de cada uno.
         * 5 - En este controlador, realizar los siguientes ejercicios:
         */
        #endregion

        // Inyección Dependencias
        private ITasksService _tasksService;
        //private IConfiguration _configuration;

        public TaskController(ITasksService tasksService, IConfiguration configuration)
        {
            _tasksService = tasksService;
        //    _configuration = configuration;
        }

        //int paginaMin = int.Parse(builder.Configuration.GetSection("PagMin").Value);

        #region Ejercicio 1
        // Crear un endpoint para obtener un listado de todas las tareas usando HTTP GET
        [HttpGet("GetTasks")]
        public IActionResult GetTasks()
        {
            var tareas = _tasksService.GetTasks();
            return Ok(tareas);
        }
        #endregion

        #region Ejercicio 2
        //Crear un endpoint para obtener una tarea por ID usando HTTP GET
        [HttpGet("GetTask/{id}", Name = "GetTaskById")]
        public IActionResult GetTaskById([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest($"id={id} - El id tarea debe ser mayor a 0");
            }
            else
            {
                var tarea = _tasksService.GetTaskById(id); 
                if (tarea == null)
                {
                    return Ok($"id={id} - Tarea no encontrada");
                }
                else
                {
                    return Ok(tarea);
                }
            }
        }
        #endregion

        #region Ejercicio 3
        // Crear un endpoint para crear una nueva tarea usando HTTP POST
        [HttpPost("AddTask")]
        public IActionResult AddTask([FromBody] NewTaskDTO tarea)
        {

            DateTime fechaHoy = DateTime.Now.Date;
            string formatoFecha = "yyyy-MM-dd";
            DateTime fechaVto;
            bool feIVtoOK = DateTime.TryParseExact(tarea.DueDate, formatoFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaVto);

            if (feIVtoOK)
            {
                if (fechaVto < fechaHoy)
                {
                    return BadRequest($"Fecha vencimiento={tarea.DueDate} - La fecha de vencimiento debe ser mayor a hoy.");
                }
                else
                {
                    Tasks t = new Tasks()
                    {
                        Title = tarea.Title,
                        Description = String.IsNullOrWhiteSpace(tarea.Description) ? tarea.Title : tarea.Description,
                        DueDate = tarea.DueDate,
                        IsActive = tarea.IsActive,
                        IsCompleted = tarea.IsCompleted
                    };
                    return Ok(_tasksService.AddTask(t));
                }
            }
            else
            {
                return BadRequest($"Fecha vencimiento={tarea.DueDate} - La fecha de vencimiento ingresada no es valida(yyyy-MM-dd).");
            }
        }

        #endregion

        #region Ejercicio 4
        // Crear un endpoint para marcar una tarea como completada usando HTTP PUT
        [HttpPut("CompleteTask/{id}")]
        public IActionResult CompleteTaskById([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest($"id={id} - El id tarea debe ser mayor a 0");
            }
            else
            {
                var tarea = _tasksService.GetTaskById(id);
                if (tarea == null)
                {
                    return Ok($"id={id} - Tarea no encontrada");
                }
                else
                {
                    if (tarea.IsCompleted == true)
                    {
                        return BadRequest($"La tarea id={id} ya fue completada");
                    }
                    else
                    {
                        return Ok(_tasksService.CompleteTaskById(id));
                    }
                }
            }
        }

        #endregion

        #region Ejercicio 5
        // Crear un endpoint para dar de baja una tarea usando HTTP PUT (baja lógica)
        [HttpPut("DesactiveTask/{id}")]
        public IActionResult DesactiveTaskById([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest($"id={id} - El id tarea debe ser mayor a 0");
            }
            else
            {
                var tarea = _tasksService.GetTaskById(id);
                if (tarea == null)
                {
                    return Ok($"id={id} - Tarea no encontrada");
                }
                else
                {
                    if (tarea.IsActive == true)
                    {
                        return Ok(_tasksService.DesactiveTaskById(id));
                    }
                    else
                    {
                        return BadRequest($"La tarea id={id} ya fue desactivada");
                    }

                }
            }
        }
        #endregion

        #region Otras
        //Tareas activas
        [HttpGet("GetActiveTasks")]
        public IActionResult GetActiveTasks()
        {
            var tareasActivas = _tasksService.GetActiveTasks();
            if (tareasActivas.Count == 0)
            {
                return Ok("No existen tareas activas");
            }
            else
            {
                return Ok(tareasActivas);
            }
        }

        //Tareas completadas
        [HttpGet("GetCompletedTasks")]
        public IActionResult GetCompletedTasks()
        {
            var tareasCompletas = _tasksService.GetCompletedTasks();
            if (tareasCompletas.Count == 0)
            {
                return Ok("No existen tareas completas");
            }
            else
            {
                return Ok(tareasCompletas);
            }
        }

        //Tareas por estado
        [HttpGet("GetTasksByStatus")]
        public IActionResult GetTasksByStatus([FromQuery] bool isCompleted, [FromQuery] bool isActive)
        {
            var tareas = _tasksService.GetTasksByStatus(isCompleted, isActive);
            if (tareas.Count == 0)
            {
                return Ok("No existen tareas en los estados solicitados.");
            }
            else
            {
                return Ok(tareas);
            }
        }

        #endregion
    }
}
