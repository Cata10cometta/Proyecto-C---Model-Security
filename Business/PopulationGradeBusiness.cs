using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los grados de población en el sistema.
    /// </summary>
    public class PopulationGradeBusiness
    {
        private readonly PopulationGradeData _populationGradeData;
        private readonly ILogger _logger;

        public PopulationGradeBusiness(PopulationGradeData populationGradeData, ILogger logger)
        {
            _populationGradeData = populationGradeData;
            _logger = logger;
        }

        // Método para obtener todos los grados de población como DTOs
        public async Task<IEnumerable<PopulationGradeDTO>> GetAllPopulationGradesAsync()
        {
            try
            {
                var populationGrades = await _populationGradeData.GetAllAsync();
                var populationGradesDTO = new List<PopulationGradeDTO>();

                foreach (var populationGrade in populationGrades)
                {
                    populationGradesDTO.Add(new PopulationGradeDTO
                    {
                        Id = populationGrade.Id,
                        Name = populationGrade.Name,
                        
                    });
                }

                return populationGradesDTO;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los grados de población");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de grados de población", ex);
            }
        }

        // Método para obtener un grado de población por ID como DTO
        public async Task<PopulationGradeDTO> GetPopulationGradeByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un grado de población con ID inválido: {GradeId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del grado de población debe ser mayor que cero");
            }

            try
            {
                var grade = await _populationGradeData.GetByIdAsync(id);
                if (grade == null)
                {
                    _logger.LogInformation("No se encontró ningún grado de población con ID: {GradeId}", id);
                    throw new EntityNotFoundException("PopulationGrade", id);
                }

                return new PopulationGradeDTO
                {
                    Id = grade.Id,
                    Name = grade.Name,
                    
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el grado de población con ID: {GradeId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el grado de población con ID {id}", ex);
            }
        }

        // Método para crear un grado de población desde un DTO
        public async Task<PopulationGradeDTO> CreatePopulationGradeAsync(PopulationGradeDTO PopulationGradeDTO)
        {
            try
            {
                ValidatePopulationGrade(PopulationGradeDTO);

                var populationGrade = new PopulationGrade
                {
                    Name = PopulationGradeDTO.Name,
                    
                };

                var createdPopulationGrade = await _populationGradeData.CreateAsync(populationGrade);

                return new PopulationGradeDTO
                {
                    Id = createdPopulationGrade.Id,
                    Name = createdPopulationGrade.Name
                    
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo grado de población: {GradeName}", PopulationGradeDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el grado de población", ex);
            }
        }

        // Método para validar el DTO
        private void ValidatePopulationGrade(PopulationGradeDTO PopulationGradeDTO)
        {
            if (PopulationGradeDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto grado de población no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(PopulationGradeDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un grado de población con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del grado de población es obligatorio");
            }
        }
        // Método para mapear de PopulationGrade a PopulationGradeDTO
        private PopulationGradeDTO MapToDTO(PopulationGrade populationGrade)
        {
            return new PopulationGradeDTO
            {
                Id = populationGrade.Id,
                Name = populationGrade.Name,

            };
        }
        // Método para mapear de PopulationGradeDTO a PopulationGrade
        private PopulationGrade MapToEntity(PopulationGradeDTO populationGradeDTO)
        {
            return new PopulationGrade
            {
                Id = populationGradeDTO.Id,
                Name = populationGradeDTO.Name,
            };
        }
        // Método para mapear una lista de PopulationGrade a una lista de PopulationGradeDTO
        private IEnumerable<PopulationGradeDTO> MapToDTOList(IEnumerable<PopulationGrade> populationGrades)
        {
            var populationGradesDTO = new List<PopulationGradeDTO>();
            foreach (var populationGrade in populationGrades)
            {
                populationGradesDTO.Add(MapToDTO(populationGrade));
            }
            return populationGradesDTO;

        }
    }
}
