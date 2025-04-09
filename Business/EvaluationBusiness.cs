using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con las evaluaciones en el sistema.
    /// </summary>
    public class EvaluationBusiness
    {
        private readonly EvaluationData _evaluationData;
        private readonly ILogger _logger;

        public EvaluationBusiness(EvaluationData evaluationData, ILogger logger)
        {
            _evaluationData = evaluationData;
            _logger = logger;
        }

        // Método para obtener todas las evaluaciones como DTOs
        public async Task<IEnumerable<EvaluationDTO>> GetAllEvaluationsAsync()
        {
            try
            {
                var evaluations = await _evaluationData.GetAllAsync();
                var evaluationsDTO = new List<EvaluationDTO>();

                foreach (var evaluation in evaluations)
                {
                    evaluationsDTO.Add(new EvaluationDTO
                    {
                        Id = evaluation.Id,
                        TypeEvaluation = evaluation.TypeEvaluation,
                        Comments = evaluation.Comments,
                        DateTime = evaluation.DateTime,
                        UserId = evaluation.UserId,
                        ExperienceId = evaluation.ExperienceId
                    });
                }
                return evaluationsDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las evaluaciones");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de evaluaciones", ex);
            }
        }

        // Método para obtener una evaluación por ID como DTO
        public async Task<EvaluationDTO> GetEvaluationByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener una evaluación con ID inválido: {EvaluationId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID de la evaluación debe ser mayor que cero");
            }

            try
            {
                var evaluation = await _evaluationData.GetByIdAsync(id);
                if (evaluation == null)
                {
                    _logger.LogInformation("No se encontró ninguna evaluación con ID: {EvaluationId}", id);
                    throw new EntityNotFoundException("Evaluation", id);
                }

                return new EvaluationDTO

                {
                    Id = evaluation.Id,
                    TypeEvaluation = evaluation.TypeEvaluation,
                    Comments = evaluation.Comments,
                    DateTime = evaluation.DateTime,
                    UserId = evaluation.UserId,
                    ExperienceId = evaluation.ExperienceId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la evaluación con ID: {EvaluationId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar la evaluación con ID {id}", ex);
            }
        }

        // Método para crear una evaluación desde un DTO
        public async Task<EvaluationDTO> CreateEvaluationAsync(EvaluationDTO EvaluationDTO)
        {
            try
            {
                ValidateEvaluation(EvaluationDTO);

                var evaluation = new Evaluation
                {
                    TypeEvaluation = EvaluationDTO.TypeEvaluation,
                    Comments = EvaluationDTO.Comments,
                    DateTime = EvaluationDTO.DateTime,
                    UserId = EvaluationDTO.UserId,
                    ExperienceId = EvaluationDTO.ExperienceId
                };

                var createdEvaluation = await _evaluationData.CreateAsync(evaluation);

                return new EvaluationDTO
                {
                    Id = createdEvaluation.Id,
                    TypeEvaluation = createdEvaluation.TypeEvaluation,
                    Comments = createdEvaluation.Comments,
                    DateTime = createdEvaluation.DateTime,
                    UserId = createdEvaluation.UserId,
                    ExperienceId = createdEvaluation.ExperienceId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo rol: {EvaluationNombre}", EvaluationDTO?.TypeEvaluation ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear la evaluación", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateEvaluation(EvaluationDTO EvaluationDTO)
        {
            if (EvaluationDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto evaluación no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(EvaluationDTO.TypeEvaluation))
            {
                _logger.LogWarning("Se intentó crear/actualizar una evaluación con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name de la evaluación es obligatorio");
            }
        }

        // Método para mapear de Evaluation a EvaluationDTO
        private EvaluationDTO MapToDTO(Evaluation evaluation)
        {
            return new EvaluationDTO
            {
                Id = evaluation.Id,
                TypeEvaluation = evaluation.TypeEvaluation,
                Comments = evaluation.Comments,
                DateTime = evaluation.DateTime,
                UserId = evaluation.UserId,
                ExperienceId = evaluation.ExperienceId
            };
        }
        // Método para mapear de EvaluationDTO a Evaluation
        private Evaluation MapToEntity(EvaluationDTO evaluationDTO)
        {
            return new Evaluation
            {
                Id = evaluationDTO.Id,
                TypeEvaluation = evaluationDTO.TypeEvaluation,
                Comments = evaluationDTO.Comments,
                DateTime = evaluationDTO.DateTime,
                UserId = evaluationDTO.UserId,
                ExperienceId = evaluationDTO.ExperienceId
            };
        }
        // Método para mapear una lista de Criteria a una lista de CriteriaDTO
        private IEnumerable<EvaluationDTO> MapToDTOList(IEnumerable<Evaluation> evaluations)
        {
            var evaluationsDTO = new List<EvaluationDTO>();
            foreach (var evaluation in evaluations)
            {
                evaluationsDTO.Add(MapToDTO(evaluation));
            }
            return evaluationsDTO;




        }
    }
}        
    

