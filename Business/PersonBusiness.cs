using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los roles del sistema.
    /// </summary>
    public class PersonBusiness
    {
        private readonly PersonData _rolData;
        private readonly PersonData _PersonData;
        private readonly ILogger _logger;

        public PersonBusiness(PersonData PersonData, ILogger logger)
        {
            _PersonData = PersonData;
            _logger = logger;
        }

        // Método para obtener todos los roles como DTOs
        public async Task<IEnumerable<PersonDTO>> GetAllPersonsAsync()
        {
            try
            {
                var persons = await _PersonData.GetAllAsync();
                var personsDTO = new List<PersonDTO>();

                foreach (var person in persons)
                {
                    personsDTO.Add(new PersonDTO
                    {
                        Id = person.Id,
                        Name = person.Name,
                        Email = person.Email,
                        Phone = person.Phone,
                        
                    });
                }

                return personsDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos las personas");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de personas", ex);
            }
        }

        // Método para obtener un rol por ID como DTO
        public async Task<PersonDTO> GetRolByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un person con ID inválido: {personId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del person debe ser mayor que cero");
            }

            try
            {
                var person = await _rolData.GetByIdAsync(id);
                if (person == null)
                {
                    _logger.LogInformation("No se encontró ningún person con ID: {PersonId}", id);
                    throw new EntityNotFoundException("Person", id);
                }

                return new PersonDTO
                {
                    Id = person.Id,
                    Name = person.Name,
                    Email = person.Email,
                    Phone = person.Phone,
                    
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el person con ID: {personId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el person con ID {id}", ex);
            }
        }

        // Método para crear un rol desde un DTO
        public async Task<PersonDTO> CreatePersonAsync(PersonDTO PersonDTO)
        {
            try
            {
                ValidatePerson(PersonDTO);

                var person = new Person
                {
                    Name = PersonDTO.Name,
                    Email = PersonDTO.Email,
                    Phone = PersonDTO.Phone,
                    

                };

                var personCreado = await _PersonData.CreateAsync(person);

                return new PersonDTO
                {
                    Id = personCreado.Id,
                    Name = personCreado.Name,
                    Email = personCreado.Email,
                    Phone = personCreado.Phone,
                    
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo rol: {RolNombre}", PersonDTO?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el person", ex);
            }
        }

        // Método para validar el DTO
        private void ValidatePerson(PersonDTO PersonDTO)
        {
            if (PersonDTO == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto person no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(PersonDTO.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un person con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del person es obligatorio");
            }
        }
        // Método para mapear de Person a PersonDTO
        private PersonDTO MapToDTO(Person person)
        {
            return new PersonDTO
            {
                Id = person.Id,
                Name = person.Name,
                Email = person.Email,
                Phone = person.Phone,

            };
        }
        // Método para mapear de PersonDTO a Person
        private Person MapToEntity(PersonDTO personDTO)
        {
            return new Person
            {
                Id = personDTO.Id,
                Name = personDTO.Name,
                Email = personDTO.Email,
                Phone = personDTO.Phone,
            };
        }
        // Método para mapear una lista de Person a una lista de PersonDTO
        private IEnumerable<PersonDTO> MapToDTOs(IEnumerable<Person> persons)
        {
            var personDTOs = new List<PersonDTO>();
            foreach (var person in persons)
            {
                personDTOs.Add(MapToDTO(person));
            }
            return personDTOs;
        }
        }
    }
