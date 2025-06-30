using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class AssignedTaskViewModel
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public DateTime? DueDate { get; set; }  
    }
}
