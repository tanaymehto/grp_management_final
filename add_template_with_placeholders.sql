-- Add new template with multiple placeholders
-- Template: Meeting Invitation with Date, Time, and Location placeholders

INSERT INTO Templates (TemplateName, TemplateMsg, TemplateType, Placeholders)
VALUES (
    'Meeting Invitation',
    'Dear Team,

You are invited to attend an important meeting.

Meeting Details:
Date: {{Date}}
Time: {{Time}}
Location: {{Location}}

Please confirm your attendance by replying to this message.

Best regards,
Management Team',
    'General',
    '{"Date": "Meeting date (e.g., Monday, January 15, 2024)", "Time": "Meeting time (e.g., 2:00 PM)", "Location": "Meeting location (e.g., Conference Room A, Building 3)"}'
);

-- Add another template with different placeholders
INSERT INTO Templates (TemplateName, TemplateMsg, TemplateType, Placeholders)
VALUES (
    'Project Update',
    'Hello {{Name}},

This is an update regarding your project {{ProjectName}}.

Current Status: {{Status}}
Next Deadline: {{Deadline}}

Please review and let us know if you have any questions.

Regards,
Project Team',
    'Frequent',
    '{"Name": "Employee name", "ProjectName": "Project name", "Status": "Current project status", "Deadline": "Next deadline date"}'
);

PRINT 'Templates with multiple placeholders have been added successfully!';
PRINT '1. Meeting Invitation (3 placeholders: Date, Time, Location)';
PRINT '2. Project Update (4 placeholders: Name, ProjectName, Status, Deadline)';
