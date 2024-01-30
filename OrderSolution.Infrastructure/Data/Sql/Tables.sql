-- dotnet ef dbcontext scaffold "Name=postgres:dfConnectionString" Npgsql.EntityFrameworkCore.PostgreSQL --context-dir ../OrderSolution.Infrastructure/Data --output-dir ../OrderSolution.Infrastructure/Entities/Dbf

DROP TABLE IF EXISTS ProductDetails;
DROP TABLE IF EXISTS ProductImages;
DROP TABLE IF EXISTS Images;
DROP TABLE IF EXISTS Products;
DROP TABLE IF EXISTS Categories;

CREATE TABLE IF NOT EXISTS Categories (
    Id text PRIMARY KEY NOT NULL,
    Name text NOT NULL
);

CREATE TABLE IF NOT EXISTS Products (
    Id text PRIMARY KEY NOT NULL,
    Name varchar(100) NOT NULL,
    Description text NOT NULL,
    Price numeric(7,2) NOT NULL,
    Quantity integer NOT NULL,
    CategoryId text NOT NULL,
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);

CREATE TABLE IF NOT EXISTS Images(
    Id text PRIMARY KEY NOT NULL,
    Url text NOT NULL
);

CREATE TABLE IF NOT EXISTS ProductImages (
    ProductId text NOT NULL,
    ImageId text NOT NULL,
    PRIMARY KEY (ProductId, ImageId),
    FOREIGN KEY (ProductId) REFERENCES Products(Id),
    FOREIGN KEY (ImageId) REFERENCES Images(Id)
);

CREATE TABLE IF NOT EXISTS ProductDetails(
    ProductId text PRIMARY KEY NOT NULL,
    Color varchar(30) NOT NULL,
    Size varchar(30) NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);