using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los criterios de evaluación en el sistema.
    /// </summary>
    public class EvaluationCriteriaBusiness
    {
        private readonly EvaluationCriteriaData _evaluationCriteriaData;
        private readonly ILogger _logger;

        public EvaluationCriteriaBusiness(EvaluationCriteriaData evaluationCriteriaData, ILogger logger)
        {
            _evaluationCriteriaData = evaluationCriteriaData;
            _logger = logger;
        }

        // Método para obtener todos los criterios de evaluación como DTOs
        public async Task<IEnumerable<EvaluationCriteriaDTO>> GetAllevaluationCriteriasAsync()
        {
            try
            {
                var evaluationCriterias = await _evaluationCriteriaData.GetAllAsync();
                var evaluationCriteriasDTO = new List<EvaluationCriteriaDTO>();

                foreach (var evaluationCriteria in evaluationCriterias)
                {
                    evaluationCriteriasDTO.Add(new EvaluationCriteriaDTO
                    {
                        Id = evaluationCriteria.Id,
                        Score = evaluationCriteria.Score,
                        EvaluationId = evaluationCriteria.EvaluationId
                    });
                }

                return evaluationCriteriasDTO;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los criterios de evaluación");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de criterios de evaluación", ex);
            }
        }

        // Método para obtener un criterio de evaluación por ID como DTO
        public async Task<EvaluationCriteriaDTO> GetEvaluationCriteriaByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un criterio de evaluación con ID inválido: {CriteriaId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del criterio de evaluación debe ser mayor que cero");
            }

            try
            {
                var criterion = await _evaluationCriteriaData.GetByIdAsync(id);
                if (criterion == null)
                {
                    _logger.LogInformation("No se encontró ningún criterio de evaluación con ID: {CriteriaId}", id);
                    throw new EntityNotFoundException("EvaluationCriteria", id);
                }

                return new EvaluationCriteriaDTO
                {
                    Id = criterion.Id,
                    Score = criterion.Score,
                    EvaluationId = criterion.EvaluationId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el criterio de evaluación con ID: {CriteriaId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el criterio de evaluación con ID {id}", ex);
            }
        }

        // Método para crear un criterio de evaluación desde un DTO
        public async Task<EvaluationCriteriaDTO> CreateEvaluationCriteriaAsync(EvaluationCriteriaDTO EvaluationCriteriaDTO)
        {
            try
            {
                ValidateEvaluationCriteria(EvaluationCriteriaDTO);

                var criterion = new EvaluationCriteria
                {
                    Score = EvaluationCriteriaDTO.Score,
                    EvaluationId = EvaluationCriteriaDTO.EvaluationId,

                };

                var createdCriterion = await _evaluationCriteriaData.CreateAsync(criterion);

                return new EvaluationCriteriaDTO
                {
                    Id = createdCriterion.Id,
                    Score = createdCriterion.Score,
                    EvaluationId = createdCriterion.EvaluationId,

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo criterio de evaluación: {CriteriaName}", EvaluationCriteriaDTO?.Score ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el criterio de evaluación", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateEvaluationCriteria(EvaluationCriteriaDTO EvaluationCriteriaDTO)
        {
            if (EvaluationCriteriaDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto criterio de evaluación no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(EvaluationCriteriaDTO.Score))
            {
                _logger.LogWarning("Se intentó crear/actualizar un criterio de evaluación con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del criterio de evaluación es obligatorio");
            }
        }

        //Método para mapear de EvaluationCriteria a EvaluationCriteriaDTO
        private EvaluationCriteriaDTO MapToDTO(EvaluationCriteria evaluationCriteria)
        {
            return new EvaluationCriteriaDTO
            {
                Id = evaluationCriteria.Id,
                Score = evaluationCriteria.Score,
                EvaluationId = evaluationCriteria.EvaluationId
            };
        }
        //Método para mapear de EvaluationCriteriaDTO a EvaluationCriteria
        private EvaluationCriteria MapToEntity(EvaluationCriteriaDTO EvaluationCriteriaDTO)
        {
            return new EvaluationCriteria
            {
                Id = EvaluationCriteriaDTO.Id,
                Score = EvaluationCriteriaDTO.Score,
                EvaluationId = EvaluationCriteriaDTO.EvaluationId
            };
        }
        // Método para mapear una lista de EvaluationCriteria a una lista de EvaluationCriteriaDTO
        private IEnumerable<EvaluationCriteriaDTO> MapToDTOList(IEnumerable<EvaluationCriteria> evaluationCriterias)
        {
            var evaluationCriteriasDTO = new List<EvaluationCriteriaDTO>();
            foreach (var evaluationCriteria in evaluationCriterias)
            {
                evaluationCriteriasDTO.Add(MapToDTO(evaluationCriteria));
            }
            return evaluationCriteriasDTO;
        }
    }
}



