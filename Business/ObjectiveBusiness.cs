using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los objetivos en el sistema.
    /// </summary>
    public class ObjectiveBusiness
    {
        private readonly ObjectiveData _objectiveData;
        private readonly ILogger _logger;

        public ObjectiveBusiness(ObjectiveData objectiveData, ILogger logger)
        {
            _objectiveData = objectiveData;
            _logger = logger;
        }

        // Método para obtener todos los objetivos como DTOs
        public async Task<IEnumerable<ObjectiveDTO>> GetAllObjectivesAsync()
        {
            try
            {
                var objectives = await _objectiveData.GetAllAsync();
                var objectivesDTO = new List<ObjectiveDTO>();

                foreach (var objective in objectives)
                {
                    objectivesDTO.Add(new ObjectiveDTO
                    {
                        Id = objective.Id,
                        ObjectiveDescription = objective.ObjectiveDescription,
                        Innovation = objective.Innovation,
                        Results = objective.Results,
                        Sustainability = objective.Sustainability,
                        ExperienceId = objective.ExperienceId
                    });                                                                                                                                                    
                }

                return objectivesDTO;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los objetivos");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de objetivos", ex);
            }
        }

        // Método para obtener un objetivo por ID como DTO
        public async Task<ObjectiveDTO> GetObjectiveByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un objetivo con ID inválido: {ObjectiveId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del objetivo debe ser mayor que cero");
            }

            try
            {
                var objective = await _objectiveData.GetByIdAsync(id);
                if (objective == null)
                {
                    _logger.LogInformation("No se encontró ningún objetivo con ID: {ObjectiveId}", id);
                    throw new EntityNotFoundException("Objective", id);
                }

                return new ObjectiveDTO
                {
                    Id = objective.Id,
                    ObjectiveDescription = objective.ObjectiveDescription,
                    Innovation = objective.Innovation,
                    Results = objective.Results,
                    Sustainability = objective.Sustainability,
                    ExperienceId = objective.ExperienceId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el objetivo con ID: {ObjectiveId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el objetivo con ID {id}", ex);
            }
        }

        // Método para crear un objetivo desde un DTO
        public async Task<ObjectiveDTO> CreateObjectiveAsync(ObjectiveDTO ObjectiveDTO)
        {
            try
            {
                ValidateObjective(ObjectiveDTO);

                var objective = new Objective
                {
                    ObjectiveDescription = ObjectiveDTO.ObjectiveDescription,
                    Innovation = ObjectiveDTO.Innovation,
                    Results = ObjectiveDTO.Results,
                    Sustainability = ObjectiveDTO.Sustainability,
                    ExperienceId = ObjectiveDTO.ExperienceId
                };

                var createdObjective = await _objectiveData.CreateAsync(objective);

                return new ObjectiveDTO
                {
                    Id = createdObjective.Id,
                    ObjectiveDescription = createdObjective.ObjectiveDescription,
                    Innovation = createdObjective.Innovation,
                    Results = createdObjective.Results,
                    Sustainability = createdObjective.Sustainability,
                    ExperienceId = createdObjective.ExperienceId

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo objetivo: {ObjectiveName}", ObjectiveDTO?.ObjectiveDescription ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el objetivo", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateObjective(ObjectiveDTO ObjectiveDTO)
        {
            if (ObjectiveDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto objetivo no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(ObjectiveDTO.ObjectiveDescription))
            {
                _logger.LogWarning("Se intentó crear/actualizar un objetivo con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del objetivo es obligatorio");
            }
        }
        // Método para mapear de Objective a ObjectiveDTO
        private ObjectiveDTO MapToDTO(Objective objective)
        {
            return new ObjectiveDTO
            {
                Id = objective.Id,
                ObjectiveDescription = objective.ObjectiveDescription,
                Innovation = objective.Innovation,
                Results = objective.Results,
                Sustainability = objective.Sustainability,
                ExperienceId = objective.ExperienceId
            };
        }
        // Método para mapear de ObjectiveDTO a Objective
        private Objective MapToEntity(ObjectiveDTO objectiveDTO)
        {
            return new Objective
            {
                Id = objectiveDTO.Id,
                ObjectiveDescription = objectiveDTO.ObjectiveDescription,
                Innovation = objectiveDTO.Innovation,
                Results = objectiveDTO.Results,
                Sustainability = objectiveDTO.Sustainability,
                ExperienceId = objectiveDTO.ExperienceId
            };
        }
        // Método para mapear una lista de Objective a una lista de ObjectiveDTO
        private IEnumerable<ObjectiveDTO> MapToDTOList(IEnumerable<Objective> objectives)
        {
            var objectiveDTOs = new List<ObjectiveDTO>();
            foreach (var objective in objectives)
            {
                objectiveDTOs.Add(MapToDTO(objective));
            }
            return objectiveDTOs;
        }
        }
    }
