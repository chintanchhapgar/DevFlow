BEGIN;

-- Development reset: retain the DEV project and replace all project data
-- with a reporting-ready dataset.
DELETE FROM project."WorkItemLabels";
DELETE FROM project."Attachments";
DELETE FROM project."Comments";
DELETE FROM project."Worklogs";
DELETE FROM project."WorkItems";
DELETE FROM project."Epics";
DELETE FROM project."Labels";
DELETE FROM project."Sprints";
DELETE FROM project."ProjectInvitations";
DELETE FROM project."ProjectMembers";
DELETE FROM project."OutboxMessages";
DELETE FROM project."Projects" WHERE "Key" <> 'DEV';

UPDATE project."Projects"
SET "Name" = 'DevFlow',
    "Description" = 'Reporting demonstration project with delivery, workload, velocity, and burndown data.',
    "OwnerId" = '768555ed-710e-4247-9d07-1b09add9a24c',
    "Visibility" = 1,
    "Status" = 1,
    "UpdatedOnUtc" = NOW()
WHERE "Key" = 'DEV';

INSERT INTO project."ProjectMembers" ("Id", "UserId", "Role", "JoinedOnUtc", "ProjectId")
VALUES
  ('10000000-0000-0000-0000-000000000001', '768555ed-710e-4247-9d07-1b09add9a24c', 1, NOW(), '1d49286f-69b9-4647-8a5f-9f51c8dd753b'),
  ('10000000-0000-0000-0000-000000000002', '20133041-4bb6-4e31-946f-2f65bc7ff0ef', 2, NOW(), '1d49286f-69b9-4647-8a5f-9f51c8dd753b'),
  ('10000000-0000-0000-0000-000000000003', 'd50f05d4-1eef-4db7-bce7-24934d3a6be7', 3, NOW(), '1d49286f-69b9-4647-8a5f-9f51c8dd753b'),
  ('10000000-0000-0000-0000-000000000004', 'b30440f3-c1e4-4049-94bf-7cc491b4bedb', 3, NOW(), '1d49286f-69b9-4647-8a5f-9f51c8dd753b'),
  ('10000000-0000-0000-0000-000000000005', '792bc192-7f07-49b7-a922-8449af4edd14', 3, NOW(), '1d49286f-69b9-4647-8a5f-9f51c8dd753b'),
  ('10000000-0000-0000-0000-000000000006', 'f8fc324d-c21d-4844-b17e-eefb09b49a27', 4, NOW(), '1d49286f-69b9-4647-8a5f-9f51c8dd753b');

INSERT INTO project."Sprints" ("Id", "ProjectId", "Name", "Goal", "Status", "StartDate", "EndDate", "StartedOnUtc", "CompletedOnUtc", "IsDeleted", "CreatedOnUtc", "UpdatedOnUtc")
VALUES
  ('20000000-0000-0000-0000-000000000001', '1d49286f-69b9-4647-8a5f-9f51c8dd753b', 'Sprint 1 — Foundation', 'Establish the reporting foundation.', 3, CURRENT_DATE - 42, CURRENT_DATE - 29, NOW() - INTERVAL '42 days', NOW() - INTERVAL '29 days', FALSE, NOW() - INTERVAL '43 days', NOW() - INTERVAL '29 days'),
  ('20000000-0000-0000-0000-000000000002', '1d49286f-69b9-4647-8a5f-9f51c8dd753b', 'Sprint 2 — Reporting', 'Ship project reporting improvements.', 2, CURRENT_DATE - 6, CURRENT_DATE + 7, NOW() - INTERVAL '6 days', NULL, FALSE, NOW() - INTERVAL '7 days', NOW() - INTERVAL '1 day'),
  ('20000000-0000-0000-0000-000000000003', '1d49286f-69b9-4647-8a5f-9f51c8dd753b', 'Sprint 3 — Collaboration', 'Improve collaboration workflows.', 1, CURRENT_DATE + 8, CURRENT_DATE + 21, NULL, NULL, FALSE, NOW(), NULL);

INSERT INTO project."WorkItems" ("Id", "ProjectId", "Key", "Title", "Description", "Type", "Status", "Priority", "AssigneeId", "ReporterId", "EpicId", "ParentId", "SprintId", "EstimateHours", "DueDate", "IsDeleted", "CreatedOnUtc", "UpdatedOnUtc", "ChildCount")
VALUES
  ('30000000-0000-0000-0000-000000000001', '1d49286f-69b9-4647-8a5f-9f51c8dd753b', 'DEV-1', 'Create project foundation', 'Initial project structure and access setup.', 1, 5, 4, '20133041-4bb6-4e31-946f-2f65bc7ff0ef', '768555ed-710e-4247-9d07-1b09add9a24c', NULL, NULL, '20000000-0000-0000-0000-000000000001', 8, NOW() - INTERVAL '32 days', FALSE, NOW() - INTERVAL '42 days', NOW() - INTERVAL '35 days', 0),
  ('30000000-0000-0000-0000-000000000002', '1d49286f-69b9-4647-8a5f-9f51c8dd753b', 'DEV-2', 'Implement authentication', 'Secure sign-in and session handling.', 1, 5, 5, 'd50f05d4-1eef-4db7-bce7-24934d3a6be7', '768555ed-710e-4247-9d07-1b09add9a24c', NULL, NULL, '20000000-0000-0000-0000-000000000001', 13, NOW() - INTERVAL '31 days', FALSE, NOW() - INTERVAL '42 days', NOW() - INTERVAL '31 days', 0),
  ('30000000-0000-0000-0000-000000000003', '1d49286f-69b9-4647-8a5f-9f51c8dd753b', 'DEV-3', 'Build project dashboard', 'Show delivery health and project metrics.', 1, 5, 3, 'b30440f3-c1e4-4049-94bf-7cc491b4bedb', '768555ed-710e-4247-9d07-1b09add9a24c', NULL, NULL, '20000000-0000-0000-0000-000000000001', 8, NOW() - INTERVAL '30 days', FALSE, NOW() - INTERVAL '42 days', NOW() - INTERVAL '30 days', 0),
  ('30000000-0000-0000-0000-000000000004', '1d49286f-69b9-4647-8a5f-9f51c8dd753b', 'DEV-4', 'Add invitation flow', 'Invite team members into projects.', 1, 5, 4, '792bc192-7f07-49b7-a922-8449af4edd14', '768555ed-710e-4247-9d07-1b09add9a24c', NULL, NULL, '20000000-0000-0000-0000-000000000001', 5, NOW() - INTERVAL '29 days', FALSE, NOW() - INTERVAL '42 days', NOW() - INTERVAL '29 days', 0),
  ('30000000-0000-0000-0000-000000000005', '1d49286f-69b9-4647-8a5f-9f51c8dd753b', 'DEV-5', 'Report API integration', 'Connect summary, workload, velocity, and burndown reports.', 1, 3, 5, '20133041-4bb6-4e31-946f-2f65bc7ff0ef', '768555ed-710e-4247-9d07-1b09add9a24c', NULL, NULL, '20000000-0000-0000-0000-000000000002', 13, NOW() + INTERVAL '2 days', FALSE, NOW() - INTERVAL '6 days', NOW() - INTERVAL '1 day', 0),
  ('30000000-0000-0000-0000-000000000006', '1d49286f-69b9-4647-8a5f-9f51c8dd753b', 'DEV-6', 'Add workload visualisation', 'Display per-member estimates and assignments.', 1, 2, 4, 'd50f05d4-1eef-4db7-bce7-24934d3a6be7', '768555ed-710e-4247-9d07-1b09add9a24c', NULL, NULL, '20000000-0000-0000-0000-000000000002', 8, NOW() + INTERVAL '3 days', FALSE, NOW() - INTERVAL '6 days', NOW() - INTERVAL '2 days', 0),
  ('30000000-0000-0000-0000-000000000007', '1d49286f-69b9-4647-8a5f-9f51c8dd753b', 'DEV-7', 'Improve sprint velocity chart', 'Compare sprint commitment with completion.', 1, 1, 3, 'b30440f3-c1e4-4049-94bf-7cc491b4bedb', '768555ed-710e-4247-9d07-1b09add9a24c', NULL, NULL, '20000000-0000-0000-0000-000000000002', 5, NOW() + INTERVAL '5 days', FALSE, NOW() - INTERVAL '6 days', NULL, 0),
  ('30000000-0000-0000-0000-000000000008', '1d49286f-69b9-4647-8a5f-9f51c8dd753b', 'DEV-8', 'Resolve report export issue', 'Fix CSV field ordering for exported reports.', 2, 4, 4, '792bc192-7f07-49b7-a922-8449af4edd14', '768555ed-710e-4247-9d07-1b09add9a24c', NULL, NULL, '20000000-0000-0000-0000-000000000002', 3, NOW() + INTERVAL '1 day', FALSE, NOW() - INTERVAL '6 days', NOW(), 0),
  ('30000000-0000-0000-0000-000000000009', '1d49286f-69b9-4647-8a5f-9f51c8dd753b', 'DEV-9', 'Add notification centre', 'Surface project activity and mentions.', 1, 1, 2, 'f8fc324d-c21d-4844-b17e-eefb09b49a27', '768555ed-710e-4247-9d07-1b09add9a24c', NULL, NULL, '20000000-0000-0000-0000-000000000002', 8, NOW() + INTERVAL '6 days', FALSE, NOW() - INTERVAL '6 days', NULL, 0),
  ('30000000-0000-0000-0000-000000000010', '1d49286f-69b9-4647-8a5f-9f51c8dd753b', 'DEV-10', 'Prepare collaboration sprint', 'Break down planned collaboration work.', 1, 1, 3, '20133041-4bb6-4e31-946f-2f65bc7ff0ef', '768555ed-710e-4247-9d07-1b09add9a24c', NULL, NULL, '20000000-0000-0000-0000-000000000003', 5, NOW() + INTERVAL '14 days', FALSE, NOW(), NULL, 0);

