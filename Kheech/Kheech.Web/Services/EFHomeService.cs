using Kheech.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kheech.Web.Services
{
    public class EFHomeService
    {
        private readonly ApplicationDbContext _context;

        public EFHomeService(ApplicationDbContext context)
        {
            _context = context;
        }

    }
}