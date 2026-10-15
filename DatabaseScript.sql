
-- Database: GitHubSearchDb

-- Create the GitHubRepositories table
CREATE TABLE IF NOT EXISTS "GitHubRepositories" (
    "Id"              BIGINT PRIMARY KEY,
    "Username"        VARCHAR(255) NULL,
    "Name"            VARCHAR(255) NOT NULL,
    "NodeId"          VARCHAR(255) NULL,
    "StargazersCount" INTEGER NOT NULL DEFAULT 0,
    "RepositoryUrl"   VARCHAR(500) NULL,
    "Description"     TEXT NULL,
    "Language"        VARCHAR(100) NULL,
    "CreatedAtUtc"    TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP
);

