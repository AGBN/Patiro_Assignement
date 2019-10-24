using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Patiro_Assignment.Models;
using Patiro_Assignment.CustomElements;

namespace Patiro_Assignment
{
    public class Storage
    {
        public static Storage _instance;

        public static Storage Instance
        {
            get
            {
                if (_instance == null)
                {
                    Instance = new Storage();
                }

                return _instance;
            }

            set { _instance = value; }
        }

        private ICollection<User> users;
        private Clinic clinic;

        public Storage()
        {
            // Create users for the list.
            users = new List<User>();
            users.Add(new User("Admin", UserRole.Admin));
            users.Add(new User("Employee", UserRole.Employee));
            users.Add(new User("Partner", UserRole.Partner));
            users.Add(new User("Creator", UserRole.Employee));

            clinic = new Clinic()
            {
                Id = 1,
                Name = "Test",
                Description = "Testing Desc",
                IsActive = true,
                City = "Aalborg",
                ZipCode = "9000",
                Members = new List<User>(),
                CreatedAt = new DateTimeOffset(2019,10,23,20,15,10, TimeSpan.Zero),
                CreatedBy = users.Last()
            };
        }

        public ICollection<User> GetUsers()
        {
            return users;
        }

        public User GetUser(string id)
        {
            return users.Where(x => id.Equals(x.Username)).FirstOrDefault();
        }

        public Clinic GetClinic()
        {
            return clinic;
        }

        public void UpdateClinic(Clinic newClinic)
        {
            this.clinic = newClinic;
        }
    }
}
