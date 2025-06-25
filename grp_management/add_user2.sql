-- Add new user named 'user2'
-- Using the same password hash as existing users (password: 'password')
INSERT INTO Users (Username, PasswordHash, Email, Role, CreatedAt)
VALUES ('user2', '$2a$12$lA02SuNwzu/8BuV2dE.97uJpkUPcr9VJvPggsoxPpL.VJZpmLOb5y', 'user2@example.com', 'Member', GETDATE());

-- Get the new user's ID
DECLARE @NewUserId INT = SCOPE_IDENTITY();

-- Add a contact record for user2 (assuming they need to be linked to an employee)
-- You may need to adjust the EmployeeId based on your existing employee data
-- For now, I'll use EmployeeId = 2 (adjust this to match an existing employee)
INSERT INTO Contacts (UserId, EmployeeId, UserName, AccessLevel, AddedAt)
VALUES (@NewUserId, 2, 'user2', 'Full', GETDATE());

PRINT 'User2 has been added successfully with UserId: ' + CAST(@NewUserId AS VARCHAR(10)); 