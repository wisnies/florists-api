use florists_manager;

CREATE TABLE inventories(
	inventory_id VARCHAR(36) NOT NULL,
    inventory_name VARCHAR(64) NOT NULL,
    available_quantity INT NOT NULL,
    unit_price DOUBLE NOT NULL,
    category SMALLINT NOT NULL,
    created_at DATETIME NOT NULL,
    updated_at DATETIME NULL,
    PRIMARY KEY (inventory_id)
);