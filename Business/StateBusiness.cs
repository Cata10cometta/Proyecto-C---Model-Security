using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los estados en el sistema.
    /// </summary>
    public class StateBusiness
    {
        private readonly StateData _stateData;
        private readonly ILogger _logger;

        public StateBusiness(StateData stateData, ILogger logger)
        {
            _stateData = stateData;
            _logger = logger;
        }

        // Método para obtener todos los estados como DTOs
        public async Task<IEnumerable<StateDTO>> GetAllStatesAsync()
        {
            try
            {
                var states = await _stateData.GetAllAsync();
                var statesDTO = new List<StateDTO>();

                foreach (var state in states)
                {
                    statesDTO.Add(new StateDTO
                    {
                        Id = state.Id,
                        Name = state.Name,
                        
                    });
                }

                return statesDTO;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los estados");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de estados", ex);
            }
        }

        // Método para obtener un estado por ID como DTO
        public async Task<StateDTO> GetStateByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un estado con ID inválido: {StateId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del estado debe ser mayor que cero");
            }

            try
            {
                var state = await _stateData.GetByIdAsync(id);
                if (state == null)
                {
                    _logger.LogInformation("No se encontró ningún estado con ID: {StateId}", id);
                    throw new EntityNotFoundException("State", id);
                }

                return new StateDTO
                {
                    Id = state.Id,
                    Name = state.Name
                    
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el estado con ID: {StateId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el estado con ID {id}", ex);
            }
        }

        // Método para crear un estado desde un DTO
        public async Task<StateDTO> CreateStateAsync(StateDTO StateDTO)
        {
            try
            {
                ValidateState(StateDTO);

                var state = new State
                {
                    Name = StateDTO.Name
               
                };

                var createdState = await _stateData.CreateAsync(state);

                return new StateDTO
                {
                    Id = createdState.Id,
                    Name = createdState.Name
                    
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo estado: {StateName}", StateDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el estado", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateState(StateDTO StateDTO)
        {
            if (StateDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto estado no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(StateDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un estado con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del estado es obligatorio");
            }
        }
        // Método para mapear de State a StateDTO
        private StateDTO MapToDTO(State state)
        {
            return new StateDTO
            {
                Id = state.Id,
                Name = state.Name
            };
        }
        // Método para mapear de StateDTO a State
        private State MapToEntity(StateDTO stateDTO)
        {
            return new State
            {
                Id = stateDTO.Id,
                Name = stateDTO.Name
            };
        }
        // Método para mapear una lista de State a una lista de StateDTO
        private IEnumerable<StateDTO> MapToDTOs(IEnumerable<State> states)
        {
            var stateDTOs = new List<StateDTO>();
            foreach (var state in states)
            {
                stateDTOs.Add(MapToDTO(state));
            }
            return stateDTOs;
        }
        }
}
