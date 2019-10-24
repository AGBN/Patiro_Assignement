using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Patiro_Assignment.CustomElements;

namespace Patiro_Assignment.Models
{
    public class User
    {
        public string Username { get; set; }
        public ICollection<UserRole> Roles { get; set; }

        public User(string username, UserRole initialRole)
        {
            Roles = new List<UserRole>();
            Roles.Add(initialRole);

            this.Username = username;
        }

        public override bool Equals(object obj)
        {
            User u = obj as User;

            if (u == null)
            {
                return false;
            }
            else
            {
                return this.Username.Equals(u.Username);
            }
        }
    }
}
