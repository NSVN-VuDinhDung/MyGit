using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public enum FieldType
    {
        [Description("Kiểu số nguyên")]
        NumberType = 0,

        [Description("Kiểu số thập phân")]
        DecimalType = 1,

        [Description("Kiểu chuỗi")]
        StringType = 2,

        [Description("Kiểu ngày tháng")]
        DateTimeType = 3,

    }

    public enum ControlType
    {
        [Description("Kiểu control dạng grid")]
        GridControl = 0,

        [Description("Kiểu control dạng grid")]
        ComboboxControl = 1,

        [Description("Kiểu control dạng grid")]
        LabelControl = 2,

        [Description("Kiểu control dạng grid")]
        TextboxControl = 3,
    }
}
