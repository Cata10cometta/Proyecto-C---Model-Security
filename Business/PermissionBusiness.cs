

using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los permisos en el sistema.
    /// </summary>
    public class PermissionBusiness
    {
        private readonly PermissionData _permissionData;
        private readonly ILogger _logger;

        public PermissionBusiness(PermissionData permissionData, ILogger logger)
        {
            _permissionData = permissionData;
            _logger = logger;
        }

        // Método para obtener todos los permisos como DTOs
        public async Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync()
        {
            try
            {
                var permissions = await _permissionData.GetAllAsync();
                var permissionsDTO = new List<PermissionDTO>();

                foreach (var permission in permissions)
                {
                    permissionsDTO.Add(new PermissionDTO
                    {
                        Id = permission.Id, 
                        PermissionType = permission.PermissionType,
                        RolId = permission.RolId
                    });
                }

                return permissionsDTO;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los permisos");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de permisos", ex);
            }
        }

        // Método para obtener un permiso por ID como DTO
        public async Task<PermissionDTO> GetPermisionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un permiso con ID inválido: {PermisionId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del permiso debe ser mayor que cero");
            }

            try
            {
                var permission = await _permissionData.GetByIdAsync(id);
                if (permission == null)
                {
                    _logger.LogInformation("No se encontró ningún permiso con ID: {PermisionId}", id);
                    throw new EntityNotFoundException("Permision", id);
                }

                return new PermissionDTO
                {
                    Id = permission.Id,
                    PermissionType = permission.PermissionType,
                    RolId = permission.RolId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso con ID: {PermisionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el permiso con ID {id}", ex);
            }
        }

        // Método para crear un permiso desde un DTO
        public async Task<PermissionDTO> CreatePermissionAsync(PermissionDTO PermissionDTO)
        {
            try
            {
                ValidatePermission(PermissionDTO);
                var permission = new Permission
                {
                    PermissionType = PermissionDTO.PermissionType,
                    RolId = PermissionDTO.RolId
                };

                var permissionCreado = await _permissionData.CreateAsync(permission);

                return new PermissionDTO
                {
                    Id = permissionCreado.Id,
                    PermissionType = permissionCreado.PermissionType,
                    RolId = permissionCreado.RolId
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear un nuevo permiso: {PermisionName}", PermissionDTO?.PermissionType ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el permiso", ex);
            }
        }

        // Método para validar el DTO
        private void ValidatePermission(PermissionDTO PermissionDTO)
        {
            if (PermissionDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto permiso no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(PermissionDTO.PermissionType))
            {
                _logger.LogWarning("Se intentó crear/actualizar un permiso con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del permiso es obligatorio");
            }
        }
        // Método para mapear un Permission a un PermissionDTO
        private PermissionDTO MapToDTO(Permission permission)
        {
            return new PermissionDTO
            {
                Id = permission.Id,
                PermissionType = permission.PermissionType,
                RolId = permission.RolId
            };
        }
        // Método para mapear un PermissionDTO a un Permission
        private Permission MapToEntity(PermissionDTO permissionDTO)
        {
            return new Permission
            {
                Id = permissionDTO.Id,
                PermissionType = permissionDTO.PermissionType,
                RolId = permissionDTO.RolId
            };
        }
        // Método para mapear una lista de Permission a una lista de PermissionDTO
        private IEnumerable<PermissionDTO> MapToDTOs(IEnumerable<Permission> permissions)
        {
            var permissionDTOs = new List<PermissionDTO>();
            foreach (var permission in permissions)
            {
                permissionDTOs.Add(MapToDTO(permission));
            }
            return permissionDTOs;
        }
        }
    }
