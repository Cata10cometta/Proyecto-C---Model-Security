using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los roles del sistema.
    /// </summary>
    public class UserBusiness
    {
        private readonly UserData _UserData;
        private readonly ILogger _logger;

        public UserBusiness(UserData UserData, ILogger logger)
        {
            _UserData = UserData;
            _logger = logger;
        }

        // Método para obtener todos los roles como DTOs
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            try
            {
                var users = await _UserData.GetAllAsync();
                var usersDTO = new List<UserDTO>();

                foreach (var User in users)
                {
                    usersDTO.Add(new UserDTO
                    {
                        Id = User.Id,
                        Name = User.Name,
                        Email = User.Email,
                        Password = User.Password 
                    });
                }

                return usersDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles", ex);
            }
        }

        // Método para obtener un rol por ID como DTO
        public async Task<UserDTO> GetRolByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un User con ID inválido: {UserId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del User debe ser mayor que cero");
            }

            try
            {
                var User = await _UserData.GetByIdAsync(id);
                if (User == null)
                {
                    _logger.LogInformation("No se encontró ningún rol con ID: {UserId}", id);
                    throw new EntityNotFoundException("User", id);
                }

                return new UserDTO
                {
                        Id = User.Id,
                        Name = User.Name,
                        Email = User.Email,
                        Password = User.Password 
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el User con ID: {UserId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el User con ID {id}", ex);
            }
        }

        // Método para crear un rol desde un DTO
        public async Task<UserDTO> CreateUserAsync(UserDTO UserDTO)
        {
            try
            {
                ValidateUser(UserDTO);

                var User = new User
                {
                   
                        Name = UserDTO.Name,
                        Email = UserDTO.Email,
                        Password = UserDTO.Password  // Si existe en la entidad
                };

                var UserCreado = await _UserData.CreateAsync(User);

                return new UserDTO
                {
                  Id = UserCreado.Id,
                        Name = UserCreado.Name,
                        Email = UserCreado.Email,
                        Password = UserCreado.Password  // Si existe en la entidad
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo User: {UserNombre}", UserDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el User", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateUser(UserDTO UserDTO)
        {
            if (UserDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto rol no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(UserDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del rol es obligatorio");
            }
        }
        //Método para mapear de User a UserDTO
        private UserDTO MapToDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password // Si existe en la entidad
            };
        }
        // Método para mapear de UserDTO a User
        private User MapToEntity(UserDTO userDTO)
        {
            return new User
            {
                Id = userDTO.Id,
                Name = userDTO.Name,
                Email = userDTO.Email,
                Password = userDTO.Password // Si existe en la entidad
            };
        }
        // Método para mapear una lista de User a una lista de UserDTO
        private List<UserDTO> MapToDTOList(IEnumerable<User> users)
        {
            var userDTOs = new List<UserDTO>();
            foreach (var user in users)
            {
                userDTOs.Add(MapToDTO(user));
            }
            return userDTOs;
        }
        }
}

