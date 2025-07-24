using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public interface IPasswordHasher
    {
        public string Generate(string password);
        public bool Verify(string? password, string? hashedPassword);
    }
}
