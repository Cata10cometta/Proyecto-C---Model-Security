using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con las líneas temáticas de experiencias en el sistema.
    /// </summary>
    public class ExperienceLineThematicBusiness
    {
        private readonly ExperienceLineThematicData _experienceLineThematicData;
        private readonly ILogger _logger;

        public ExperienceLineThematicBusiness(ExperienceLineThematicData experienceLineThematicData, ILogger logger)
        {
            _experienceLineThematicData = experienceLineThematicData;
            _logger = logger;
        }

        // Método para obtener todas las líneas temáticas de experiencias como DTOs
        public async Task<IEnumerable<ExperienceLineThematicDTO>> GetAllExperienceLineThematicsAsync()
        {
            try
            {
                var experienceLineThematics = await _experienceLineThematicData.GetAllAsync();
                var experienceLineThematicDTO = new List<ExperienceLineThematicDTO>();

                foreach (var experienceLineThematic in experienceLineThematics)
                {
                    experienceLineThematicDTO.Add(new ExperienceLineThematicDTO
                    {
                        Id = experienceLineThematic.Id,
                        LineThematicId = experienceLineThematic.LineThematicId,
                        ExperienceId = experienceLineThematic.ExperienceId


                    });
                }

                return experienceLineThematicDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las líneas temáticas de experiencias");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de líneas temáticas de experiencias", ex);
            }
        }

        // Método para obtener una línea temática de experiencia por ID como DTO
        public async Task<ExperienceLineThematicDTO> GetExperienceLineThematicByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una línea temática de experiencia con ID inválido: {ExperienceLineThematicId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID de la línea temática de experiencia debe ser mayor que cero");
            }

            try
            {
                var experiencelineThematic = await _experienceLineThematicData.GetByIdAsync(id);
                if (experiencelineThematic == null)
                {
                    _logger.LogInformation("No se encontró ninguna línea temática de experiencia con ID: {ExperienceLineThematicId}", id);
                    throw new EntityNotFoundException("ExperienceLineThematic", id);
                }

                return new ExperienceLineThematicDTO
                {
                    Id = experiencelineThematic.Id,
                    LineThematicId = experiencelineThematic.LineThematicId,
                    ExperienceId = experiencelineThematic.ExperienceId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la línea temática de experiencia con ID: {ExperienceLineThematicId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la línea temática de experiencia con ID {id}", ex);
            }
        }

        // Método para crear una línea temática de experiencia desde un DTO
        public async Task<ExperienceLineThematicDTO> CreateExperienceLineThematicAsync(ExperienceLineThematicDTO ExperienceLineThematicDTO)
        {
            try
            {
                ValidateExperienceLineThematic(ExperienceLineThematicDTO);

                var lineThematic = new ExperienceLineThematic
                {
                    LineThematicId = ExperienceLineThematicDTO.LineThematicId,
                    ExperienceId = ExperienceLineThematicDTO.ExperienceId

                };

                var lineThematicCreated = await _experienceLineThematicData.CreateAsync(lineThematic);

                return new ExperienceLineThematicDTO
                {
                    Id = lineThematicCreated.Id,
                    LineThematicId = lineThematicCreated.LineThematicId,
                    ExperienceId = lineThematicCreated.ExperienceId

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nueva línea temática de experiencia: {ExperienceLineThematicName}", ExperienceLineThematicDTO?.LineThematicId ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la línea temática de experiencia", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateExperienceLineThematic(ExperienceLineThematicDTO experienceLineThematicDto)
        {
            if (experienceLineThematicDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto línea temática de experiencia no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(experienceLineThematicDto.LineThematicId))
            {
                _logger.LogWarning("Se intentó crear/actualizar una línea temática de experiencia con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name de la línea temática de experiencia es obligatorio");
            }
        }
        //Método  para mapear de ExperienceLineThematic a ExperienceLineThematicDTO
        private ExperienceLineThematicDTO MapToDTO(ExperienceLineThematic experienceLineThematic)
        {
            return new ExperienceLineThematicDTO
            {
                Id = experienceLineThematic.Id,
                LineThematicId = experienceLineThematic.LineThematicId,
                ExperienceId = experienceLineThematic.ExperienceId
            };
        }
        // Método para mapear de ExperienceLineThematicDTO a ExperienceLineThematic
        private ExperienceLineThematic MapToEntity(ExperienceLineThematicDTO experienceLineThematicDto)
        {
            return new ExperienceLineThematic
            {
                Id = experienceLineThematicDto.Id,
                LineThematicId = experienceLineThematicDto.LineThematicId,
                ExperienceId = experienceLineThematicDto.ExperienceId
            };
        }
        // Método para mapear una lista de ExperienceLineThematic a una lista de ExperienceLineThematicDTO
        private IEnumerable<ExperienceLineThematicDTO> MapToDTOs(IEnumerable<ExperienceLineThematic> experienceLineThematics)
        {
            var experienceLineThematicDTOs = new List<ExperienceLineThematicDTO>();
            foreach (var experienceLineThematic in experienceLineThematics)
            {
                experienceLineThematicDTOs.Add(MapToDTO(experienceLineThematic));
            }
            return experienceLineThematicDTOs;
        }
    }
}
