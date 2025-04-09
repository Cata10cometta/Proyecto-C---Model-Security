using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los grados de experiencia en el sistema.
    /// </summary>
    public class ExperienceGradeBusiness
    {
        private readonly ExperienceGradeData _experienceGradeData;
        private readonly ILogger _logger;

        public ExperienceGradeBusiness(ExperienceGradeData experienceGradeData, ILogger logger)
        {
            _experienceGradeData = experienceGradeData;
            _logger = logger;
        }

        // Método para obtener todos los grados de experiencia como DTOs
        public async Task<IEnumerable<ExperienceGradeDTO>> GetAllExperienceGradesAsync()
        {
            try
            {
                var experienceGrades = await _experienceGradeData.GetAllAsync();
                var experienceGradesDTO = new List<ExperienceGradeDTO>();

                foreach (var experienceGrade in experienceGrades)
                {
                    experienceGradesDTO.Add(new ExperienceGradeDTO
                    {
                        Id = experienceGrade.Id,
                        GradeId = experienceGrade.GradeId
                        
                        

                    });
                }

                return experienceGradesDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los grados de experiencia");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de grados de experiencia", ex);
            }
        }

        // Método para obtener un grado de experiencia por ID como DTO
        public async Task<ExperienceGradeDTO> GetExperienceGradeByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un grado de experiencia con ID inválido: {ExperienceGradeId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del grado de experiencia debe ser mayor que cero");
            }

            try
            {
                var experienceGrade = await _experienceGradeData.GetByIdAsync(id);
                if (experienceGrade == null)
                {
                    _logger.LogInformation("No se encontró ningún grado de experiencia con ID: {ExperienceGradeId}", id);
                    throw new EntityNotFoundException("ExperienceGrade", id);
                }

                return new ExperienceGradeDTO
                {
                  Id = experienceGrade.Id,
                  GradeId = experienceGrade.GradeId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el grado de experiencia con ID: {ExperienceGradeId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el grado de experiencia con ID {id}", ex);
            }
        }

        // Método para crear un grado de experiencia desde un DTO
        public async Task<ExperienceGradeDTO> CreateExperienceGradeAsync(ExperienceGradeDTO ExperienceGradeDTO)
        {
            try
            {
                ValidateExperienceGrade(ExperienceGradeDTO);

                var experienceGrade = new ExperienceGrade
                {

                    GradeId = ExperienceGradeDTO.GradeId
                };

                var experienceGradeCreated = await _experienceGradeData.CreateAsync(experienceGrade);

                return new ExperienceGradeDTO
                {
                    Id = experienceGradeCreated.Id,
                    GradeId = experienceGrade.GradeId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo grado de experiencia: {ExperienceGradeName}", ExperienceGradeDTO?.GradeId ? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el grado de experiencia", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateExperienceGrade(ExperienceGradeDTO experienceGradeDto)
        {
            if (experienceGradeDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto grado de experiencia no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(experienceGradeDto.GradeId))
            {
                _logger.LogWarning("Se intentó crear/actualizar un grado de experiencia con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del grado de experiencia es obligatorio");
            }

        }
        //Método  para mapear de ExperienceGrade a ExperienceGradeDTO
        private ExperienceGradeDTO MapToDTO(ExperienceGrade experienceGrade)
        {
            return new ExperienceGradeDTO
            {
                Id = experienceGrade.Id,
                GradeId = experienceGrade.GradeId
            };

        }
        //Método para mapear de ExperienceGradeDTO a ExperienceGrade
        private ExperienceGrade MapToEntity(ExperienceGradeDTO experienceGradeDto)
        {
            return new ExperienceGrade
            {
                Id = experienceGradeDto.Id,
                GradeId = experienceGradeDto.GradeId
            };
        }
        //Método para mapear una lista de ExperienceGrade a una lista de ExperienceGradeDTO
        private IEnumerable<ExperienceGradeDTO> MapToDTOList(IEnumerable<ExperienceGrade> experienceGrades)
        {
            var experienceGradesDTO = new List<ExperienceGradeDTO>();
            foreach (var experienceGrade in experienceGrades)
            {
                experienceGradesDTO.Add(MapToDTO(experienceGrade));
            }
            return experienceGradesDTO;
        }
        }
    }
