using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Patiro_Assignment.CustomElements
{
    /// <summary>
    /// Attribute that marks which roles can change this property. leave empty, if this property is not allowed to be changed by anyone.
    /// Ispiration: https://stackoverflow.com/questions/52795404/c-sharp-custom-attribute-validation-in-console-environment
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SetterRolesAttribute : Attribute
    {
        private IEnumerable<UserRole> ValidRoles;

        public SetterRolesAttribute(params UserRole[] roles)
        {
            this.ValidRoles = roles;
        }

        // Checks if the given collection of roles contains at least one of the valid roles given in the attribute.
        public bool HasValidRole(ICollection<UserRole> roles)
        {
            bool hasValidRole = false;

            foreach (UserRole x in ValidRoles)
            {
                if (roles.Contains(x))
                {
                    hasValidRole = true;
                    break;
                }
            }
            return hasValidRole;
        }
    }
}
