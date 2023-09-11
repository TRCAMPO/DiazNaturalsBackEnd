-- SCRIPT DE LA BASE DE DATOS DE LA APLICACIÓN WEB DE DIAZ NATURALS

-- Definición de la tabla "CREDENTIALS" para almacenar credenciales de usuarios
CREATE TABLE CREDENTIALS (
    id_credential INT IDENTITY(1,1), -- ID de la credencial (clave primaria)
    password_credential VARCHAR(250) NOT NULL, -- Contraseña de la credencial
    salt_credential VARCHAR(250) NOT NULL -- Valor de sal para la contraseña
);

-- Definición de la tabla "PRESENTATIONS" para almacenar presentaciones de productos
CREATE TABLE PRESENTATIONS (
    id_presentation INT IDENTITY(1,1), -- ID de la presentación (clave primaria)
    name_presentation VARCHAR(50) NOT NULL -- Nombre de la presentación
);

-- Definición de la tabla "CATEGORIES" para almacenar categorías de productos
CREATE TABLE CATEGORIES (
    id_category INT IDENTITY(1,1), -- ID de la categoría (clave primaria)
    name_category VARCHAR(100) NOT NULL -- Nombre de la categoría
);

-- Definición de la tabla "STATUSES" para almacenar estados
CREATE TABLE STATUSES (
    id_status INT IDENTITY(1,1), -- ID del estado (clave primaria)
    name_status VARCHAR(100) NOT NULL -- Nombre del estado
);

-- Definición de la tabla "SUPPLIERS" para almacenar proveedores
CREATE TABLE SUPPLIERS (
    id_supplier INT IDENTITY(1,1), -- ID del proveedor (clave primaria)
    nit_supplier VARCHAR(100) NOT NULL, -- NIT (Número de Identificación Tributaria) del proveedor
    name_supplier VARCHAR(250) NOT NULL, -- Nombre del proveedor
    address_supplier VARCHAR(100) NOT NULL, -- Dirección del proveedor
    phone_supplier VARCHAR(50) NOT NULL, -- Número de teléfono del proveedor
    email_supplier VARCHAR(100) NOT NULL, -- Correo electrónico del proveedor
    is_active_supplier BIT NOT NULL -- Indicador de proveedor activo (1 o 0)
);

-- Definición de la tabla "ADMINISTRATORS" para almacenar administradores
CREATE TABLE ADMINISTRATORS (
    id_administrator INT IDENTITY(1,1), -- ID del administrador (clave primaria)
    id_credential INT NOT NULL, -- ID de la credencial asociada (clave foránea)
    name_administrator VARCHAR(100) NOT NULL, -- Nombre del administrador
    email_administrator VARCHAR(100) NOT NULL -- Correo electrónico del administrador
);

-- Definición de la tabla "CLIENTS" para almacenar clientes
CREATE TABLE CLIENTS (
    id_client INT IDENTITY(1,1), -- ID del cliente (clave primaria)
    id_credential INT NOT NULL, -- ID de la credencial asociada (clave foránea)
    nit_client VARCHAR(50) NOT NULL, -- NIT (Número de Identificación Tributaria) del cliente
    name_client VARCHAR(250) NOT NULL, -- Nombre del cliente
    email_client VARCHAR(100) NOT NULL, -- Correo electrónico del cliente
    is_active_client BIT NOT NULL, -- Indicador de cliente activo (1 o 0)
    address_client VARCHAR(100) NOT NULL, -- Dirección del cliente
    phone_client VARCHAR(50) NOT NULL, -- Número de teléfono del cliente
    city_client VARCHAR(50) NOT NULL, -- Ciudad del cliente
    state_client VARCHAR(50) NOT NULL, -- Estado/provincia del cliente
    name_contact_client VARCHAR(100) NOT NULL -- Nombre del contacto del cliente
);

-- Definición de la tabla "ORDERS" para almacenar órdenes
CREATE TABLE ORDERS (
    id_order INT IDENTITY(1,1), -- ID de la orden (clave primaria)
    id_client INT NOT NULL, -- ID del cliente asociado (clave foránea)
    start_date_order DATETIME NOT NULL -- Fecha de inicio de la orden
);

