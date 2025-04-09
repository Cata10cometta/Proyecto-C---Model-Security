using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los permisos de roles en el sistema.
    /// </summary>
    public class RolPermissionBusiness
    {
        private readonly RolPermissionData _rolPermissionData;
        private readonly ILogger _logger;

        public RolPermissionBusiness(RolPermissionData rolPermissionData, ILogger logger)
        {
            _rolPermissionData = rolPermissionData;
            _logger = logger;
        }

        // Método para obtener todos los permisos de roles como DTOs
        public async Task<IEnumerable<RolPermissionDTO>> GetAllRolPermisionsAsync()
        {
            try
            {
                var rolPermissions = await _rolPermissionData.GetAllAsync();
                var rolPermissionsDTO = new List<RolPermissionDTO>();

                foreach (var rolPermission in rolPermissions)
                {
                    rolPermissionsDTO.Add(new RolPermissionDTO 
                    {
                        Id = rolPermission.Id,
                        RolId = rolPermission.RolId,
                        PermissionId = rolPermission.PermissionId
                    });
                }

                return rolPermissionsDTO;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los permisos de roles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de permisos de roles", ex);
            }
        }

        // Método para obtener un permiso de rol por ID como DTO
        public async Task<RolPermissionDTO> GetRolPermisionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un permiso de rol con ID inválido: {RolPermisionId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del permiso de rol debe ser mayor que cero");
            }

            try
            {
                var rolPermision = await _rolPermissionData.GetByIdAsync(id);
                if (rolPermision == null)
                {
                    _logger.LogInformation("No se encontró ningún permiso de rol con ID: {RolPermisionId}", id);
                    throw new EntityNotFoundException("RolPermision", id);
                }

                return new RolPermissionDTO
                {
                    Id = rolPermision.Id,
                    RolId = rolPermision.RolId,
                    PermissionId = rolPermision.PermissionId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el permiso de rol con ID: {RolPermisionId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el permiso de rol con ID {id}", ex);
            }
        }

        // Método para crear un permiso de rol desde un DTO
        public async Task<RolPermissionDTO> CreateRolPermissionAsync(RolPermissionDTO RolPermissionDTO)
        {
            try
            {
                ValidateRolPermission(RolPermissionDTO);

                var rolPermission = new RolPermission
                {
                    RolId = RolPermissionDTO.RolId,
                    PermissionId = RolPermissionDTO.PermissionId
                };

                var createdRolPermision = await _rolPermissionData.CreateAsync(rolPermission);

                return new RolPermissionDTO
                {
                    Id = createdRolPermision.Id,
                    RolId = createdRolPermision.RolId,
                    PermissionId = createdRolPermision.PermissionId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo rol: {RolNombre}", RolPermissionDTO?.RolId ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el rol", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateRolPermission(RolPermissionDTO RolPermissionDTO)
        {
            if (RolPermissionDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto rol no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(RolPermissionDTO.RolId))
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del rol es obligatorio");
            }

        }
        // Método para mapear de RolPermission a RolPermissionDTO
        private RolPermissionDTO MapToDTO(RolPermission rolPermission)
        {
            return new RolPermissionDTO
            {
                Id = rolPermission.Id,
                RolId = rolPermission.RolId,
                PermissionId = rolPermission.PermissionId
            };
        }
        // Método para mapear de RolPermissionDTO a RolPermission
        private RolPermission MapToEntity(RolPermissionDTO rolPermissionDTO)
        {
            return new RolPermission
            {
                Id = rolPermissionDTO.Id,
                RolId = rolPermissionDTO.RolId,
                PermissionId = rolPermissionDTO.PermissionId
            };
        }
        // Método para mapear una lista de RolPermission a una lista de RolPermissionDTO
        private IEnumerable<RolPermissionDTO> MapToDTOs(IEnumerable<RolPermission> rolPermissions)
        {
            var rolPermissionsDTO = new List<RolPermissionDTO>();
            foreach (var rolPermission in rolPermissions)
            {
                rolPermissionsDTO.Add(MapToDTO(rolPermission));
            }
            return rolPermissionsDTO;
        }
        }
}
