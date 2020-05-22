using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace qbotapi.Resources
{
    public class QuestionResource
    {
        public int id { get; set; }
        public string title{ get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public string answer2 { get; set; }
        public string source { get; set; }
        public string author { get; set; }
        public string comment { get; set; }
        public string imageLink { get; set; }
    }
}