-- Definición de la tabla "ORDER_HISTORY" para almacenar historial de órdenes
CREATE TABLE ORDER_HISTORY (
    id_order INT NOT NULL, -- ID de la orden asociada (clave foránea)
    id_status INT NOT NULL, -- ID del estado de la orden (clave foránea)
    date_order_history DATETIME NOT NULL -- Fecha y hora del historial de la orden
);

-- Definición de la tabla "PRODUCTS" para almacenar productos
CREATE TABLE PRODUCTS (
    id_product INT IDENTITY(1,1), -- ID del producto (clave primaria)
    id_supplier INT NOT NULL, -- ID del proveedor asociado (clave foránea)
    id_presentation INT NOT NULL, -- ID de la presentación asociada (clave foránea)
    id_category INT NOT NULL, -- ID de la categoría asociada (clave foránea)
    name_product VARCHAR(250) NOT NULL, -- Nombre del producto
    price_product INT NOT NULL, -- Precio del producto
    quantity_product INT NOT NULL, -- Cantidad disponible del producto
    description_product VARCHAR(250) NOT NULL, -- Descripción del producto
    image_product VARCHAR(250) NOT NULL, -- Ruta de la imagen del producto
    is_active_product BIT NOT NULL -- Indicador de producto activo (1 o 0)
);

-- Definición de la tabla "ENTRADAS" para almacenar entradas de productos
CREATE TABLE ENTRIES (
    id_entry INT IDENTITY(1,1), -- ID de la entrada (clave primaria)
    id_product INT NOT NULL, -- ID del producto asociado (clave foránea)
    date_entry DATETIME NOT NULL, -- Fecha y hora del historial de la entrada
    quantity_product_entry INT NOT NULL -- Cantidad entrada del producto
);

-- Definición de la tabla "CARTS" para almacenar carritos de compra
CREATE TABLE CARTS (
    id_order INT NOT NULL, -- ID de la orden asociada (clave foránea)
    id_product INT NOT NULL, -- ID del producto asociado (clave foránea)
    quantity_product_cart INT NOT NULL -- Cantidad de productos en el carrito
);

-- Se agregan restricciones a las tablas existentes para garantizar integridad y unicidad de datos
-- (Claves primarias, claves foráneas y restricciones únicas)

-- Agrega una restricción de clave primaria (PK) a la tabla "CREDENTIALS" en la columna "id_credential"
ALTER TABLE CREDENTIALS
    ADD CONSTRAINT PK_Credential PRIMARY KEY (id_credential);

-- Agrega una restricción de clave primaria (PK) a la tabla "PRESENTATIONS" en la columna "id_presentation"
ALTER TABLE PRESENTATIONS
    ADD CONSTRAINT PK_Id_Presentation PRIMARY KEY (id_presentation);

-- Agrega una restricción única (UQ) a la tabla "PRESENTATIONS" en la columna "name_presentation"
ALTER TABLE PRESENTATIONS
    ADD CONSTRAINT UQ_Name_Presentation UNIQUE (name_presentation);

-- Agrega una restricción de clave primaria (PK) a la tabla "CATEGORIES" en la columna "id_category"
ALTER TABLE CATEGORIES
    ADD CONSTRAINT PK_Id_Category PRIMARY KEY (id_category);

-- Agrega una restricción única (UQ) a la tabla "CATEGORIES" en la columna "name_category"
ALTER TABLE CATEGORIES
    ADD CONSTRAINT UQ_Name_Category UNIQUE (name_category);

-- Agrega una restricción de clave primaria (PK) a la tabla "STATUSES" en la columna "id_status"
ALTER TABLE STATUSES
    ADD CONSTRAINT PK_Id_Status PRIMARY KEY (id_status);

-- Agrega una restricción única (UQ) a la tabla "STATUSES" en la columna "name_status"
ALTER TABLE STATUSES
    ADD CONSTRAINT UQ_Name_Status UNIQUE (name_status);

