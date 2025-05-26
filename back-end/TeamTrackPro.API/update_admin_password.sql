-- First, check the current password hash
SELECT Username, PasswordHash FROM Users WHERE Username = 'admin';

-- Update the admin password with a properly hashed value
UPDATE Users 
SET PasswordHash = '$2a$11$cF.MHMW/3xG7PXqBnuIKLuYvL2ofoGH9cNAnjyKPZUjvKRNk5xJNy'
WHERE Username = 'admin';

-- Verify the update
SELECT Username, PasswordHash FROM Users WHERE Username = 'admin'; 