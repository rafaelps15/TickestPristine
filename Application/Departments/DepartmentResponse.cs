using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tickest.Application.Departments
{
    public sealed class DepartmentResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ResponsibleUserName { get;  set; }
        public List<string> SectorNames { get; set; }
    }
}