-- Agrega una restricción de clave primaria (PK) a la tabla "SUPPLIERS" en la columna "id_supplier"
ALTER TABLE SUPPLIERS
    ADD CONSTRAINT PK_Id_Supplier PRIMARY KEY (id_supplier);

-- Agrega una restricción única (UQ) a la tabla "SUPPLIERS" en la columna "nit_supplier"
ALTER TABLE SUPPLIERS
    ADD CONSTRAINT UQ_Nit_Supplier UNIQUE (nit_supplier);

-- Agrega una restricción única (UQ) a la tabla "SUPPLIERS" en la columna "name_supplier"
ALTER TABLE SUPPLIERS
    ADD CONSTRAINT UQ_Name_Supplier UNIQUE (name_supplier);

-- Agrega una restricción de clave primaria (PK) a la tabla "ADMINISTRATORS" en la columna "id_administrator"
ALTER TABLE ADMINISTRATORS
    ADD CONSTRAINT PK_Id_Administrator PRIMARY KEY (id_administrator);

-- Agrega una restricción de clave foránea (FK) a la tabla "ADMINISTRATORS" en la columna "id_credential"
-- que hace referencia a la tabla "CREDENTIALS" en la columna "id_credential"
ALTER TABLE ADMINISTRATORS
    ADD CONSTRAINT FK_IdCredential_Administrator FOREIGN KEY (id_credential) REFERENCES CREDENTIALS (id_credential);

-- Agrega una restricción única (UQ) a la tabla "ADMINISTRATORS" en la columna "name_administrator"
ALTER TABLE ADMINISTRATORS
    ADD CONSTRAINT UQ_Name_Administrator UNIQUE (name_administrator);

-- Agrega una restricción única (UQ) a la tabla "ADMINISTRATORS" en la columna "email_administrator"
ALTER TABLE ADMINISTRATORS
    ADD CONSTRAINT UQ_Email_Administrator UNIQUE (email_administrator);

-- Agrega una restricción de clave primaria (PK) a la tabla "CLIENTS" en la columna "id_client"
ALTER TABLE CLIENTS
    ADD CONSTRAINT PK_Id_Client PRIMARY KEY (id_client);

-- Agrega una restricción de clave foránea (FK) a la tabla "CLIENTS" en la columna "id_credential"
-- que hace referencia a la tabla "CREDENTIALS" en la columna "id_credential"
ALTER TABLE CLIENTS
    ADD CONSTRAINT FK_IdCredential_Client FOREIGN KEY (id_credential) REFERENCES CREDENTIALS (id_credential);

-- Agrega una restricción única (UQ) a la tabla "CLIENTS" en la columna "nit_client"
ALTER TABLE CLIENTS
    ADD CONSTRAINT UQ_Nit_Client UNIQUE (nit_client);

-- Agrega una restricción única (UQ) a la tabla "CLIENTS" en la columna "name_client"
ALTER TABLE CLIENTS
    ADD CONSTRAINT UQ_Name_Client UNIQUE (name_client);

-- Agrega una restricción única (UQ) a la tabla "CLIENTS" en la columna "email_client"
ALTER TABLE CLIENTS
    ADD CONSTRAINT UQ_Email_Client UNIQUE (email_client);

-- Agrega una restricción de clave primaria (PK) a la tabla "ORDERS" en la columna "id_order"
ALTER TABLE ORDERS
    ADD CONSTRAINT PK_Id_Order PRIMARY KEY (id_order);

-- Agrega una restricción de clave foránea (FK) a la tabla "ORDERS" en la columna "id_client"
-- que hace referencia a la tabla "CLIENTS" en la columna "id_client"
ALTER TABLE ORDERS
    ADD CONSTRAINT FK_IdClient_Order FOREIGN KEY (id_client) REFERENCES CLIENTS (id_client);

-- Agrega una restricción de clave primaria compuesta (PK) a la tabla "ORDER_HISTORY" en las columnas "id_order" e "id_status"
ALTER TABLE ORDER_HISTORY
    ADD CONSTRAINT PK_Id_Order_History PRIMARY KEY (id_order, id_status);

