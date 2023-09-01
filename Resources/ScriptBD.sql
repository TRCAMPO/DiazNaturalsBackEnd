CREATE TABLE ADMINISTRATOR
(
    id_administrator    INT NOT NULL,
    name_administrator  VARCHAR(50) NOT NULL,
    email_administrator VARCHAR(100) NOT NULL,

    CONSTRAINT PK_Administrator PRIMARY KEY (id_administrator)
);

CREATE TABLE CREDENTIALS
(
    password         VARCHAR(50) NOT NULL,
    salt_credential  VARCHAR(250) NOT NULL,
    id_administrator INT NOT NULL,

    CONSTRAINT PK_Credential PRIMARY KEY (password), 

    CONSTRAINT FK_Administrator FOREIGN KEY (id_administrator) REFERENCES Administrator (id_administrator)
);

ALTER TABLE Administrator
ADD CONSTRAINT UQ_Administrator_ID UNIQUE (id_administrator);

ALTER TABLE Administrator
ADD CONSTRAINT UQ_Administrator_name UNIQUE (name_administrator);

ALTER TABLE Administrator
ADD CONSTRAINT UQ_Administrator_Email UNIQUE (email_administrator);