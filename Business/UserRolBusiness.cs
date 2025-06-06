using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los roles de usuario del sistema.
    /// </summary>
    public class UserRolBusiness
    {
        private readonly UserRolData _userRolData;
        private readonly ILogger _logger;

        public UserRolBusiness(UserRolData userRolData, ILogger logger)
        {
            _userRolData = userRolData;
            _logger = logger;
        }

        // Método para obtener todos los roles de usuario como DTOs
        public async Task<IEnumerable<UserRolDTO>> GetAllUserRolesAsync()
        {
            try
            {
                var userRoles = await _userRolData.GetAllAsync();
                var userRolesDTO = new List<UserRolDTO>();

                foreach (var userRol in userRoles)
                {
                    userRolesDTO.Add(new UserRolDTO
                    {
                        Id = userRol.Id,
                        UserId = userRol.UserId,
                        RolId = userRol.RolId
                    });
                }

                return userRolesDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles de usuario");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles de usuario", ex);
            }
        }

        // Método para obtener un rol de usuario por ID como DTO
        public async Task<UserRolDTO> GetUserRolByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un rol de usuario con ID inválido: {UserRolId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del rol de usuario debe ser mayor que cero");
            }

            try
            {
                var userRol = await _userRolData.GetByIdAsync(id);
                if (userRol == null)
                {
                    _logger.LogInformation("No se encontró ningún rol de usuario con ID: {UserRolId}", id);
                    throw new EntityNotFoundException("UserRol", id);
                }

                return new UserRolDTO
                {
                    Id = userRol.Id,
                    UserId = userRol.UserId,
                    RolId = userRol.RolId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol de usuario con ID: {UserRolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el rol de usuario con ID {id}", ex);
            }
        }

        // Método para crear un rol de usuario desde un DTO
        public async Task<UserRolDTO> CreateUserRolAsync(UserRolDTO UserRolDTO)
        {
            try
            {
                ValidateUserRol(UserRolDTO);

                var userRol = new UserRol
                {
                    UserId = UserRolDTO.UserId,
                    RolId = UserRolDTO.RolId

                };

                var userRolCreado = await _userRolData.CreateAsync(userRol);

                return new UserRolDTO
                {
                   Id = userRolCreado.Id,
                   UserId = userRolCreado.UserId,
                   RolId = userRolCreado.RolId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo rol de usuario: {UserRolNombre}", UserRolDTO?.UserId ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el rol de usuario", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateUserRol(UserRolDTO UserRolDTO)
        {
            if (UserRolDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto rol de usuario no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(UserRolDTO.UserId))
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol de usuario con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del rol de usuario es obligatorio");
            }
        }
        // Método para mapear de UserRol a UserRolDTO
        private UserRolDTO MapToDTO(UserRol userRol)
        {
            return new UserRolDTO
            {
                Id = userRol.Id,
                UserId = userRol.UserId,
                RolId = userRol.RolId
            };
        }
        // Método para mapear de UserRolDTO a UserRol
        private UserRol MapToEntity(UserRolDTO userRolDTO)
        {
            return new UserRol
            {
                Id = userRolDTO.Id,
                UserId = userRolDTO.UserId,
                RolId = userRolDTO.RolId
            };
        }
        // Método para mapear una lista de UserRol a una lista de UserRolDTO
        private IEnumerable<UserRolDTO> MapToDTOList(IEnumerable<UserRol> userRoles)
        {
            var userRolesDTO = new List<UserRolDTO>();
            foreach (var userRol in userRoles)
            {
                userRolesDTO.Add(MapToDTO(userRol));
            }
            return userRolesDTO;
        }
        }
    }
