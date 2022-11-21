using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.Getir.Class
{
    public class GetirResponseChangeLog
    {
        public class Root
        {
            public List<Section> sections { get; set; }
        }

        public class Name
        {
            public string tr { get; set; }
            public string en { get; set; }
        }

        public class Type
        {
            public string tr { get; set; }
            public string en { get; set; }
        }

        public class Title
        {
            public string tr { get; set; }
            public string en { get; set; }
        }

        public class Description
        {
            public string tr { get; set; }
            public string en { get; set; }
        }

        public class Datum
        {
            public Type type { get; set; }
            public Title title { get; set; }
            public DateTime date { get; set; }
            public Description description { get; set; }
        }

        public class Version
        {
            public Name name { get; set; }
            public List<Datum> data { get; set; }
        }

        public class Section
        {
            public Name name { get; set; }
            public List<Version> versions { get; set; }
        }
    }

  
}
