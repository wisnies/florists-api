use florists_manager;

CREATE TABLE flowers(
	flower_id VARCHAR(36) NOT NULL,
    flower_name VARCHAR(128) NOT NULL,
    available_quantity INT NOT NULL,
    unit_price DOUBLE NOT NULL,
    created_at DATETIME NOT NULL,
    updated_at DATETIME NULL,
    PRIMARY KEY (flower_id)
);