INSERT INTO project."Worklogs" ("Id", "WorkItemId", "UserId", "Description", "StartedAtUtc", "EndedAtUtc", "MinutesSpent", "IsRunning", "CreatedOnUtc", "UpdatedOnUtc", "IsDeleted")
VALUES
  ('40000000-0000-0000-0000-000000000001', '30000000-0000-0000-0000-000000000001', '20133041-4bb6-4e31-946f-2f65bc7ff0ef', 'Project foundation', NOW() - INTERVAL '34 days', NOW() - INTERVAL '34 days' + INTERVAL '4 hours', 240, FALSE, NOW() - INTERVAL '34 days', NULL, FALSE),
  ('40000000-0000-0000-0000-000000000002', '30000000-0000-0000-0000-000000000002', 'd50f05d4-1eef-4db7-bce7-24934d3a6be7', 'Authentication flows', NOW() - INTERVAL '31 days', NOW() - INTERVAL '31 days' + INTERVAL '6 hours', 360, FALSE, NOW() - INTERVAL '31 days', NULL, FALSE),
  ('40000000-0000-0000-0000-000000000003', '30000000-0000-0000-0000-000000000005', '20133041-4bb6-4e31-946f-2f65bc7ff0ef', 'Report API wiring', NOW() - INTERVAL '1 day', NOW() - INTERVAL '1 day' + INTERVAL '3 hours', 180, FALSE, NOW() - INTERVAL '1 day', NULL, FALSE),
  ('40000000-0000-0000-0000-000000000004', '30000000-0000-0000-0000-000000000006', 'd50f05d4-1eef-4db7-bce7-24934d3a6be7', 'Workload UI', NOW() - INTERVAL '2 days', NOW() - INTERVAL '2 days' + INTERVAL '2 hours', 120, FALSE, NOW() - INTERVAL '2 days', NULL, FALSE),
  ('40000000-0000-0000-0000-000000000005', '30000000-0000-0000-0000-000000000008', '792bc192-7f07-49b7-a922-8449af4edd14', 'CSV export fix', NOW() - INTERVAL '4 hours', NOW() - INTERVAL '4 hours' + INTERVAL '90 minutes', 90, FALSE, NOW() - INTERVAL '4 hours', NULL, FALSE);

COMMIT;
