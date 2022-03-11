using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApplication.Application.ViewModels.GroupMember;
using Microsoft.Identity.Client;

namespace ChatApplication.Application.ViewModels.Group
{
    public class MyGroupGetDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public IEnumerable<GroupMemberDto> Members { get; set; }
    }
}
