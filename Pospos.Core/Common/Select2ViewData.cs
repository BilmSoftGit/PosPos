using System.Collections.Generic;

namespace Pospos.Core.Common
{
    public class Select2ViewData
    {
        public List<Select2InData> results { get; set; }
        public bool pagination { get; set; }
    }

    public class Select2InData
    {
        public int id { get; set; }
        public string text { get; set; }
    }
}