-- Agrega una restricción de clave foránea (FK) a la tabla "ORDER_HISTORY" en la columna "id_order"
-- que hace referencia a la tabla "ORDERS" en la columna "id_order"
ALTER TABLE ORDER_HISTORY
    ADD CONSTRAINT FK_IdOrder_OrdHis FOREIGN KEY (id_order) REFERENCES ORDERS (id_order);

-- Agrega una restricción de clave foránea (FK) a la tabla "ORDER_HISTORY" en la columna "id_status"
-- que hace referencia a la tabla "STATUSES" en la columna "id_status"
ALTER TABLE ORDER_HISTORY
    ADD CONSTRAINT FK_IdStatus_OrdHis FOREIGN KEY (id_status) REFERENCES STATUSES (id_status);

-- Agrega una restricción de clave primaria (PK) a la tabla "PRODUCTS" en la columna "id_product"
ALTER TABLE PRODUCTS
    ADD CONSTRAINT PK_Id_Product PRIMARY KEY (id_product);

-- Agrega una restricción de clave foránea (FK) a la tabla "PRODUCTS" en la columna "id_supplier"
-- que hace referencia a la tabla "SUPPLIERS" en la columna "id_supplier"
ALTER TABLE PRODUCTS
    ADD CONSTRAINT FK_IdSupplier_Product FOREIGN KEY (id_supplier) REFERENCES SUPPLIERS (id_supplier);

-- Agrega una restricción de clave foránea (FK) a la tabla "PRODUCTS" en la columna "id_presentation"
-- que hace referencia a la tabla "PRESENTATIONS" en la columna "id_presentation"
ALTER TABLE PRODUCTS
    ADD CONSTRAINT FK_IdPresentation_Product FOREIGN KEY (id_presentation) REFERENCES PRESENTATIONS (id_presentation);

-- Agrega una restricción de clave foránea (FK) a la tabla "PRODUCTS" en la columna "id_category"
-- que hace referencia a la tabla "CATEGORIES" en la columna "id_category"
ALTER TABLE PRODUCTS
    ADD CONSTRAINT FK_IdCategory_Product FOREIGN KEY (id_category) REFERENCES CATEGORIES (id_category);

-- Agrega una restricción única (UQ) a la tabla "PRODUCTS" en la columna "image_product"
ALTER TABLE PRODUCTS
    ADD CONSTRAINT UQ_Image_Product UNIQUE (image_product);

-- Agrega una restricción de clave primaria (PK) a la tabla "ENTRIES" en la columna "id_entry"
ALTER TABLE ENTRIES
    ADD CONSTRAINT PK_Id_Entry PRIMARY KEY (id_entry);

-- Agrega una restricción de clave foránea (FK) a la tabla "ENTRIES" en la columna "id_product"
-- que hace referencia a la tabla "PRODUCTS" en la columna "id_product"
ALTER TABLE ENTRIES
    ADD CONSTRAINT FK_IdProduct_Entry FOREIGN KEY (id_product) REFERENCES PRODUCTS (id_product);

-- Agrega una restricción de clave primaria compuesta (PK) a la tabla "CARTS" en las columnas "id_order" e "id_product"
ALTER TABLE CARTS
    ADD CONSTRAINT PK_Id_Carts PRIMARY KEY (id_order, id_product);

-- Agrega una restricción de clave foránea (FK) a la tabla "CARTS" en la columna "id_order"
-- que hace referencia a la tabla "ORDERS" en la columna "id_order"
ALTER TABLE CARTS
    ADD CONSTRAINT FK_IdOrder_Carts FOREIGN KEY (id_order) REFERENCES ORDERS (id_order);

-- Agrega una restricción de clave foránea (FK) a la tabla "CARTS" en la columna "id_product"
-- que hace referencia a la tabla "PRODUCTS" en la columna "id_product"
ALTER TABLE CARTS
    ADD CONSTRAINT FK_IdProduct_Carts FOREIGN KEY (id_product) REFERENCES PRODUCTS (id_product);
