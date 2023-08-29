CREATE TABLE Department (
    Id SERIAL PRIMARY KEY,
    ParentId INTEGER,
    ManagerId INTEGER,
    Name VARCHAR(255) NOT NULL,
    Phone VARCHAR(20),
    FOREIGN KEY (ParentId) REFERENCES Department(Id),
    FOREIGN KEY (ManagerId) REFERENCES Employee(Id)
);

CREATE TABLE Employee (
    Id SERIAL PRIMARY KEY,
    DepartmentId INTEGER NOT NULL,
    FullName VARCHAR(255) NOT NULL,
    Login VARCHAR(50) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    FOREIGN KEY (DepartmentId) REFERENCES Department(Id)
);

CREATE TABLE JobTitle (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(255) NOT NULL
);