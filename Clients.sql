-- SQL script to insert 100 fake client records into the Clients table.
BEGIN TRANSACTION;
BEGIN TRY

    INSERT INTO Clients (Name, Email, Phone, RegistrationDate)
    VALUES ('João Silva', 'joao.silva@email.com', '11987654321', GETDATE());

    INSERT INTO Clients (Name, Email, Phone, RegistrationDate)
    VALUES ('Maria Souza', 'maria.souza@email.com', '11987654322', GETDATE());

    INSERT INTO Clients (Name, Email, Phone, RegistrationDate)
    VALUES ('Pedro Santos', 'pedro.santos@email.com', '11987654323', GETDATE());

    INSERT INTO Clients (Name, Email, Phone, RegistrationDate)
    VALUES ('Ana Oliveira', 'ana.oliveira@email.com', '11987654324', GETDATE());

    INSERT INTO Clients (Name, Email, Phone, RegistrationDate)
    VALUES ('Lucas Lima', 'lucas.lima@email.com', '11987654325', GETDATE());
    
    INSERT INTO Clients (Name, Email, Phone, RegistrationDate)
    VALUES ('Beatriz Costa', 'beatriz.costa@email.com', '11987654326', GETDATE());

    INSERT INTO Clients (Name, Email, Phone, RegistrationDate)
    VALUES ('Carlos Ferreira', 'carlos.ferreira@email.com', '11987654327', GETDATE());

    INSERT INTO Clients (Name, Email, Phone, RegistrationDate)
    VALUES ('Juliana Alves', 'juliana.alves@email.com', '11987654328', GETDATE());

    INSERT INTO Clients (Name, Email, Phone, RegistrationDate)
    VALUES ('Rafael Pereira', 'rafael.pereira@email.com', '11987654329', GETDATE());

    INSERT INTO Clients (Name, Email, Phone, RegistrationDate)
    VALUES ('Fernanda Rocha', 'fernanda.rocha@email.com', '11987654330', GETDATE());

    DECLARE @counter INT = 11;
    WHILE @counter <= 100
    BEGIN
        DECLARE @name VARCHAR(100) = 'Client ' + CAST(@counter AS VARCHAR(3));
        DECLARE @email VARCHAR(100) = 'client' + CAST(@counter AS VARCHAR(3)) + '@email.com';
        DECLARE @phone VARCHAR(20) = '1190000' + RIGHT('000' + CAST(@counter AS VARCHAR(3)), 4);
        INSERT INTO Clients (Name, Email, Phone, RegistrationDate)
        VALUES (@name, @email, @phone, GETDATE());
        SET @counter = @counter + 1;
    END

    -- Commit the transaction if everything was successful
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    -- Rollback the transaction in case of an error
    ROLLBACK TRANSACTION;
    THROW;
END CATCH
