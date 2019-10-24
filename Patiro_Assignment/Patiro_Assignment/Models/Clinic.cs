using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Patiro_Assignment.CustomElements;

namespace Patiro_Assignment.Models
{
    public class Clinic
    {
        [SetterRoles(UserRole.Admin)]
        public int Id { get; set; }

        [SetterRoles(UserRole.Admin, UserRole.Partner)]
        public string Name { get; set; }

        [SetterRoles(UserRole.Admin, UserRole.Partner)]
        public string Description { get; set; }

        [SetterRoles(UserRole.Admin, UserRole.Employee)]
        public bool IsActive { get; set; }

        [SetterRoles(UserRole.Admin, UserRole.Partner)]
        public string City { get; set; }

        [SetterRoles(UserRole.Admin, UserRole.Partner)]
        public string ZipCode { get; set; }

        [SetterRoles(UserRole.Admin, UserRole.Partner)]
        public ICollection<User> Members { get; set; }

        [SetterRoles()]
        public DateTimeOffset CreatedAt { get; set; }

        [SetterRoles()]
        public User CreatedBy { get; set; }


        /// <summary>
        /// Compares two Clinic objects. Returns true if it's a valid update.
        /// Inspiriration: https://stackoverflow.com/questions/506096/comparing-object-properties-in-c-sharp
        /// </summary>
        /// <param name="newC"></param>
        /// <param name="oldC"></param>
        /// <returns></returns>
        public static bool ValidateUpdate(Clinic newC, Clinic oldC, User user)
        {
            bool valid = true;
            Type type = typeof(Clinic);
            //Creates a list with the roles, this way any changes only happens inside this method.
            List<UserRole> roleList = user.Roles.ToList();

            // Iterate through all properties on the class.
            foreach (PropertyInfo pi in type.GetProperties(/*Bindingflags can be used to limit which properties should be returned.*/))
            {
                object newValue = type.GetProperty(pi.Name).GetValue(newC);
                object oldValue = type.GetProperty(pi.Name).GetValue(oldC);

                bool a = newValue != oldValue;
                bool b = (newValue == null || !newValue.Equals(oldValue));
                bool c = !CollectionCompare(newValue, oldValue);

                if (newValue != oldValue && (newValue == null || !newValue.Equals(oldValue)) && !CollectionCompare(newValue, oldValue))
                {
                    // Value has changed.
                    try
                    { 
                        // Check if the user has permission to change this property.
                        SetterRolesAttribute att = pi.GetCustomAttribute(typeof(SetterRolesAttribute)) as SetterRolesAttribute;


                        // Creator should be able to change everything, so if user is the creator he gets a temporary admin role, which is only valid in this method.
                        // Shoud probably compare on some sort of ID instead of username.
                        if (oldC.CreatedBy.Username.Equals(user.Username))
                        {
                            roleList.Add(UserRole.Admin);
                        }


                        // If the user is a partner but not a member of the clinic, temporarily remove his partner role.
                        if (!oldC.Members.Contains(user))
                        {
                            roleList.Remove(UserRole.Partner);
                        }
                        

                        // Check if the change is valid with the current roles.
                        if ( att.HasValidRole(roleList))
                        {
                            //User has permission, carry on.
                        }
                        else
                        {
                            // User does not have permission, cancel.
                            valid = false;
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        // Log something here.
                        valid = false;
                        break;
                    }
                    
                }
            }

            return valid;
        }

        /// <summary>
        /// Method tries to compare two objects as collections. If it fails they are either: Not collections or not equal.
        /// </summary>
        /// <param name="objectA"></param>
        /// <param name="objectB"></param>
        /// <returns></returns>
        private static bool CollectionCompare(object objectA, object objectB)
        {
            bool equal = false;
            ICollection<User> collectionA = objectA as ICollection<User>;
            ICollection<User> collectionB = objectB as ICollection<User>;

            if (collectionA != null && collectionB != null)
            {
                equal = collectionA.SequenceEqual(collectionB);
            }

            return equal;
        }
    }
}
