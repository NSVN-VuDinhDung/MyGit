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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IEnumerable<FieldInWorkflowEntity> dsFieldInWorkFlow = GetFieldInWorkFlow();

            //dynamic expando = new ExpandoObject();
            //AddProperty(expando, "Name", typeof(Int32));
            //AddProperty(expando, "Old", typeof(string));
           
            //IDictionary<string, object> propertyValues = expando;

            //foreach ( var property in propertyValues.Keys )
            //{
            //    Console.WriteLine(String.Format("{0} : {1} : {2}", property, propertyValues[property], propertyValues[property].GetType().FullName));
            //}

            //foreach ( string propertyName in GetPropertyKeysForDynamic(expando) )
            //{
            //    object  propertyValue = expando[propertyName];
            //    // And
            //   // dynamicToGetPropertiesFor[propertyName] = "Your Value"; // Or an object value
            //}
        }

        public void AddProperty(ExpandoObject expando, string propertyName, Type propertyType)
        {
            object newInstance = null;
            if ( propertyType.IsValueType )
            {
                 newInstance = Activator.CreateInstance(propertyType);
            }
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if ( expandoDict.ContainsKey(propertyName) )
                expandoDict[propertyName] = newInstance;
            else
                expandoDict.Add(propertyName, newInstance);
            
        }

        public List<string> GetPropertyKeysForDynamic(dynamic dynamicToGetPropertiesFor)
        {
            JObject attributesAsJObject = dynamicToGetPropertiesFor;
            Dictionary<string, object> values = attributesAsJObject.ToObject<Dictionary<string, object>>();
            List<string> toReturn = new List<string>();
            foreach ( string key in values.Keys )
            {
                toReturn.Add(key);
            }
            return toReturn;
        }

        private IEnumerable<FieldInWorkflowEntity> GetFieldInWorkFlow()
        {
            DL dLBase = new DL();
            return dLBase.QueryList<FieldInWorkflowEntity>("MD_FIELD_GETALL", 1);
        }
    }
}
