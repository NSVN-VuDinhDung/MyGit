using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class EntityMapper<T> : IResultSetMapper<T> where T : new()
    {
        /// <summary>
        /// When implemented by a class, returns an enumerable of <typeparamref name="TResult" /> based on <paramref name="reader" />.
        /// </summary>
        /// TODO: Performance review
        /// <param name="reader">The <see cref="T:System.Data.IDataReader" /> to map.</param>
        /// <returns>
        /// The enurable of <typeparamref name="TResult" /> that is based on <paramref name="reader" />.
        /// </returns>
        /// <Author>DungVD</Author>
        /// <ModifiedDate>7/3/2017</ModifiedDate>
        public IEnumerable<T> MapSet(IDataReader reader)
        {
            while ( reader.Read() )
            {
                T objectInstance = Activator.CreateInstance<T>();

                for ( int index = 0; index < reader.FieldCount; index++ )
                {
                    var fieldName = reader.GetName(index);
                    PropertyInfo info = objectInstance.GetType().GetProperty(fieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if ( (info != null) )
                    {
                        object value = DBNull.Value;
                        value = reader.GetValue(index);
                        value = ConvertValue(value, info.PropertyType);
                        info.SetValue(objectInstance, value, null);
                        //var value = reader[index];
                        //if (value == DBNull.Value)
                        //{
                        //    value = GetDefault(info.PropertyType);
                        //}
                        //info.SetValue(objectInstance, value, null);
                    }
                }

                yield return objectInstance;
            }
        }


        public static object GetDefault(Type type)
        {
            if ( type.IsValueType )
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
        /// <summary>
        /// Determines whether [is nullable type] [the specified typeOfObject].
        /// </summary>
        /// <param name="typeOfObject">The type of object</param>
        /// <returns></returns>
        /// <Author>DungVD</Author>
        /// <ModifiedDate>7/3/2017</ModifiedDate>
        private bool IsNullableType(Type typeOfObject)
        {
            return typeOfObject.IsGenericType && typeOfObject.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Converts the nullable value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="conversionType">Type of the conversion.</param>
        /// <returns></returns>
        /// <Author>DungVD</Author>
        /// <ModifiedDate>7/3/2017</ModifiedDate>
        protected object ConvertNullableValue(object value, Type conversionType)
        {
            if ( value != DBNull.Value )
            {
                NullableConverter nullableConverter = new NullableConverter(conversionType);
                return nullableConverter.ConvertFrom(value);
            }
            return null;
        }

        /// <summary>
        /// Converts the non nullable value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="conversionType">Type of the conversion.</param>
        /// <returns></returns>
        /// <Author>DungVD</Author>
        /// <ModifiedDate>7/3/2017</ModifiedDate>
        protected object ConvertNonNullableValue(object value, Type conversionType)
        {
            object result = null;
            if ( value != DBNull.Value )
            {
                result = Convert.ChangeType(value, conversionType, CultureInfo.CurrentCulture);
            }
            else if ( conversionType.IsValueType )
            {
                result = Activator.CreateInstance(conversionType);
            }
            return result;
        }

        /// <summary>
        /// Converts the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="conversionType">Type of the conversion.</param>
        /// <returns></returns>
        /// <Author>DungVD</Author>
        /// <ModifiedDate>7/3/2017</ModifiedDate>
        protected object ConvertValue(object value, Type conversionType)
        {
            if ( IsNullableType(conversionType) )
            {
                return ConvertNullableValue(value, conversionType);
            }
            return ConvertNonNullableValue(value, conversionType);
        }
    }
}
