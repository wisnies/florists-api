use florists_manager;

CREATE TABLE flower_transactions(
	transaction_id VARCHAR(36) NOT NULL,
    flower_id VARCHAR(36) NOT NULL,
    user_id VARCHAR(36) NOT NULL,
    purchase_order_number VARCHAR(36) NULL,
    production_order_number VARCHAR(36) NULL,
    quantity_before INT NOT NULL,
    quantity_after INT NOT NULL,
    transaction_value DOUBLE NOT NULL,
    transaction_type VARCHAR(36) NOT NULL,
    created_at DATETIME NOT NULL,
    updated_at DATETIME NULL,
    PRIMARY KEY (transaction_id),
    FOREIGN KEY (flower_id) REFERENCES flowers(flower_id),
    FOREIGN KEY (user_id) REFERENCES users(user_id)
);