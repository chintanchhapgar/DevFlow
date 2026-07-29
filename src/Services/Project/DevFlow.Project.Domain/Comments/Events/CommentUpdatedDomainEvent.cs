using DevFlow.Project.Domain.Comments.ValueObjects;
using DevFlow.SharedKernel.Domain;
using System.ComponentModel.Design;

namespace DevFlow.Project.Domain.Comments.Events;

public sealed record CommentUpdatedDomainEvent(
    CommentId CommentId)
    : IDomainEvent;
