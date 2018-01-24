using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class ConvertValueHelperHelper
    {
        public ExpandoObject CreateExpandoObject(IEnumerable<FieldInWorkflowEntity> fieldInWorkflow)
        {
            dynamic expando = new ExpandoObject();

            return expando;
            //object newInstance = null;
            //if ( propertyType.IsValueType )
            //{
            //    newInstance = Activator.CreateInstance(propertyType);
            //}
            //// ExpandoObject supports IDictionary so we can extend it like this
            //var expandoDict = expando as IDictionary<string, object>;
            //if ( expandoDict.ContainsKey(propertyName) )
            //    expandoDict[propertyName] = newInstance;
            //else
            //    expandoDict.Add(propertyName, newInstance);

        }

        public void AddProperty(ExpandoObject expandoObject, IEnumerable<FieldInWorkflowEntity> fieldInWorkflow)
        {
            if ( fieldInWorkflow == null )
                return;

            foreach ( FieldInWorkflowEntity item in fieldInWorkflow )
            {
                object newInstance = null;


                // ExpandoObject supports IDictionary so we can extend it like this
                //var expandoDict = expando as IDictionary<string, object>;
                //if ( expandoDict.ContainsKey(propertyName) )
                //    expandoDict[propertyName] = newInstance;
                //else
                //    expandoDict.Add(propertyName, newInstance);

                //switch ( (FieldType)item.FIELD_TYPE_ID )
                //{
                //    case FieldType.StringType:

                //        newInstance = Activator.CreateInstance(typeof(string));

                //        break;

                //    default:
                //        break;
                //}
            }

        }


    }
}
