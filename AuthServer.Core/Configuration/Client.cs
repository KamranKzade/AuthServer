using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Configuration;

public class Client
{
    public string Id { get; set; }
    public string Secret { get; set; }

    // Hansi Api -a muraciet ede biler, onu yaziriq burda
    public List<string> Audiences { get; set; }
}