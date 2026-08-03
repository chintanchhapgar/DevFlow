using DevFlow.SharedKernel.Domain.DomainEvents;
using DevFlow.Project.Domain.Comments.ValueObjects;
using System.ComponentModel.Design;

namespace DevFlow.Project.Domain.Comments.Events;

public sealed record CommentUpdatedDomainEvent(
    CommentId CommentId)
    : IDomainEvent;
