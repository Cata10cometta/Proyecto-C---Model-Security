using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los grados en el sistema.
    /// </summary>
    public class GradeBusiness
    {
        private readonly GradeData _gradeData;
        private readonly ILogger _logger;

        public GradeBusiness(GradeData gradeData, ILogger logger)
        {
            _gradeData = gradeData;
            _logger = logger;
        }

        // Método para obtener todos los grados como DTOs
        public async Task<IEnumerable<GradeDTO>> GetAllGradesAsync()
        {
            try
            {
                var grades = await _gradeData.GetAllAsync();
                var gradesDTO = new List<GradeDTO>();

                foreach (var grade in grades)
                {
                    gradesDTO.Add(new GradeDTO
                    {
                        Id = grade.Id,
                        Name = grade.Name
                        
                    });
                }

                return gradesDTO;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los grados");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de grados", ex);
            }
        }

        // Método para obtener un grado por ID como DTO
        public async Task<GradeDTO> GetGradeByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un grado con ID inválido: {GradeId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del grado debe ser mayor que cero");
            }

            try
            {
                var grade = await _gradeData.GetByIdAsync(id);
                if (grade == null)
                {
                    _logger.LogInformation("No se encontró ningún grado con ID: {GradeId}", id);
                    throw new EntityNotFoundException("Grade", id);
                }

                return new GradeDTO
                {
                    Id = grade.Id,
                    Name = grade.Name
                    
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el grado con ID: {GradeId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el grado con ID {id}", ex);
            }
        }

        // Método para crear un grado desde un DTO
        public async Task<GradeDTO> CreateGradeAsync(GradeDTO GradeDTO)
        {
            try
            {
                ValidateGrade(GradeDTO);

                var grade = new Grade
                {
                    Name = GradeDTO.Name
                    
                };

                var createdGrade = await _gradeData.CreateAsync(grade);

                return new GradeDTO
                {
                    Id = createdGrade.Id,
                    Name = createdGrade.Name
                   
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo grado: {GradeName}", GradeDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el grado", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateGrade(GradeDTO gradeDto)
        {
            if (gradeDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto grado no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(gradeDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un grado con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del grado es obligatorio");
            }
        }
        // Método para mapear de Grade a GradeDTO
        private GradeDTO MapToDTO(Grade grade)
        {
            return new GradeDTO
            {
                Id = grade.Id,
                Name = grade.Name
            };
        }
        // Método para mapear de GradeDTO a Grade
        private Grade MapToEntity(GradeDTO gradeDto)
        {
            return new Grade
            {
                Id = gradeDto.Id,
                Name = gradeDto.Name
            };
        }
        // Método para mapear una lista de Grade a una lista de GradeDTO
        private IEnumerable<GradeDTO> MapToDTOs(IEnumerable<Grade> grades)
        {
            var gradeDTOs = new List<GradeDTO>();
            foreach (var grade in grades)
            {
                gradeDTOs.Add(MapToDTO(grade));
            }
            return gradeDTOs;
        }
        }
}