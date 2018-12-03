using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DAL.Extensions
{
/// <summary>
///     Generic reader
/// </summary>
    public static class NullTypeSafe
    {
        /// <summary>
        ///     Extension method for getting fields without worrying about nulls
        /// </summary>
        public static T Get<T>(this IDataRecord row, string fieldName)
        {
            int ordinal = row.GetOrdinal(fieldName);
            return row.Get<T>(ordinal);
        }
        /// <summary>
        ///    Extension method for getting fields without worrying about nulls
        /// </summary>
        public static T Get<T>(this IDataRecord row, int ordinal)
        {
            var value = row.IsDBNull(ordinal) ? default(T) : row.GetValue(ordinal);
            return (T)Convert.ChangeType(value, typeof(T));
        }
        /// <summary>
        /// Generically extracts a field value by name from any IDataRecord as specified type. Will return default generic types value if DNE.
        /// </summary>
        public static T GetValueOrDefault<T>(this IDataRecord row, string fieldName, T defaultValue = default(T))
            => row.GetValueOrDefault<T>(row.GetOrdinal(fieldName), defaultValue);

        /// <summary>
        /// Generically extracts a field value by ordinal from any IDataRecord as specified type. Will return default generic types value if DNE.
        /// </summary>
        public static T GetValueOrDefault<T>(this IDataRecord row, int ordinal, T defaultValue = default(T))
            => (T)(row.IsDBNull(ordinal) ? defaultValue : row.GetValue(ordinal));

    }
}