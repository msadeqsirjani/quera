using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApplication.Application.ViewModels.Group
{
    public class CreateGroupResponseDto
    {
        public GroupDto Group { get; set; }

        public class GroupDto
        {
            public int Id { get; set; }
        }
    }
}
