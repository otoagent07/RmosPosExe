using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pos.PavoModels
{
    public class PavoResponse
    {
        public bool success { get; set; } = true;
        public string message { get; set; } = "Başarılı";
        public object data { get; set; }
    }
}
