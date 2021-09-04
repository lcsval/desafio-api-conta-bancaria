CREATE TABLE accounts (
    id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
	name varchar(100) NOT NULL,
    balance decimal NOT NULL,
);

CREATE TABLE accountrecords (
	id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
	accountid UNIQUEIDENTIFIER NOT NULL,
	date DATETIME DEFAULT GETDATE(),
    type varchar(7) NOT NULL,
	value decimal(9,2) NOT NULL,
    tax decimal(9,2) NOT NULL,
    totalvalue decimal(9,2) NOT NULL,
	FOREIGN KEY (accountid) REFERENCES accounts(id)
);


INSERT INTO accounts (name, balance) VALUES ('Conta1', 100);
INSERT INTO accountsrecords (




INSERT INTO accounts (name, balance) VALUES ('Conta2', 100);
INSERT INTO accounts (name, balance) VALUES ('Conta3', 100);
INSERT INTO accounts (name, balance) VALUES ('Conta4', 100);
	
	
	
SELECT * FROM accounts;




