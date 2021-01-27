using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ContactList.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactList.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class ContactListController : ControllerBase {
        private readonly ContactRepository _repository;

        public ContactListController(ContactRepository repository) {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Contact>))]
        public IActionResult GetAll() => Ok(_repository.GetAll());

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Contact))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Add([Required] [FromBody] Contact contact) {
            if (contact.Id < 1 || string.IsNullOrWhiteSpace(contact.Firstname) ||
                string.IsNullOrWhiteSpace(contact.Lastname) || string.IsNullOrWhiteSpace(contact.Email))
                return BadRequest("Invalid input");

            _repository.Add(contact);
            
            return CreatedAtRoute(nameof(Add), contact);
        }

        [HttpDelete]
        [Route("{personId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int personId) {
            if (personId < 1) return BadRequest("Invalid ID supplied");
            
            try {
                _repository.Delete(personId);
            }
            catch (ArgumentException) {
                return NotFound("Person not found");
            }

            return NoContent();
        }

        [HttpGet("findByName")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Contact>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult FindByName([Required] [FromQuery] string nameFilter) {
            IEnumerable<Contact> contacts;
            
            try {
                contacts = _repository.FindByName(nameFilter);
            }
            catch (ArgumentException) {
                return BadRequest("Invalid or missing name");
            }

            return Ok(contacts);
        }
    }
}
