﻿using Business;
using Data;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;

namespace Web
{
    /// <summary>
    /// Controlador para la gestión de verificaciones en el sistema.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class VerificationController : ControllerBase
    {
        private readonly VerificationBusiness _VerificationBusiness;
        private readonly ILogger<VerificationController> _logger;

        /// <summary>
        /// Constructor del controlador de verificaciones.
        /// </summary>
        /// <param name="VerificationBusiness">Capa de negocios de verificaciones.</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public VerificationController(VerificationBusiness VerificationBusiness, ILogger<VerificationController> logger)
        {
            _VerificationBusiness = VerificationBusiness;
            _logger = logger;
        }
        /// <summary>
        /// Obtiene todas las verificaciones del sistema.
        /// </summary>
        /// <returns>Lista de verificaciones</returns>
        /// <response code="200">Lista de verificaciones</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<VerificationData>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllVerifications()
        {
            try
            {
                var verifications = await _VerificationBusiness.GetAllVerificationsAsync();
                return Ok(verifications);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener las verificaciones");
                return StatusCode(500, new { message = ex.Message });
            }
        }
        /// <summary>
        /// Obtiene una verificación por su ID.
        /// </summary>
        /// <param name="id">ID de la verificación</param>
        /// <returns>Verificación solicitada</returns>
        /// <response code="200">Retorna la verificación solicitada</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Verificación no encontrada</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(VerificationDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetVerificationById(int id)
        {
            try
            {
                var verification = await _VerificationBusiness.GetVerificationByIdAsync(id);
                return Ok(verification);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para la verificación con ID: {VerificationId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Verificación no encontrada con ID: {VerificationId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener verificación con ID: {VerificationId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
        /// <summary>
        /// Crea una verificación en el sistema.
        /// </summary>
        /// <para name="Verification">Datos de la verificación a crear</para>
        /// <returns>Verificación creada</returns>
        /// <response code="201">Retorna la verificación creada</response>
        /// <response code="400">Datos de la verificación no válidos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [ProducesResponseType(typeof(VerificationDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateVerification([FromBody] VerificationDTO Verification)
        {
            try
            {
                var createdVerification = await _VerificationBusiness.CreateVerificationAsync(Verification);
                return CreatedAtAction(nameof(GetVerificationById), new { id = createdVerification.Id }, createdVerification);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al crear verificación");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear verificación");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
