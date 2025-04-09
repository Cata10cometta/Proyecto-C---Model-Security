CREATE TABLE "Experience" (
    "Id" SERIAL PRIMARY KEY,
    "NameExperiences" VARCHAR(255),
    "Summary" TEXT,
    "Methodologies" TEXT,
    "Transfer" TEXT,
    "DateRegistration" DATE,
    "Code" VARCHAR(100),
    "DeleteAt" TIMESTAMP,
    "CreateAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "UserId" INTEGER,
    "InstitutionId" INTEGER  
);

CREATE TABLE "UserRol" (
    "Id" SERIAL PRIMARY KEY,
    "DeleteAt" TIMESTAMP,
    "CreateAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "UserId" INTEGER NOT NULL,
    "RolId" INTEGER NOT NULL
);

CREATE TABLE "HistoryExperience" (
    "Id" SERIAL PRIMARY KEY,
    "Action" VARCHAR(100),
    "DateTime" TIMESTAMP,
    "TableName" VARCHAR(100),
    "UserId" INTEGER NOT NULL,
    "ExperienceId" INTEGER NOT NULL
);

CREATE TABLE "User" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(100),
    "Email" VARCHAR(150) UNIQUE NOT NULL,
    "Password" VARCHAR(255) NOT NULL,
    "DeleteAt" TIMESTAMP,
    "CreateAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE "State" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(100) NOT NULL
);

CREATE TABLE "Person" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(100) NOT NULL,
    "Email" VARCHAR(150) UNIQUE NOT NULL,
    "Phone" VARCHAR(20),
    "DeleteAt" TIMESTAMP,
    "CreateAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE "Document" (
    "Id" SERIAL PRIMARY KEY,
    "Url" TEXT NOT NULL,
    "Name" VARCHAR(150),
    "ExperienceId" INTEGER NOT NULL,
    
);

CREATE TABLE "ExperienceLineThematic" (
    "Id" SERIAL PRIMARY KEY,
    "DeleteAt" TIMESTAMP,
    "CreateAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    "LineThematicId" INTEGER NOT NULL,
    "ExperienceId" INTEGER NOT NULL,
    
);

CREATE TABLE "Objective" (
    "Id" SERIAL PRIMARY KEY,
    "ObjectiveDescription" TEXT,
    "Innovation" TEXT,
    "Results" TEXT,
    "Sustainability" TEXT,
    "ExperienceId" INTEGER NOT NULL,
   
);

CREATE TABLE "Evaluation" (
    "Id" SERIAL PRIMARY KEY,
    "TypeEvaluation" TEXT,
    "Comments" TEXT,
    "DateTime" TIMESTAMP,
    "UserId" INTEGER NOT NULL,
    "ExperienceId" INTEGER NOT NULL 
);


CREATE TABLE "Institution" (
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(255) NOT NULL,
    "Address" TEXT,
    "Phone" TEXT,
    "EmailInstitution" TEXT,
    "Department" TEXT,
    "Commune" TEXT
);

CREATE TABLE "EvaluationCriteria" (
    "Id" SERIAL PRIMARY KEY,
    "Score" nvarchar(max)  NOT NULL,
    "EvaluationId" INTEGER

);

CREATE TABLE "Criteria"(
    "Id" SERIAL PRIMARY KEY,
    "Name" TEXT NOT NULL,
);

CREATE TABLE "ExperiencePopulationGroup"(
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(50) NOT NULL,
    "ExperienceId" INTEGER,
    "PopulationGradeId" INTEGER,
    
);

CREATE TABLE "PopulationGrade"(
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(50) NOT NULL,

);

CREATE TABLE "Verification"(
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(50) NOT NULL,
    "Description" TEXT NOT NULL

);

CREATE TABLE "LineThematic"(
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(50) NOT NULL,
    "Code" VARCHAR(20) NOT NULL UNIQUE,
    "Active" BOOLEAN DEFAULT TRUE,
    "DeleteAt" TIMESTAMP,
    "CreateAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE "ExperienceGrade"(
    "Id" SERIAL PRIMARY KEY
);

CREATE TABLE "Grade"(
    "Id" SERIAL PRIMARY KEY,
    "Name" VARCHAR(20) NOT NULL
);

CREATE TABLE "ROL"(
    "Id" SERIAL PRIMARY KEY,
    "TypeRol" VARCHAR(100) NOT NULL,
    "Code" VARCHAR(20) NOT NULL UNIQUE,
    "Name" VARCHAR(20) NOT NULL,
    "Active" BOOLEAN DEFAULT TRUE,
    "DeleteAt" TIMESTAMP,
    "CreateAt" TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE "RolPermission"(
    "Id" SERIAL PRIMARY KEY,
    "RolId" INTEGER,
    "PermissionId" INTEGER,
    
);

CREATE TABLE "Permission"(
    "Id" SERIAL PRIMARY KEY,
    "PermissionType" VARCHAR(100) NOT NULL,
    "RolId" INTEGER,
    
);


CONSTRAINT fk_experience_user FOREIGN KEY ("UserId") REFERENCES "User"("Id"),
CONSTRAINT fk_experience_institution FOREIGN KEY ("InstitutionId") REFERENCES "Institution"("Id")


CONSTRAINT fk_userRol_user FOREIGN KEY ("UserId") REFERENCES "User"("Id"),
CONSTRAINT fk_userRol_rol FOREIGN KEY ("RolId") REFERENCES "Rol"("Id")


CONSTRAINT fk_historyExperience_user FOREIGN KEY ("UserId") REFERENCES "User"("Id"),
CONSTRAINT fk_historyExperience_experience FOREIGN KEY ("ExperienceId") REFERENCES "Experience"("Id")


CONSTRAINT fk_experienceLineThematic_lineThematic FOREIGN KEY ("LineThematicId") REFERENCES "LineThematic"("Id"),
CONSTRAINT fk_experienceLineThematic_experience FOREIGN KEY ("ExperienceId") REFERENCES "Experience"("Id")


CONSTRAINT fk_document_experience FOREIGN KEY ("ExperienceId") REFERENCES "Experience"("Id")


CONSTRAINT fk_objective_experience FOREIGN KEY ("ExperienceId") REFERENCES "Experience"("Id")


CONSTRAINT fk_evaluation_experience FOREIGN KEY ("ExperienceId") REFERENCES "Experience"("Id"),
CONSTRAINT fk_exvaluation_user FOREIGN KEY ("UserId") REFERENCES "User"("Id")


CONSTRAINT fk_evaluationCriteria_evaluation FOREIGN KEY ("EvaluationId") REFERENCES "Evaluation"("Id"),


CONSTRAINT fk_experiencePopulationGroup_experience FOREIGN KEY ("ExperienceId") REFERENCES "Experience"("Id"),
CONSTRAINT fk_experiencePopulationGroup_populationGrade FOREIGN KEY ("PopulationGradeId") REFERENCES "PopulationGrade"("Id")


CONSTRAINT fk_rolPermission_rol FOREIGN KEY ("RolId") REFERENCES "Rol"("Id"),
CONSTRAINT fk_rolPermission_permission FOREIGN KEY ("PermissionId") REFERENCES "Permission"("Id")


CONSTRAINT fk_permission_rol FOREIGN KEY ("RolId") REFERENCES "Rol"("Id")