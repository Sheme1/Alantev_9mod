use YchtStud

create table stud (
	StudID INT PRIMARY KEY IDENTITY,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    Speciality NVARCHAR(50),
    DateOfHire DATE
)

insert into stud(FirstName, LastName, Speciality, DateOfHire) values ('Иван','Иванов','3ПОИТ21','12/09/2001')

truncate table stud

select * from stud