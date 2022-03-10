
using System.Collections.Generic;

namespace SF.IP.Application.Models;

public record BaseResultDTO
{
    public BaseResultDTO()
    {
        Errors = new List<string>();
    }

    public bool IsSuccesfull { get; set; }

    public List<string> Errors { get; set; }
}

