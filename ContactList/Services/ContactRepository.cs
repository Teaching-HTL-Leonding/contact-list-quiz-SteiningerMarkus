using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ContactList.Services {
    public record Contact([Required] int Id, string Firstname, string Lastname, [Required] string Email);

    public class ContactRepository {
        private List<Contact> Contacts { get; } = new();

        /// <summary>
        /// Get all people in contact list
        /// </summary>
        public IEnumerable<Contact> GetAll() => Contacts;

        /// <summary>
        /// Adds a new person to the list of contacts
        /// </summary>
        /// <param name="contact">Person to add</param>
        public void Add(Contact contact) => Contacts.Add(contact);

        /// <summary>
        /// Deletes a person from the list of contacts
        /// </summary>
        /// <param name="id">ID of person to delete</param>
        public void Delete(int id) => Contacts.Remove(Contacts.FirstOrDefault(c => c.Id == id));

        /// <summary>
        /// Finds person in contact list by name
        /// </summary>
        public IEnumerable<Contact> FindByName(string nameFilter) {
            List<Contact> contacts = Contacts.Where(c => c.Firstname.Contains(nameFilter) || c.Lastname.Contains(nameFilter))
                .ToList();
            if (!contacts.Any())
                throw new ArgumentException("Invalid or missing name");
            return contacts;
        }
    }
}
