using EjercicioModulo3Clase2.Domain.DTOs;
using EjercicioModulo3Clase2.Domain.Entities;
using EjercicioModulo3Clase2.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using System.Globalization;

namespace EjercicioModulo3Clase2.Controllers
{
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
        private readonly DBContext _context;

        public TaskController(DBContext context)
        {
            _context = context;
        }


        #region Ejercicio 1
        // Crear un endpoint para obtener un listado de todas las tareas usando HTTP GET
        [HttpGet("GetTasks")]
        public IActionResult GetTasks()
        {
            return Ok(_context.Tasks.ToList());
        }
        #endregion

        #region Ejercicio 2
        // Crear un endpoint para obtener una tarea por ID usando HTTP GET
        [HttpGet("GetTask/{id}", Name = "GetTaskById")]
        public IActionResult GetTaskById([FromRoute] int id = 1)
        {
            if (id <= 0)
            {
                return BadRequest($"id={id} - El id tarea debe ser mayor a 0");
            }
            else
            {
                var tarea = _context.Tasks.FirstOrDefault(f => f.Id == id);
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
                    _context.Add(t);
                    _context.SaveChanges();
                    return CreatedAtRoute("GetTaskById", new { id = t.Id }, t);
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
                var tarea = _context.Tasks.FirstOrDefault(f => f.Id == id);
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
                        tarea.IsCompleted = true;
                        _context.SaveChanges();
                        return Ok(tarea);
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
                var tarea = _context.Tasks.FirstOrDefault(f => f.Id == id);
                if (tarea == null)
                {
                    return Ok($"id={id} - Tarea no encontrada");
                }
                else
                {
                    if (tarea.IsActive == true)
                    {
                        tarea.IsActive = false;
                        _context.SaveChanges();
                        return Ok(tarea);
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
            var tareasActivas = _context.Tasks.Where(w => w.IsActive == true).ToList();
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
            var tareasCompletas = _context.Tasks.Where(w => w.IsCompleted == true).ToList();
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
            var tareas = _context.Tasks.Where(w => w.IsCompleted == isCompleted && w.IsActive == isActive).ToList();
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
