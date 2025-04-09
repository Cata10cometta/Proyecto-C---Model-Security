using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con el historial de experiencias en el sistema.
    /// </summary>
    public class HistoryExperienceBusiness
    {
        private readonly HistoryExperienceData _historyExperienceData;
        private readonly ILogger _logger;

        public HistoryExperienceBusiness(HistoryExperienceData historyExperienceData, ILogger logger)
        {
            _historyExperienceData = historyExperienceData;
            _logger = logger;
        }

        // Método para obtener todo el historial de experiencias como DTOs
        public async Task<IEnumerable<HistoryExperienceDTO>> GetAllHistoryExperiencesAsync()
        {
            try
            {
                var HistoryExperiences = await _historyExperienceData.GetAllAsync();
                var HistoryExperiencesDTO = new List<HistoryExperienceDTO>();

                foreach (var historyExperience in HistoryExperiences)
                {
                    HistoryExperiencesDTO.Add(new HistoryExperienceDTO
                    {
                        Id = historyExperience.Id,
                        Action = historyExperience.Action,
                        DateTime = historyExperience.DateTime,
                        TableName = historyExperience.TableName,
                        UserId = historyExperience.UserId,
                        ExperienceId = historyExperience.ExperienceId


                    });
                }

                return HistoryExperiencesDTO;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el historial de experiencias");
                throw new ExternalServiceException("Base de datos", "Error al recuperar el historial de experiencias", ex);
            }
        }

        // Método para obtener un historial de experiencia por ID como DTO
        public async Task<HistoryExperienceDTO> GetHistoryExperienceByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un historial de experiencia con ID inválido: {HistoryId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del historial de experiencia debe ser mayor que cero");
            }

            try
            {
                var history = await _historyExperienceData.GetByIdAsync(id);
                if (history == null)
                {
                    _logger.LogInformation("No se encontró ningún historial de experiencia con ID: {HistoryId}", id);
                    throw new EntityNotFoundException("HistoryExperience", id);
                }

                return new HistoryExperienceDTO
                {
                    Id = history.Id,
                    Action = history.Action,
                    DateTime = history.DateTime,
                    TableName = history.TableName,
                    UserId = history.UserId,
                    ExperienceId = history.ExperienceId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el historial de experiencia con ID: {HistoryId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el historial de experiencia con ID {id}", ex);
            }
        }

        // Método para registrar una nueva entrada en el historial de experiencias
        public async Task<HistoryExperienceDTO> CreateHistoryExperienceAsync(HistoryExperienceDTO HistoryExperienceDTO)
        {
            try
            {
                ValidateHistoryExperience(HistoryExperienceDTO);

                var history = new HistoryExperience
                {
  
                    Action = HistoryExperienceDTO.Action,
                    DateTime = HistoryExperienceDTO.DateTime,
                    TableName = HistoryExperienceDTO.TableName,
                    UserId = HistoryExperienceDTO.UserId,
                    ExperienceId = HistoryExperienceDTO.ExperienceId
                };

                var createdHistory = await _historyExperienceData.CreateAsync(history);

                return new HistoryExperienceDTO
                {
                    Id = createdHistory.Id,
                    Action = createdHistory.Action,
                    DateTime = createdHistory.DateTime,
                    TableName = createdHistory.TableName,
                    UserId = createdHistory.UserId,
                    ExperienceId = createdHistory.ExperienceId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo rol: {RolNombre}", HistoryExperienceDTO?.TableName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el rol", ex);

            }
        }

        // Método para validar el DTO
        private void ValidateHistoryExperience(HistoryExperienceDTO HistoryExperienceDTO)
        {
            if (HistoryExperienceDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto rol no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(HistoryExperienceDTO.TableName))
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del rol es obligatorio");
            }

        }
        // Método para mapear de HistoryExperience a HistoryExperienceDTO
        private HistoryExperienceDTO MapToDTO(HistoryExperience historyExperience)
        {
            return new HistoryExperienceDTO
            {
                Id = historyExperience.Id,
                Action = historyExperience.Action,
                DateTime = historyExperience.DateTime,
                TableName = historyExperience.TableName,
                UserId = historyExperience.UserId,
                ExperienceId = historyExperience.ExperienceId
            };
        }
        // Método para mapear de HistoryExperienceDTO a HistoryExperience
        private HistoryExperience MapToEntity(HistoryExperienceDTO historyExperienceDTO)
        {
            return new HistoryExperience
            {
                Id = historyExperienceDTO.Id,
                Action = historyExperienceDTO.Action,
                DateTime = historyExperienceDTO.DateTime,
                TableName = historyExperienceDTO.TableName,
                UserId = historyExperienceDTO.UserId,
                ExperienceId = historyExperienceDTO.ExperienceId
            };
        }
        // Método para mapear una lista de HistoryExperience a una lista de HistoryExperienceDTO
        private IEnumerable<HistoryExperienceDTO> MapToDTOList(IEnumerable<HistoryExperience> historyExperiences)
        {
            var historyExperienceDTOs = new List<HistoryExperienceDTO>();
            foreach (var historyExperience in historyExperiences)
            {
                historyExperienceDTOs.Add(MapToDTO(historyExperience));
            }
            return historyExperienceDTOs;
        }
        }
}
