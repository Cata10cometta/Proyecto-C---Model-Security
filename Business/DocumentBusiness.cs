using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los documentos del sistema.
    /// </summary>
    public class DocumentBusiness
    {
        private readonly DocumentData _documentData;
        private readonly ILogger _logger;

        public DocumentBusiness(DocumentData documentData, ILogger logger)
        {
            _documentData = documentData;
            _logger = logger;
        }

        // Método para obtener todos los documentos como DTOs
        public async Task<IEnumerable<DocumentDTO>> GetAllDocumentsAsync()
        {
            try
            {
                var documents = await _documentData.GetAllAsync();
                var documentDTOs = new List<DocumentDTO>();

                foreach (var document in documents)
                {
                    documentDTOs.Add(new DocumentDTO
                    {
                        Id = document.Id,
                        Url = document.Url,
                        Name = document.Name,
                        ExperienceId = document.ExperienceId

                    });
                }

                return MapToDTOList(documents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los documentos");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de documentos", ex);
            }
        }

        // Método para obtener un documento por ID como DTO
        public async Task<DocumentDTO> GetDocumentByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un documento con ID inválido: {DocumentId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del documento debe ser mayor que cero");
            }

            try
            {
                var document = await _documentData.GetByIdAsync(id);
                if (document == null)
                {
                    _logger.LogInformation("No se encontró ningún documento con ID: {DocumentId}", id);
                    throw new EntityNotFoundException("Document", id);
                }

                 return new DocumentDTO

                {
                    Id = document.Id,
                    Url = document.Url,
                    Name = document.Name,
                    ExperienceId = document.ExperienceId
                };
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el documento con ID: {DocumentId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el documento con ID {id}", ex);
            }
        }

        // Método para crear un documento desde un DTO
        public async Task<DocumentDTO> CreateDocumentAsync(DocumentDTO documentDTO)
        {
            try
            {
                ValidateDocument(documentDTO);

                var document = new Document
                {
                   
                        Url = documentDTO.Url,
                        Name = documentDTO.Name,
                        ExperienceId = documentDTO.ExperienceId

                };

                var DocumentCreado = MapToEntity(documentDTO);

                return new DocumentDTO
                {
                  Id = DocumentCreado.Id,
                  Url = DocumentCreado.Url,
                  Name = DocumentCreado.Name,
                  ExperienceId = DocumentCreado.ExperienceId    
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo documento: {DocumentNombre}", documentDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el documento", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateDocument(DocumentDTO documentDTO)
        {
            if (documentDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto documento no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(documentDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un documento con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Title", "El Name del documento es obligatorio");
            }
        }
        //Método para mapear de Document a DocumentDTO

        private DocumentDTO MapToDTO(Document document)
        {
            return new DocumentDTO
            {
                Id = document.Id,
                Url = document.Url,
                Name = document.Name,
                ExperienceId = document.ExperienceId    
            };
        }

        //Método para mapear de DocumentDTO a Document
        private Document MapToEntity(DocumentDTO documentDTO)
        {
            return new Document
            {
                Id = documentDTO.Id,
                Url = documentDTO.Url,
                Name = documentDTO.Name,
                ExperienceId = documentDTO.ExperienceId 
            };
        }

        //Método para mapear una lista de Document a una lista de DocumentDTO

        private IEnumerable<DocumentDTO> MapToDTOList(IEnumerable<Document> documents)
        {
            var documentsDTO = new List<DocumentDTO>();
            foreach (var document in documents)
            {
                documentsDTO.Add(MapToDTO(document));
            }
            return documentsDTO;
        }
    }
}

