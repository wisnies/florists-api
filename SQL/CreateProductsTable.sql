use florists_manager;

CREATE TABLE products(
	product_id VARCHAR(36) NOT NULL,
    product_name VARCHAR(64) NOT NULL,
    available_quantity INT NOT NULL,
    unit_price DOUBLE NOT NULL,
    sku VARCHAR(16) NOT NULL,
	is_active BOOL NOT NULL,
    category SMALLINT NOT NULL,
    created_at DATETIME NOT NULL,
    updated_at DATETIME NULL,
    PRIMARY KEY (product_id)
);

CREATE TABLE product_inventories(
	product_id VARCHAR(36) NOT NULL,
	inventory_id VARCHAR(36) NOT NULL,
    required_quantity INT NOT NULL,
    created_at DATETIME NOT NULL,
    updated_at DATETIME NULL,
    FOREIGN KEY (product_id) REFERENCES products(product_id),
    FOREIGN KEY (inventory_id) REFERENCES inventories(inventory_id)
);