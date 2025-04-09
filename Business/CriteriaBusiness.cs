using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Utilities.Exceptions;

namespace Business
{
    ///<summary>
    ///Clase de negocio encargada de la lógica relacionada con los roles del sitema.
    ///</summary>
    public class CriteriaBusiness
    {
        private readonly CriteriaData _criteriaData;
        private readonly ILogger _logger;

        public CriteriaBusiness(CriteriaData criteriaData, ILogger logger)
        {
            _criteriaData = criteriaData;
            _logger = logger;
        }
        // Método para obtener todos los roles como DTOs
        public async Task<IEnumerable<CriteriaDTO>> GetAllCriteriaAsync()
        {
            try
            {
                var criterias = await _criteriaData.GetAllAsync();
                var criteriasDTO = new List<CriteriaDTO>();

                foreach (var criteria in criterias)
                {
                    criteriasDTO.Add(new CriteriaDTO
                    {
                        Id = criteria.Id,
                        Name = criteria.Name
                       
                    });
                }
                return MapToDTOList(criterias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles", ex);
            }
        }
        //Método para obtener un rol por ID como DTO
        public async Task<CriteriaDTO> GetCriteriaByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una criteria con ID inválido: {CriteriaId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID de la criteria debe ser mayor que cero");
            }
            try
            {
                var criteria = await _criteriaData.GetByIdAsync(id);
                if (criteria == null)
                {
                    _logger.LogInformation("No se encontró ningúna criteria con ID: {CriteriaId}", id);
                    throw new EntityNotFoundException("Criteria", id);
                }
                return new CriteriaDTO
                {
                    Id = criteria.Id,
                    Name = criteria.Name,

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la criteria con ID: {CriteriaId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la criteria con ID {id}", ex);
            }
        }

        // Método para crear un rol desde un DTO
        public async Task<CriteriaDTO> CreateCriteriaAsync(CriteriaDTO CriteriaDTO)
        {
            try
            {
                ValidateCriteria (CriteriaDTO);

                var criteria = new Criteria
                {
                    Name = CriteriaDTO.Name

                };

                var criteriaCreado = MapToEntity(CriteriaDTO);

                return new CriteriaDTO
                {
                    Id = criteriaCreado.Id,
                    Name = criteriaCreado.Name

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo criteria: {CriteriaNombre}", CriteriaDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el rol", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateCriteria(CriteriaDTO CriteriaDTO)
        {
            if (CriteriaDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto critera no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(CriteriaDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar una criteria con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name de criteria es obligatorio");
            }
        }
        // Método para mapear de Criteria a CriteriaDTO

        private CriteriaDTO MapToDTO(Criteria criteria)
        {
            return new CriteriaDTO
            {
                Id = criteria.Id,
                Name = criteria.Name
            };
        }

        // Método para mapear de CriteriaDTO a Criteria
        private Criteria MapToEntity(CriteriaDTO criteriaDTO)
        {
            return new Criteria
            {
                Id = criteriaDTO.Id,
                Name = criteriaDTO.Name
            };
        }
        // Método para mapear una lista de Criteria a una lista de CriteriaDTO
        private IEnumerable<CriteriaDTO> MapToDTOList(IEnumerable<Criteria> criterias)
        {
            var criteriasDTO = new List<CriteriaDTO>();
            foreach (var criteria in criterias)
            {
                criteriasDTO.Add(MapToDTO(criteria));
            }
            return criteriasDTO;
        }
    }
}



