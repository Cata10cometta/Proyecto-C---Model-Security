use modelsecurity
	CREATE TABLE Experience (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    NameExperiences VARCHAR(255) NOT NULL,
    Summary TEXT,
    Methodologies TEXT,
    Transfer TEXT,
    DateRegistration DATETIME NOT NULL,
    Code VARCHAR(100),
    DeleteAt DATETIME,
    CreateAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UserId INT NOT NULL,
    InstitutionId INT NOT NULL,

    
    FOREIGN KEY (UserId) REFERENCES User(Id),
    FOREIGN KEY (InstitutionId) REFERENCES Institution(Id)
);

CREATE TABLE UserRol (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    DeleteAt DATETIME,
    CreateAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UserId INT NOT NULL,
    RolId INT NOT NULL,

  
    FOREIGN KEY (UserId) REFERENCES User(Id),
    FOREIGN KEY (RolId) REFERENCES Rol(Id)
);

CREATE TABLE HistoryExperience (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Action VARCHAR(100) NOT NULL,
    DateTime DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    TableName VARCHAR(100),
    UserId INT NOT NULL,
    ExperienceId INT NOT NULL,
    
    
    FOREIGN KEY (UserId) REFERENCES User(Id),
    FOREIGN KEY (ExperienceId) REFERENCES Experience(Id)
);

CREATE TABLE `User` (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    DeleteAt DATETIME,
    CreateAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE State (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL
);

CREATE TABLE Person (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Phone VARCHAR(20),
    DeleteAt DATETIME,
    CreateAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Document (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Url VARCHAR(500) NOT NULL,
    Name VARCHAR(255),
    ExperienceId INT NOT NULL,

    FOREIGN KEY (ExperienceId) REFERENCES Experience(Id)
);

CREATE TABLE ExperienceLineThematic (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    DeleteAt DATETIME,
    CreateAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    LineThematicId INT NOT NULL,
    ExperienceId INT NOT NULL,

    FOREIGN KEY (LineThematicId) REFERENCES LineThematic(Id),
    FOREIGN KEY (ExperienceId) REFERENCES Experience(Id)
);

CREATE TABLE Objective (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ObjectiveDescription TEXT NOT NULL,
    Innovation TEXT,
    Results TEXT,
    Sustainability TEXT,
    ExperienceId INT NOT NULL,

    FOREIGN KEY (ExperienceId) REFERENCES Experience(Id)
);

CREATE TABLE Evaluation (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    TypeEvaluation VARCHAR(100),
    Comments TEXT,
    DateTime DATETIME DEFAULT CURRENT_TIMESTAMP,
    UserId INT NOT NULL,
    ExperienceId INT NOT NULL,
    

    FOREIGN KEY (UserId) REFERENCES User(Id),
    FOREIGN KEY (ExperienceId) REFERENCES Experience(Id)
    );

CREATE TABLE Institution (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    Address VARCHAR(255) NOT NULL,
    Phone VARCHAR(20) NOT NULL,
    EmailInstitution VARCHAR(255) NOT NULL UNIQUE,
    Department VARCHAR(100) NOT NULL,
    Commune VARCHAR(100) NOT NULL
);

CREATE TABLE EvaluationCriteria (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Score varchar(max) NOT NULL,
    EvaluationId INT NOT NULL,
    FOREIGN KEY (EvaluationId) REFERENCES Evaluation(Id)

);

CREATE TABLE Criteria (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL
);

CREATE TABLE ExperiencePopulationGroup (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    ExperienceId INT NOT NULL,
    PopulationGradeId INT NOT NULL,

    FOREIGN KEY (ExperienceId) REFERENCES Experience(Id),
    FOREIGN KEY (PopulationGradeId) REFERENCES PopulationGrade(Id)
);

CREATE TABLE PopulationGarde (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL
);

CREATE TABLE Verification (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    Description VARCHAR(255) NOT NULL
)

CREATE TABLE LineThematic (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    Code VARCHAR(20) NOT NULL UNIQUE,
    Active TINYINT(1) DEFAULT 1
    DeleteAt DATETIME,
    CreateAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE ExperienceGrade (
    Id INT AUTO_INCREMENT PRIMARY KEY

);

CREATE TABLE Grade (
     Id INT AUTO_INCREMENT PRIMARY KEY,
     Name VARCHAR(20) NOT NULL
);

CREATE TABLE Rol (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    TypeRol VARCHAR(100) NOT NULL,
    Code VARCHAR(20) NOT NULL UNIQUE,
    Name VARCHAR(100) NOT NULL,
    Active TINYINT(1) DEFAULT 1,
    DeleteAt DATETIME,
    CreateAt DATETIME DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE RolPermission (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    RolId INT NOT NULL,
    PermissionId INT NOT NULL,
    
    FOREIGN KEY (RolId) REFERENCES Rol(Id),
    FOREIGN KEY (PermissionId) REFERENCES Permission(Id)
);

CREATE TABLE Permission (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    PermissionType VARCHAR(100) NOT NULL,
    RolId INT NOT NULL,
    
    FOREIGN KEY (RolId) REFERENCES Rol(Id)
);