using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF.IP.Application.Models
{
    public class BaseResultDTO
    {  
        public BaseResultDTO()
        {
            Errors = new List<string>();
        }

        public bool IsSuccesfull { get; set; }

        public List<string> Errors { get; set; }

       
    }
}
