using DevFlow.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DevFlow.Project.Domain.WorkItems.Errors
{
    public static class WorkItemErrors
    {

        public static readonly AppError NotFound =
            AppError.NotFound(
                "WorkItem.NotFound",
                "Work item was not found.");

        public static readonly AppError Forbidden =
            AppError.Forbidden(
                "WorkItem.Forbidden",
                "You do not have permission to modify this work item.");
    }
}
