using System;
using System.Collections.Generic;
using System.Text;

namespace Pospos.Core.Attributes
{
    public class TableName : Attribute
    {
        public string SchemeName { get; set; }
        public string Name { get; set; }
        public TableName(string name,string schemeName = "dbo")
        {
            SchemeName = schemeName;
            Name = name;
        }
    }

    public class ColumnName : Attribute
    {
        public string Name { get; set; }
        public ColumnName(string name)
        {
            Name = name;
        }
    }

    public class ClaimName : Attribute
    {
        public ClaimName(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
    }

    public class IgnoreParameter : Attribute { }
}
