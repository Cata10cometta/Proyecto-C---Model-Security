using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con la población de experiencias en el sistema.
    /// </summary>
    public class ExperiencePopulationBusiness
    {
        private readonly ExperiencePopulationData _experiencePopulationData;
        private readonly ILogger _logger;

        public ExperiencePopulationBusiness(ExperiencePopulationData experiencePopulationData, ILogger logger)
        {
            _experiencePopulationData = experiencePopulationData;
            _logger = logger;
        }

        // Método para obtener todas las poblaciones de experiencia como DTOs
        public async Task<IEnumerable<ExperiencePopulationGroupDTO>> GetAllExperiencePopulationsAsync()
        {
            try
            {
                var ExperiencePopulations = await _experiencePopulationData.GetAllAsync();
                var ExperiencePopulationsDTO = new List<ExperiencePopulationGroupDTO>();

                foreach (var experiencePopulation in ExperiencePopulations)
                {
                    ExperiencePopulationsDTO.Add(new ExperiencePopulationGroupDTO
                    {
                        Id = experiencePopulation.Id,
                        Name = experiencePopulation.Name,
                        ExperienceId = experiencePopulation.ExperienceId,
                        PopulationGradeId = experiencePopulation.PopulationGradeId

                    });
                }

                return ExperiencePopulationsDTO;
            }
            catch (Exception ex)

            {
                _logger.LogError(ex, "Error al obtener todas las poblaciones de experiencia");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de poblaciones de experiencia", ex);
            }
        }


        // Método para obtener una población de experiencia por ID como DTO
        public async Task<ExperiencePopulationGroupDTO> GetExperiencePopulationByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una población de experiencia con ID inválido: {PopulationId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID de la población de experiencia debe ser mayor que cero");
            }

            try
            {
                var experiencePopulation = await _experiencePopulationData.GetByIdAsync(id);
                if (experiencePopulation == null)
                {
                    _logger.LogInformation("No se encontró ninguna población de experiencia con ID: {PopulationId}", id);
                    throw new EntityNotFoundException("ExperiencePopulation", id);
                }

                return new ExperiencePopulationGroupDTO
                {
                    Id = experiencePopulation.Id,
                    Name = experiencePopulation.Name,
                    ExperienceId = experiencePopulation.ExperienceId,
                    PopulationGradeId = experiencePopulation.PopulationGradeId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la población de experiencia con ID: {PopulationId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la población de experiencia con ID {id}", ex);
            }
        }

        // Método para crear una población de experiencia desde un DTO
        public async Task<ExperiencePopulationGroupDTO> CreateExperiencePopulationAsync(ExperiencePopulationGroupDTO ExperiencePopulationGroupDTO)
        {
            try
            {
                ValidateExperiencePopulation(ExperiencePopulationGroupDTO);

                var experiencePopulationGroup = new ExperiencePopulationGroup
                {
                    Name = ExperiencePopulationGroupDTO.Name,
                    ExperienceId = ExperiencePopulationGroupDTO.ExperienceId,
                    PopulationGradeId = ExperiencePopulationGroupDTO.PopulationGradeId
                };

                var createdPopulation = await _experiencePopulationData.CreateAsync(experiencePopulationGroup);

                return new ExperiencePopulationGroupDTO
                {
                    Id = createdPopulation.Id,
                    Name = createdPopulation.Name,
                    ExperienceId = createdPopulation.ExperienceId,
                    PopulationGradeId = createdPopulation.PopulationGradeId

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva población de experiencia: {PopulationName}", ExperiencePopulationGroupDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la población de experiencia", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateExperiencePopulation(ExperiencePopulationGroupDTO populationDto)
        {
            if (populationDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto población de experiencia no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(populationDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar una población de experiencia con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name de la población de experiencia es obligatorio");
            }
        }
        // Método para mapear de ExperiencePopulationGroup a ExperiencePopulationGroupDTO
        private ExperiencePopulationGroupDTO MapToDTO(ExperiencePopulationGroup experiencePopulation)
        {
            return new ExperiencePopulationGroupDTO
            {
                Id = experiencePopulation.Id,
                Name = experiencePopulation.Name,
                ExperienceId = experiencePopulation.ExperienceId,
                PopulationGradeId = experiencePopulation.PopulationGradeId
            };
        }
        // Método para mapear de ExperiencePopulationGroupDTO a ExperiencePopulationGroup
        private ExperiencePopulationGroup MapToEntity(ExperiencePopulationGroupDTO experiencePopulationDto)
        {
            return new ExperiencePopulationGroup
            {
                Id = experiencePopulationDto.Id,
                Name = experiencePopulationDto.Name,
                ExperienceId = experiencePopulationDto.ExperienceId,
                PopulationGradeId = experiencePopulationDto.PopulationGradeId
            };
        }
        // Método para mapear de una lista de ExperiencePopulationGroup a una lista de ExperiencePopulationGroupDTO
        private IEnumerable<ExperiencePopulationGroupDTO> MapToDTOs(IEnumerable<ExperiencePopulationGroup> experiencePopulations)
        {
            var experiencePopulationDtos = new List<ExperiencePopulationGroupDTO>();
            foreach (var experiencePopulation in experiencePopulations)
            {
                experiencePopulationDtos.Add(MapToDTO(experiencePopulation));
            }
            return experiencePopulationDtos;
        }
    }
}
