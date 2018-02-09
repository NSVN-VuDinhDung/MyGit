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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string json = @"{
                          'AMOUNT': 18.5,
                          'SUBMIT_DATE': '1995-4-7T00:00:00',
                          'DOSSIER_COUNT': 1,
                        'GRID_DETAIL' : [{
                          NAME: 'Ted',
                          AGE: 30
                        },  {
                           NAME: 'Dung',
                          AGE: 27
                        }],
                        'GRID_DETAIL_2' : [{
                          YEAR: 44,
                          OLD: 30
                        },  {
                           YEAR: 77,
                          OLD: 27
                        }]
                        }";

        private void Form1_Load(object sender, EventArgs e)
        {
            IEnumerable<FieldInWorkflowEntity> dsFieldInWorkFlow = GetFieldInWorkFlow();

            var workFlowStruct =  ConvertValueHelperHelper.CreateExpandoObject(dsFieldInWorkFlow);
            var obj = JsonConvert.DeserializeObject<DynamicDictionary>(json);
            var seri = JsonConvert.SerializeObject(obj);

            workFlowStruct.SetMember("AMOUNT", 12);

            var workFlowStruct1 = ConvertValueHelperHelper.CreateExpandoObject1(dsFieldInWorkFlow);
            var jsonWorkFlowStruct1 = JsonConvert.SerializeObject(workFlowStruct1);
            var expConverter = new ExpandoObjectConverter();
            ExpandoObject obj1 = JsonConvert.DeserializeObject<ExpandoObject>(json, expConverter);
            var seri1 = JsonConvert.SerializeObject(obj1);

            //((IDictionary<string, object>)((List<object>)propertyValues["ListItem"])[0])["name"]
            IDictionary<string, object> propertyValues = obj1;

            
            JObject job = new JObject();
            job.Add("a", 123);

            dynamic job1 = JsonConvert.DeserializeObject<JObject>(json);

            dynamic jsonObject = new JObject();
            jsonObject.Date = DateTime.Now;
            jsonObject.Album = "Me Against the world";
            jsonObject.Year = 1995;
            jsonObject.Artist = "2Pac";
            var seri2 = JsonConvert.SerializeObject(jsonObject);

            DataTable table = new DataTable();
            table.Columns.Add("FirstName", typeof(string));
            table.Columns.Add("LastName", typeof(string));
            table.Columns.Add("DateOfBirth", typeof(DateTime));
            
            dynamic row = table.NewRow().AsDynamic();
            row.FirstName = "John";
            row.LastName = "Doe";
            row.DateOfBirth = new DateTime(1981, 9, 12);
            table.Rows.Add(row.DataRow);

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

    public static class DynamicDataRowExtensions
    {
        public static dynamic AsDynamic(this DataRow dataRow)
        {
            return new DynamicDataRow(dataRow);
        }
    }

    public class DynamicDataRow : DynamicObject
    {
        private DataRow _dataRow;

        public DynamicDataRow(DataRow dataRow)
        {
            if ( dataRow == null )
                throw new ArgumentNullException("dataRow");
            this._dataRow = dataRow;
        }

        public DataRow DataRow
        {
            get
            {
                return _dataRow;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            if ( _dataRow.Table.Columns.Contains(binder.Name) )
            {
                result = _dataRow[binder.Name];
                return true;
            }
            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if ( _dataRow.Table.Columns.Contains(binder.Name) )
            {
                _dataRow[binder.Name] = value;
                return true;
            }
            return false;
        }
    }
}
