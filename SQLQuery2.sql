Create database dbemarketing

use dbemarketing


-- Create table
CREATE TABLE tbl_admin (
    ad_id INT IDENTITY PRIMARY KEY,
    ad_username NVARCHAR(50) NOT NULL UNIQUE,
    ad_password NVARCHAR(50) NOT NULL
);

-- Insert data
INSERT INTO tbl_admin VALUES ('root', 'admin123');
INSERT INTO tbl_admin VALUES ('admin', 'admin123');
INSERT INTO tbl_admin VALUES ('test', 'admin123');


-- Create table
CREATE TABLE tbl_category (
    cat_id INT IDENTITY PRIMARY KEY,
    cat_name NVARCHAR(50) NOT NULL UNIQUE,
    cat_image NVARCHAR(MAX) NOT NULL,
    cat_fk_ad INT FOREIGN KEY REFERENCES tbl_admin(ad_id) 
);


-- Create table
CREATE TABLE tbl_user (
    u_id INT IDENTITY PRIMARY KEY,
    u_name NVARCHAR(50) NOT NULL,
    u_email NVARCHAR(50) NOT NULL UNIQUE,
    u_password NVARCHAR(50) NOT NULL,
    u_image NVARCHAR(MAX) NOT NULL,
    u_contact NVARCHAR(50) NOT NULL UNIQUE
);

-- Create table
CREATE TABLE tbl_product (
    pro_id INT IDENTITY PRIMARY KEY,
    pro_name NVARCHAR(50) NOT NULL,
    pro_image NVARCHAR(MAX) NOT NULL,
    pro_des NVARCHAR(MAX) NOT NULL,
    pro_price INT,
    pro_fk_cat INT FOREIGN KEY REFERENCES tbl_category(cat_id),
    pro_fk_user INT FOREIGN KEY REFERENCES tbl_user(u_id)
);
select *from tbl_category