CREATE TABLE Experience (
    Id INT IDENTITY(1,1) PRIMARY KEY,
	NameExperiences VARCHAR(50) NOT NULL,
	Summary TEXT(500) NOT NULL,
	Methodologies TEXT(500) NOT NULL,
	[Transfer] TEXT(500) NOT NULL,
	DateRegistration DATETIME2 NOT NULL DEFAULT GETDATE(),
	Code CHAR(10) NOT NULL UNIQUE,
	UserId INT NOT NULL,
	InstitutionId INT NOT NULL,
	DeleteAt DATETIME NOT NULL,
    CreateAt DATETIME NOT NULL DEFAULT GETDATE()
)

CREATE TABLE UserRol (
    Id INT IDENTITY(1,1) PRIMARY KEY,
	UserId INT NOT NULL,
	RolId INT NOT NULL,
	DeleteAt DATETIME NOT NULL,
    CreateAt DATETIME NOT NULL DEFAULT GETDATE()
)

CREATE TABLE HistoryExperience (
    Id INT IDENTITY(1,1) PRIMARY KEY,
	[Action] VARCHAR(255) NOT NULL,
	[DateTime] DATETIME NOT NULL,
	TableName VARCHAR(255) NOT NULL,
	UserId INT NOT NULL,
	ExperienceId INT NOT NULL,

)

CREATE TABLE [User] (
    Id INT IDENTITY(1,1) PRIMARY KEY ,
	[Name] VARCHAR(50) NOT NULL,
	Email VARCHAR(255) UNIQUE NOT NULL,
	[Password] VARCHAR(255) NOT NULL,
	DeleteAt DATETIME NOT NULL,
    CreateAt DATETIME NOT NULL DEFAULT GETDATE()
	
)

CREATE TABLE [State] (
    Id  INT PRIMARY KEY IDENTITY(1,1),
	[Name] VARCHAR(50) NOT NULL
	
)


CREATE TABLE Person (
    Id INT IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR(50) NOT NULL,
	Email VARCHAR(255) UNIQUE NOT NULL,
	Phone VARCHAR(15) NOT NULL,
	DeleteAt DATETIME NOT NULL,
    CreateAt DATETIME NOT NULL DEFAULT GETDATE()
)



CREATE TABLE Document (
    Id INT IDENTITY(1,1) PRIMARY KEY,
	[Url] VARCHAR(255) NOT NULL,
	[Name] VARCHAR(50) NOT NULL,
	ExperienceId INT NULL
	
)



CREATE TABLE ExperienceLineThematic (
    Id INT IDENTITY(1,1) PRIMARY KEY,
	DeleteAt DATETIME NOT NULL,
    CreateAt DATETIME NOT NULL DEFAULT GETDATE(),
    LineThematicId INT NOT NULL,
    ExperienceId INT NOT NULL
	
)



CREATE TABLE Objective (
    Id INT IDENTITY(1,1) PRIMARY KEY,
	ObjectiveDescription TEXT(500) NOT NULL,
	Innovation TEXT(500) NOT NULL,
	Results TEXT(255) NOT NULL,
	Sustainability  TEXT(255) NOT NULL,
	ExperienceId INT NULL,
	
)


CREATE TABLE Evaluation (
    Id INT  IDENTITY(1,1) PRIMARY KEY,
	TypeEvaluation VARCHAR(100) NOT NULL,
	Comments VARCHAR(255) NOT NULL,
	[DateTime] DATETIME NOT NULL
	UserId INT NULL,
	ExperienceId INT NULL
	
)

CREATE TABLE Institution (
    Id INT IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR(50) NOT NULL,
	[Address] VARCHAR(255) NOT NULL,
	Phone VARCHAR(15) NOT NULL,
	EmailInstitution VARCHAR(255) NOT NULL UNIQUE,
	Department VARCHAR(100) NOT NULL,
	Commune VARCHAR(100) NOT NULL
)

CREATE TABLE EvaluationCriteria (
    Id INT IDENTITY(1,1) PRIMARY KEY,
	Score nvarchar(max)  NOT NULL,
    EvaluationId INT NOT NULL

)

CREATE TABLE Criteria (
    Id INT  IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR(50) NOT NULL
)

CREATE TABLE ExperiencePopulationGroup (
    Id INT  IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR(50) NOT NULL,
	ExperienceId INT NOT NULL,
	PopulationGradeId INT NOT NULL
)

CREATE TABLE PopulationGarde (
    Id INT IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR(50) NOT NULL,
)

CREATE TABLE Verification (
    Id INT IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR(50) NOT NULL,
	[Description] TEXT(500) NOT NULL
)

