using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con las experiencias en el sistema.
    /// </summary>
    public class ExperienceBusiness
    {
        private readonly ExperienceData _experienceData;
        private readonly ILogger _logger;

        public ExperienceBusiness(ExperienceData experienceData, ILogger logger)
        {
            _experienceData = experienceData;
            _logger = logger;
        }

        // Método para obtener todas las experiencias como DTOs
        public async Task<IEnumerable<ExperienceDTO>> GetAllExperiencesAsync()
        {
            try
            {
                var experiences = await _experienceData.GetAllAsync();
                var experienceDTO = new List<ExperienceDTO>();

                foreach (var experience in experiences)
                {
                    experienceDTO.Add(new ExperienceDTO
                    {
                        Id = experience.Id,
                        NameExperiences = experience.NameExperiences,
                        Summary = experience.Summary,
                        Methodologies = experience.Methodologies,
                        Transfer = experience.Transfer,
                        DateRegistration = experience.DateRegistration,
                        code = experience.code,
                        UserId = experience.UserId,
                        InstitutionId = experience.InstitutionId
                    });
                }

                return experienceDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las experiencias");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de experiencias", ex);
            }
        }

        // Método para obtener una experiencia por ID como DTO
        public async Task<ExperienceDTO> GetExperienceByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una experiencia con ID inválido: {ExperienceId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID de la experiencia debe ser mayor que cero");
            }

            try
            {
                var experience = await _experienceData.GetByIdAsync(id);
                if (experience == null)
                {
                    _logger.LogInformation("No se encontró ninguna experiencia con ID: {ExperienceId}", id);
                    throw new EntityNotFoundException("Experience", id);
                }

                return new ExperienceDTO
                {
                    Id = experience.Id,
                    NameExperiences = experience.NameExperiences,
                    Summary = experience.Summary,
                    Methodologies = experience.Methodologies,
                    Transfer = experience.Transfer,
                    DateRegistration = experience.DateRegistration,
                    code = experience.code,
                    UserId = experience.UserId,
                    InstitutionId = experience.InstitutionId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la experiencia con ID: {ExperienceId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la experiencia con ID {id}", ex);
            }
        }

        // Método para crear una experiencia desde un DTO
        public async Task<ExperienceDTO> CreateExperienceAsync(ExperienceDTO ExperienceDTO)
        {
            try
            {
                ValidateExperience(ExperienceDTO);

                var experience = new Experience
                {
                    NameExperiences = ExperienceDTO.NameExperiences,
                    Summary = ExperienceDTO.Summary,
                    Methodologies = ExperienceDTO.Methodologies,
                    Transfer = ExperienceDTO.Transfer,
                    DateRegistration = ExperienceDTO.DateRegistration,
                    code = ExperienceDTO.code,
                    UserId = ExperienceDTO.UserId,
                    InstitutionId = ExperienceDTO.InstitutionId
                };

                var experienceCreated = await _experienceData.CreateAsync(experience);

                return new ExperienceDTO
                {
                    Id = experienceCreated.Id,
                    NameExperiences = experienceCreated.NameExperiences,
                    Summary = experienceCreated.Summary,
                    Methodologies = experienceCreated.Methodologies,
                    Transfer = experienceCreated.Transfer,
                    DateRegistration = experienceCreated.DateRegistration,
                    code = experienceCreated.code,
                    UserId = experienceCreated.UserId,
                    InstitutionId = experienceCreated.InstitutionId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nueva experiencia: {ExperienceTitle}", ExperienceDTO?.NameExperiences ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la experiencia", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateExperience(ExperienceDTO experienceDto)
        {
            if (experienceDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto experiencia no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(experienceDto.NameExperiences))
            {
                _logger.LogWarning("Se intentó crear/actualizar una experiencia con Title vacío");
                throw new Utilities.Exceptions.ValidationException("Title", "El Title de la experiencia es obligatorio");
            }
        }
        //Método para mapear de Experience a ExperienceDTO
        private ExperienceDTO MapToDTO(Experience experience)
        {
            return new ExperienceDTO
            {
                Id = experience.Id,
                NameExperiences = experience.NameExperiences,
                Summary = experience.Summary,
                Methodologies = experience.Methodologies,
                Transfer = experience.Transfer,
                DateRegistration = experience.DateRegistration,
                code = experience.code,
                UserId = experience.UserId,
                InstitutionId = experience.InstitutionId
            };
        }

        //Método para mapear de ExperienceDTO a Experience
        private Experience MapToEntity(ExperienceDTO experienceDto)
        {
            return new Experience
            {
                Id = experienceDto.Id,
                NameExperiences = experienceDto.NameExperiences,
                Summary = experienceDto.Summary,
                Methodologies = experienceDto.Methodologies,
                Transfer = experienceDto.Transfer,
                DateRegistration = experienceDto.DateRegistration,
                code = experienceDto.code,
                UserId = experienceDto.UserId,
                InstitutionId = experienceDto.InstitutionId
            };
        }
        //Método para mapear una lista de Experience a una lista de ExperienceDTO
        private IEnumerable<ExperienceDTO> MapToDTOList(IEnumerable<Experience> experiences)
        {
            var experienceDTOs = new List<ExperienceDTO>();
            foreach (var experience in experiences)
            {
                experienceDTOs.Add(MapToDTO(experience));
            }
            return experienceDTOs;
        }
    }
}
