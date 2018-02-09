using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
    public static class ConvertValueHelperHelper
    {
        public static DynamicDictionary CreateExpandoObject(IEnumerable<FieldInWorkflowEntity> fieldInWorkflow)
        {
            dynamic expandoObject = new DynamicDictionary();

            AddProperty(expandoObject, fieldInWorkflow);

            return expandoObject;

        }

        public static void AddProperty(DynamicDictionary expandoObject, IEnumerable<FieldInWorkflowEntity> fieldInWorkflow)
        {
            if ( fieldInWorkflow == null )
                return;
            
            //var expandoDict = expandoObject as IDictionary<string, object>;

            foreach ( FieldInWorkflowEntity item in fieldInWorkflow )
            {
                expandoObject.SetMember(item.FIELD_NAME, null);
            }

        }

        public static ExpandoObject CreateExpandoObject1(IEnumerable<FieldInWorkflowEntity> fieldInWorkflow)
        {
            dynamic expandoObject = new ExpandoObject();

            AddProperty(expandoObject, fieldInWorkflow);

            return expandoObject;

        }

        public static void AddProperty(ExpandoObject expandoObject, IEnumerable<FieldInWorkflowEntity> fieldInWorkflow)
        {
            if ( fieldInWorkflow == null )
                return;

            var expandoDict = expandoObject as IDictionary<string, object>;

            var gridControl = fieldInWorkflow.Where(i => (ControlType)i.CONTROL_TYPE_ID == ControlType.GridControl).ToList();

            Dictionary<string, Dictionary<string, object>> columnInGrid = new Dictionary<string, Dictionary<string, object>>();

            foreach ( FieldInWorkflowEntity item in gridControl )
            {
                var items = fieldInWorkflow.Where(i => i.GRID_ID == item.FIELD_NAME);

                Dictionary<string, object> column = new Dictionary<string, object>();

                foreach ( var field in items )
                {
                    column.Add(field.FIELD_NAME, null);
                }

                if ( column.Count > 0 )
                {
                    columnInGrid.Add(item.FIELD_NAME, column);
                }
            }

            foreach ( FieldInWorkflowEntity item in fieldInWorkflow )
            {
                if ( !string.IsNullOrEmpty(item.GRID_ID) )
                    continue;

                if ( (ControlType)item.CONTROL_TYPE_ID == ControlType.GridControl )
                {
                    expandoDict[item.FIELD_NAME] = new List<object> { columnInGrid[item.FIELD_NAME] };
                }
                else
                {
                    expandoDict[item.FIELD_NAME] = null;
                }

            }

            var seri1 = JsonConvert.SerializeObject(expandoDict);
            var expConverter = new ExpandoObjectConverter();
            ExpandoObject obj1 = JsonConvert.DeserializeObject<ExpandoObject>(seri1, expConverter);
        }

        public static void AddDataToExpandoObject(ExpandoObject expandoObjectStruct, ExpandoObject expandoObjectContent)
        {
            IDictionary<string, object> dicObjectStruct = expandoObjectStruct;
            IDictionary<string, object> dicObjectContent = expandoObjectContent;
            foreach ( string key in dicObjectStruct.Keys )
            {
                if ( dicObjectContent.ContainsKey(key) )
                {
                    dicObjectStruct[key] = dicObjectContent[key];
                }
            }
        }
        private static object GetDefaultValue(int fieldType)
        {
            object value = null;

            switch ( (FieldType)fieldType )
            {
                case FieldType.StringType:
                    value = Activator.CreateInstance(typeof(string));
                    break;

                case FieldType.NumberType:
                    value = Activator.CreateInstance(typeof(int));
                    break;

                case FieldType.DecimalType:
                    value = Activator.CreateInstance(typeof(decimal));
                    break;

                case FieldType.DateTimeType:
                    value = Activator.CreateInstance(typeof(DateTime));
                    break;
                
                default:
                    break;
            }

            return value;
        }


    }
}
