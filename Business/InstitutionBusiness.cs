using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con las instituciones en el sistema.
    /// </summary>
    public class InstitutionBusiness
    {
        private readonly InstitutionData _institutionData;
        private readonly ILogger _logger;

        public InstitutionBusiness(InstitutionData institutionData, ILogger logger)
        {
            _institutionData = institutionData;
            _logger = logger;
        }

        // Método para obtener todas las instituciones como DTOs
        public async Task<IEnumerable<InstitutionDTO>> GetAllInstitutionesAsync()
        {
            try
            {
                var institutiones = await _institutionData.GetAllAsync();
                var institutionesDTO = new List<InstitutionDTO>();

                foreach (var institution in institutiones)
                {
                    institutionesDTO.Add(new InstitutionDTO
                    {
                        Id = institution.Id,
                        Name = institution.Name,
                        Address = institution.Address,
                        Phone = institution.Phone,
                        EmailInstitution = institution.EmailInstitution,
                        Department = institution.Department,
                        Commune = institution.Commune
                         
                    });
                }

                return institutionesDTO;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las instituciones");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de instituciones", ex);
            }
        }

        // Método para obtener una institución por ID como DTO
        public async Task<InstitutionDTO> GetInstitucionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una institución con ID inválido: {InstitucionId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID de la institución debe ser mayor que cero");
            }

            try
            {
                var institution = await _institutionData.GetByIdAsync(id);
                if (institution == null)
                {
                    _logger.LogInformation("No se encontró ninguna institución con ID: {InstitutionId}", id);
                    throw new EntityNotFoundException("Institution", id);
                }

                return new InstitutionDTO
                {
                    Id = institution.Id,
                    Name = institution.Name,
                    Address = institution.Address,
                    Phone = institution.Phone,
                    EmailInstitution = institution.EmailInstitution,
                    Department = institution.Department,
                    Commune = institution.Commune
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la institución con ID: {InstitucionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la institución con ID {id}", ex);
            }
        }

        // Método para crear una institución desde un DTO
        public async Task<InstitutionDTO> CreateInstitutionAsync(InstitutionDTO InstitutionDTO)
        {
            try
            {
                ValidateInstitution(InstitutionDTO);

                var institution = new Institution
                {
                    Name = InstitutionDTO.Name,
                    Address = InstitutionDTO.Address,
                    Phone = InstitutionDTO.Phone,
                    EmailInstitution = InstitutionDTO.EmailInstitution,
                    Department = InstitutionDTO.Department,
                    Commune = InstitutionDTO.Commune
                };

                var createdInstitution = await _institutionData.CreateAsync(institution);

                return new InstitutionDTO
                {
                    Id = createdInstitution.Id,
                    Name = createdInstitution.Name,
                    Address = createdInstitution.Address,
                    Phone = createdInstitution.Phone,
                    EmailInstitution = createdInstitution.EmailInstitution,
                    Department = createdInstitution.Department,
                    Commune = createdInstitution.Commune
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear una nueva institución: {InstitucionName}", InstitutionDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la institución", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateInstitution(InstitutionDTO InstitutionDTO)
        {
            if (InstitutionDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto institución no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(InstitutionDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar una institución con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name de la institución es obligatorio");
            }
        }
        // Método para mapear de Institution a InstitutionDTO
        private InstitutionDTO MaptoDTO(Institution institution)
        {
            return new InstitutionDTO
            {
                Id = institution.Id,
                Name = institution.Name,
                Address = institution.Address,
                Phone = institution.Phone,
                EmailInstitution = institution.EmailInstitution,
                Department = institution.Department,
                Commune = institution.Commune
            };
        }
        // Método para mapear de InstitutionDTO a Institution
        private Institution MaptoEntity(InstitutionDTO institutionDTO)
        {
            return new Institution
            {
                Id = institutionDTO.Id,
                Name = institutionDTO.Name,
                Address = institutionDTO.Address,
                Phone = institutionDTO.Phone,
                EmailInstitution = institutionDTO.EmailInstitution,
                Department = institutionDTO.Department,
                Commune = institutionDTO.Commune
            };
        }
        // Método para mapear una lista de Institution a una lista de InstitutionDTO
        private IEnumerable<InstitutionDTO> MaptoDTOList(IEnumerable<Institution> institutions)
        {
            var institutionDTOs = new List<InstitutionDTO>();
            foreach (var institution in institutions)
            {
                institutionDTOs.Add(MaptoDTO(institution));
            }
            return institutionDTOs;
        }
        }
    }
