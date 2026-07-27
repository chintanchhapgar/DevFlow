using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFlow.Projects.Archive
{
    public sealed record ArchiveProjectResponse(
    Guid ProjectId,
    string Key,
    string Name,
    string Status);
}
