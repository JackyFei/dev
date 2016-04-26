using System;

namespace Common.Utils
{
    /// <summary>
    /// Static helper class to throw argument exception.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Throw <see cref="ArgumentNullException"/> if <param name="obj"/> is null.
        /// </summary>
        /// <param name="obj">Object to be check.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">Message of the exception, if not provided, a default message will be returned.</param>
        public static void ArgumentNotNull(object obj, string paramName, string message = null)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName, string.IsNullOrEmpty(message) ? string.Format(ResUtils.Guard_ArgumentNotNull, paramName) : message);
            }            
        }

        /// <summary>
        /// Throw <see cref="ArgumentException"/> if <param name="str"/> is null or empty string.
        /// </summary>
        /// <param name="str">String to be check.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="message">Message of the exception, if not provided, a default message will be returned.</param>
        public static void ArgumentNotNullOrEmpty(string str, string paramName, string message = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException(string.IsNullOrEmpty(message) ? string.Format(ResUtils.Guard_ArgumentNotNullOrEmpty, paramName) : message);
            }
        }

        /// <summary>
        /// Throw <see cref="ArgumentException"/> if <param name="obj"/> is not instance of type <param name="expectedType"/>.
        /// </summary>
        /// <param name="obj">Object to be check.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="expectedType">Expected type of the object.</param>
        /// <param name="message">Message of the exception, if not provided, a default message will be returned.</param>
        public static void InstanceOfType(object obj, string paramName, Type expectedType, string message = null)
        {
            ArgumentNotNull(obj, "obj");
            ArgumentNotNull(expectedType, "expectedType");

            if (obj.GetType() != expectedType)
            {
                throw new ArgumentException(string.IsNullOrEmpty(message) ? string.Format(ResUtils.Guard_InstanceOfType, paramName, expectedType) : message);
            }
        }

        /// <summary>
        /// Throw <see cref="ArgumentException"/> if <param name="objType"/> is not type <param name="expectedType"/>.
        /// </summary>
        /// <param name="objType">Type of object to be check.</param>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="expectedType">Expected type of the object.</param>
        /// <param name="message">Message of the exception, if not provided, a default message will be returned.</param>
        public static void InstanceOfType(Type objType, string paramName, Type expectedType, string message = null)
        {
            ArgumentNotNull(objType, "objType");
            ArgumentNotNull(expectedType, "expectedType");

            if (objType != expectedType)
            {
                throw new ArgumentException(string.IsNullOrEmpty(message) ? string.Format(ResUtils.Guard_InstanceOfType, paramName, expectedType) : message);
            }
        }
    }
}