CREATE TABLE LineThematic (
    Id INT IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR(50) NOT NULL,
	Code CHAR(10) NOT NULL UNIQUE,
	Active BIT DEFAULT 1,
	DeleteAt DATETIME NULL,
    CreateAt DATETIME NOT NULL DEFAULT GETDATE()

)

CREATE TABLE ExperienceGrade (
    Id INT PRIMARY KEY IDENTITY(1,1),

)

CREATE TABLE Grade (
    Id INT IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR(10) NOT NULL
)

CREATE TABLE Rol (
    Id INT IDENTITY(1,1)PRIMARY KEY,
	TypeRol VARCHAR(25) NOT NULL,
	Code CHAR(10) NOT NULL UNIQUE,
	[Name] VARCHAR(50) NOT NULL,
	Active BIT DEFAULT 1,
	DeleteAt DATETIME NOT NULL,
    CreateAt DATETIME NOT NULL DEFAULT GETDATE()

)

CREATE TABLE RolPermission (
    Id INT IDENTITY(1,1) PRIMARY KEY,
	RolId INT NOT NULL,
	PermissionId INT NOT NULL

)

CREATE TABLE Permission (
    Id INT  IDENTITY(1,1) PRIMARY KEY,
	PermissionType VARCHAR(25) NOT NULL,
	RolId INT NOT NULL
)



ALTER TABLE HistoryExperience 
ADD CONSTRAINT FK_HistoryExperience_User FOREIGN KEY (UserId) REFERENCES [User](Id);

ALTER TABLE HistoryExperience 
ADD CONSTRAINT FK_HistoryExperience_Experience FOREIGN KEY (ExperienceId) REFERENCES [Experience](Id);


ALTER TABLE Evaluation 
ADD CONSTRAINT FK_Evaluation_User FOREIGN KEY (UserId) REFERENCES [User](Id);

ALTER TABLE Evaluation 
ADD CONSTRAINT FK_Evaluation_Experience FOREIGN KEY (ExperienceId) REFERENCES [Experience](Id);



ALTER TABLE Experience 
ADD CONSTRAINT FK_Experience_User FOREIGN KEY (UserId) REFERENCES [User](Id);

ALTER TABLE Experience 
ADD CONSTRAINT FK_Experience_Institution FOREIGN KEY (InstitutionId) REFERENCES [Institution](Id);


ALTER TABLE UserRol 
ADD CONSTRAINT FK_UserRol_User FOREIGN KEY (UserId) REFERENCES [User](Id);

ALTER TABLE UserRol
ADD CONSTRAINT FK_UserRol_Rol FOREIGN KEY (RolId) REFERENCES [Rol](Id);


ALTER TABLE Document 
ADD CONSTRAINT FK_Document_Experience FOREIGN KEY (ExperienceId) REFERENCES [Experience](Id);


ALTER TABLE ExperienceLineThematic 
ADD CONSTRAINT FK_ExperienceLineThematic_LineThematic FOREIGN KEY (LineThematicId) REFERENCES [LineThematic](Id);
ALTER TABLE ExperienceLineThematic
ADD CONSTRAINT FK_ExperienceLineThematic_Experience FOREIGN KEY (ExperienceId) REFERENCES [Experience](Id);


ALTER TABLE Objective
ADD CONSTRAINT FK_Objective_Experience FOREIGN KEY (ExperienceId) REFERENCES [Experience](Id);



ALTER TABLE EvaluationCriteria
ADD CONSTRAINT FK_EvaluationCriteria_Evaluation FOREIGN KEY (EvaluationId) REFERENCES [Evaluation](Id);


ALTER TABLE ExperiencePopulationGroup
ADD CONSTRAINT FK_ExperiencePopulationGroup_Experience FOREIGN KEY (ExperienceId) REFERENCES [Experience](Id);
ALTER TABLE ExperiencePopulationGroup
ADD CONSTRAINT FK_ExperiencePopulationGroup_PopulationGrade FOREIGN KEY (PopulationGradeId) REFERENCES [PopulationGrade](Id);


ALTER TABLE RolPermission
ADD CONSTRAINT FK_RolPermission_Rol FOREIGN KEY (RolId) REFERENCES [Rol](Id);
ALTER TABLE RolPermission
ADD CONSTRAINT FK_RolPermission_Permission FOREIGN KEY (PermissionId) REFERENCES [Permission](Id);


ALTER TABLE Permission
ADD CONSTRAINT FK_Permission_Rol FOREIGN KEY (RolId) REFERENCES [Rol](Id);