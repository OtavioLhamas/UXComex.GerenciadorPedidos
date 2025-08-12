-- SQL script to insert 50 fake product records into the Products table.

-- Start a transaction to ensure all or none of the inserts are committed.
BEGIN TRANSACTION;
BEGIN TRY

    INSERT INTO Products (Name, Description, Price, StockQuantity)
    VALUES 
    ('Laptop Ultrabook', 'A powerful and lightweight laptop with a 13-inch display.', 1500.50, 25),
    ('Wireless Mouse', 'Ergonomic wireless mouse with customizable buttons.', 35.99, 150),
    ('Mechanical Keyboard', 'RGB mechanical keyboard with quiet switches.', 120.00, 75),
    ('4K Monitor', '27-inch 4K UHD monitor with a high refresh rate.', 450.75, 40),
    ('USB-C Hub', 'Multi-port hub for modern laptops.', 49.99, 200);

    DECLARE @counter INT = 6;
    DECLARE @name VARCHAR(255);
    DECLARE @description VARCHAR(1000);
    DECLARE @price DECIMAL(18, 2);
    DECLARE @stock INT;

    WHILE @counter <= 50
    BEGIN
        SET @name = 'Product ' + CAST(@counter AS VARCHAR(3));
        SET @description = 'This is a description for ' + @name + '. It is a generic test product.';
        SET @price = RAND() * 1000 + 10; -- Generates a random price between 10.00 and 1010.00
        SET @stock = CAST(RAND() * 200 AS INT) + 1; -- Generates a random stock quantity between 1 and 200

        INSERT INTO Products (Name, Description, Price, StockQuantity)
        VALUES (@name, @description, @price, @stock);

        SET @counter = @counter + 1;
    END

    -- Commit the transaction if all inserts were successful.
    COMMIT TRANSACTION;
    PRINT '50 products have been successfully inserted.';

END TRY
BEGIN CATCH
    -- If any error occurs, roll back the transaction and throw the error.
    ROLLBACK TRANSACTION;
    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
    DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
    DECLARE @ErrorState INT = ERROR_STATE();
    RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH
